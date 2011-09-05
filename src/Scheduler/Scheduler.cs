using System;
using System.Collections;
using System.Threading;
using log4net;
using log4net.Config;
using MindHarbor.Scheduler.Configuration;
using MindHarbor.Scheduler.Exceptions;

namespace MindHarbor.Scheduler {
	/// <summary>
	///  enumeration of Scheduler events used by the delegate
	/// </summary>
	public enum SchedulerEventType {
		CREATED,
		DELETED,
		INVOKED
	} ;

	/// <summary>
	///  delegate for Scheduler events
	/// </summary>
	/// <param name="type"></param>
	/// <param name="scheduleName"></param>
	public delegate void SchedulerEventDelegate(SchedulerEventType type, string scheduleName);

	/// <summary>
	///  This is the main class which will maintain the list of Schedules
	/// and also manage them, like rescheduling, deleting schedules etc.
	/// </summary>
	public sealed class Scheduler {
		// Event raised when for any event inside the scheduler

		// next event which needs to be kicked off,
		// this is set when a new Schedule is added or after invoking a Schedule
		private const string WINDOWINFOFORMAT =
			"\n\n*****************************\n****** {0}*****\n*****************************";

		private static readonly object lockObj = new object();
		private static ILog logger;

		private static Schedule m_nextSchedule = null;
		private static ArrayList m_schedulesList = new ArrayList(); // list of schedles

		private static Timer m_timer = new Timer(new TimerCallback(DispatchEvents), // main timer
		                                         null,
		                                         Timeout.Infinite,
		                                         Timeout.Infinite);

		#region public members

		public static bool IsShutDown {
			get { return m_timer == null; }
		}

		/// <summary>
		/// Get schedule at a particular index in the array list
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static Schedule GetScheduleAt(int index) {
			if (index < 0 || index >= m_schedulesList.Count)
				return null;
			return (Schedule) m_schedulesList[index];
		}

		/// <summary>
		/// Shutdown the scheduler
		/// </summary>
		/// <remarks>
		/// once shutdown, the only way to restart the scheduler is to restart the whole application
		/// </remarks>
		public static void Shutdown() {
			lock (lockObj) {
				if (IsShutDown) return;
				m_timer = null;
				m_schedulesList.Clear();
				m_nextSchedule = null;
				Logger.InfoFormat(WINDOWINFOFORMAT, "Scheduler Shutdown");
				LogManager.Shutdown();
			}
		}

		/// <summary>
		/// Number of schedules in the list
		/// </summary>
		/// <returns></returns>
		public static int Count() {
			return m_schedulesList.Count;
		}

		/// <summary>
		/// Indexer to access a Schedule object by name
		/// </summary>
		/// <param name="scheduleName"></param>
		/// <returns></returns>
		public static Schedule GetSchedule(string scheduleName) {
			for (int index = 0; index < m_schedulesList.Count; index++)
				if (((Schedule) m_schedulesList[index]).Name == scheduleName)
					return (Schedule) m_schedulesList[index];
			return null;
		}

		public static bool Contains(Schedule val) {
			return m_schedulesList.Contains(val);
		}

		public static void AddSchedule(Schedule s) {
			AddSchedule(s, null);
		}

		/// <summary>
		/// add a new schedule
		/// </summary>
		/// <param name="s"></param>
		/// <param name="i"></param>
		public static void AddSchedule(Schedule s, IInterceptor i) {
			if (IsShutDown)
				throw new SchedulerException("Scheduler is already shutdown");
			if (GetSchedule(s.Name) != null)
				throw new SchedulerException("Schedule with the same name already exists");
			m_schedulesList.Add(s);
			if (i != null) {
				s.PreTrigger += new Invoke(i.SchedulePreTrigger);
				s.PostTrigger += new Invoke(i.SchedulePostTrigger);
			}
			m_schedulesList.Sort();

			// adjust the next event time if schedule is added at the top of the list
			if (m_schedulesList[0] == s)
				SetNextEventTime();
			if (OnSchedulerEvent != null)
				OnSchedulerEvent(SchedulerEventType.CREATED, s.Name);
		}

		/// <summary>
		/// remove a schedule object from the list
		/// </summary>
		/// <param name="s"></param>
		public static void RemoveSchedule(Schedule s) {
			m_schedulesList.Remove(s);
			SetNextEventTime();
			if (OnSchedulerEvent != null)
				OnSchedulerEvent(SchedulerEventType.DELETED, s.Name);
		}

		/// <summary>
		///  remove schedule by name
		/// </summary>
		/// <param name="name"></param>
		public static void RemoveSchedule(string name) {
			RemoveSchedule(GetSchedule(name));
		}

		#endregion

		public static string Name {
			get { return SchedulerConfigurationSection.GetInstance().SchedulerName; }
		}

		internal static ILog Logger {
			get {
				if (logger == null) {
					XmlConfigurator.Configure();
					logger = LogManager.GetLogger("MindHarbor.SchedulerLogger");
				}
				return logger;
			}
		}

		public static string InterceptorTypeName {
			get {
				IInterceptor interceptor = SchedulerConfigurationSection.GetInstance().DefaultInterceptor;
				return interceptor != null ? interceptor.GetType().FullName : "None";
			}
		}

		#region private methods

		// call back for the timer function
		private static void DispatchEvents(object obj) // obj ignored
		{
			lock (lockObj) {
				if (m_nextSchedule == null) {
					Logger.Info("Schedule checked, no outstanding schedule found.");
					return;
				}

				Logger.InfoFormat("Start to trigger the schedule: {0}.", m_nextSchedule.Name);
				m_nextSchedule.TriggerEvents(); // make this happen on a thread to let this thread continue

				if (m_nextSchedule.Type == ScheduleType.ONETIME)
					RemoveSchedule(m_nextSchedule); // remove the schedule from the list
				else {
					if (OnSchedulerEvent != null)
						OnSchedulerEvent(SchedulerEventType.INVOKED, m_nextSchedule.Name);
					m_schedulesList.Sort();
					SetNextEventTime();
				}
			}
		}

		// method to set the time when the timer should wake up to invoke the next schedule
		private static void SetNextEventTime() {
			if (m_schedulesList.Count == 0) {
				m_timer.Change(Timeout.Infinite, Timeout.Infinite); // this will put the timer to sleep
				return;
			}
			m_nextSchedule = (Schedule) m_schedulesList[0];
			TimeSpan ts = m_nextSchedule.NextInvokeTime.Subtract(DateTime.Now);
			if (ts < TimeSpan.Zero)
				ts = TimeSpan.Zero; // cannot be negative !
			m_timer.Change((int) ts.TotalMilliseconds, Timeout.Infinite); // invoke after the timespan
		}

		#endregion

		public static event SchedulerEventDelegate OnSchedulerEvent;
	}
}
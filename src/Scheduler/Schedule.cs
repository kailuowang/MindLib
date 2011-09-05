using System;
using System.Threading;
using MindHarbor.Scheduler.Exceptions;

namespace MindHarbor.Scheduler {
	// delegate for OnTrigger() event
	public delegate void Invoke(string scheduleName);

	// enumeration for schedule types
	public enum ScheduleType {
		ONETIME,
		INTERVAL,
		DAILY,
		WEEKLY,
		MONTHLY
	}

	// base class for all schedules
	public abstract class Schedule : IComparable {
		protected bool m_active; // is schedule active ?

		// m_fromTime and m_toTime are used to defined a time range during the day
		// between which the schedule can run.
		// This is useful to define a range of working hours during which a schedule can run
		protected TimeSpan m_fromTime;

		// time interval in seconds used by schedules like IntervalSchedule
		private long m_interval = 0;
		private DateTime? m_lastInovkedTime = null;
		protected string m_name; // name of the schedule
		protected DateTime m_nextTime; // time when this schedule is invoked next, used by scheduler
		protected DateTime m_startTime; // time the schedule starts
		protected DateTime m_stopTime; // ending time for the schedule
		protected TimeSpan m_toTime;
		protected ScheduleType m_type; // type of schedule

		public Schedule(string name, DateTime startTime, ScheduleType type) {
			StartTime = startTime;
			m_nextTime = startTime;
			m_type = type;
			m_name = name;
		}

		// Accessor for type of schedule
		public ScheduleType Type {
			get { return m_type; }
		}

		// Accessor for name of schedule
		// Name is set in constructor only and cannot be changed
		public string Name {
			get { return m_name; }
		}

		public DateTime? LastInovkedTime {
			get { return m_lastInovkedTime; }
		}

		// Method which will return when the Schedule has to be invoked next
		// This method is used by Scheduler for sorting Schedule objects in the list
		public DateTime NextInvokeTime {
			get { return m_nextTime; }
		}

		// Accessor for m_startTime
		public DateTime StartTime {
			get { return m_startTime; }
			set {
				// start time can only be in future
				/*if (value.CompareTo(DateTime.Now) <= 0)
					throw new SchedulerException("Start Time should be in future");*/
				m_startTime = value;
			}
		}

		// Accessor for m_interval in seconds
		// Put a lower limit on the interval to reduce burden on resources
		// I am using a lower limit of 30 seconds
		public long Interval {
			get { return m_interval; }
			set {
				if (value < 1)
					throw new SchedulerException("Interval cannot be less than 1 second");
				if (value < 30)
					Scheduler.Logger.ErrorFormat("WARRNING: Schedule {0} has an interval less than 30 seconds", Name);
				m_interval = value;
			}
		}

		#region IComparable Members

		public int CompareTo(object obj) {
			if (obj is Schedule)
				return m_nextTime.CompareTo(((Schedule) obj).m_nextTime);
			throw new Exception("Not a Schedule object");
		}

		#endregion

		public event Invoke OnTrigger;
		public event Invoke PreTrigger;
		public event Invoke PostTrigger;

		// Constructor

		// Sets the next time this Schedule is kicked off and kicks off events on
		// a seperate thread, freeing the Scheduler to continue
		public void TriggerEvents() {
			CalculateNextInvokeTime(); // first set next invoke time to continue with rescheduling
			m_lastInovkedTime = DateTime.Now;
			ThreadStart ts = new ThreadStart(KickOffEvents);
			Thread t = new Thread(ts);
			t.Start();
		}

		// Implementation of ThreadStart delegate.
		// Used by Scheduler to kick off events on a seperate thread
		private void KickOffEvents() {
			if (OnTrigger != null)
				try {
					if (PreTrigger != null)
						PreTrigger(Name);
					OnTrigger(Name);
				}
				finally {
					if (PostTrigger != null)
						PostTrigger(Name);
				}
		}

		// To be implemented by specific schedule objects when to invoke the schedule next
		internal abstract void CalculateNextInvokeTime();

		// Check to see if the next time calculated is within the time range
		// given by m_fromTime and m_toTime
		// The ranges can be during a day, for eg. 9 AM to 6 PM on same day
		// or overlapping 2 different days like 10 PM to 5 AM (i.e over the night)
		protected bool IsInvokeTimeInTimeRange() {
			if (m_fromTime < m_toTime) // eg. like 9 AM to 6 PM
				return (m_nextTime.TimeOfDay > m_fromTime && m_nextTime.TimeOfDay < m_toTime);
			else // eg. like 10 PM to 5 AM
				return (m_nextTime.TimeOfDay > m_toTime && m_nextTime.TimeOfDay < m_fromTime);
		}

		// IComparable interface implementation is used to sort the array of Schedules
		// by the Scheduler
	}
}
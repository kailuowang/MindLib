using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using MindHarbor.Scheduler.Configuration;

namespace MindHarbor.Scheduler {
	public enum TaskStatus {
		Running,
		Finished,
		Waitting
	}

	/// <summary>
	/// a task to be performed by the scheduler
	/// </summary>
	/// <remarks>
	/// task is going to be saved in staitic field, so avoid keeping heavy weight state (such as entity) in it
	/// </remarks>
	public abstract class Task : ITask {
		private readonly LinkedList<TaskLogEntry> logs = new LinkedList<TaskLogEntry>();
		private readonly LinkedList<TaskLogEntry> errlogs = new LinkedList<TaskLogEntry>();
		private bool isRunning = false;
		private DateTime? lastPerformDone;

		private int logSize = 2000;
		private Schedule schedule = null;

		public DateTime? LastPerformDone {
			get { return lastPerformDone; }
		}
		protected IList<string> errorAlertEmails = new List<string>();
		#region ITask Members

		public event TaskLogEventHandler LogAdded;
		public event EventHandler PostPerformed;
		private string errorAlertFrom;

		public IList<string> ErrorAlertEmails {
			get {
				
				return errorAlertEmails;
			}
			set { errorAlertEmails = value; }
		}

		public string ErrorAlertFrom {
			get { return errorAlertFrom; }
			set { errorAlertFrom = value; }
		}

		public Schedule Schedule {
			get {
				if (schedule == null) {
					schedule = CreateSchedule();
					schedule.OnTrigger += new Invoke(Schedule_OnTrigger);
				}
				return schedule;
			}
			private set { schedule = value; }
		}

		public ScheduleType ScheduleType {
			get { return Schedule.Type; }
		}

		/// <summary>
		/// return the array of LogEntries
		/// </summary>
		/// <remarks>
		///  returns a new array to which all the entries are copied to. 
		/// </remarks>
		public TaskLogEntry[] Logs {
			get {
				return LinkListToArray(logs);
			}
		}

		public TaskLogEntry[] ErrLogs {
			get { return LinkListToArray(errlogs); }
		}

		private static T[] LinkListToArray<T>(ICollection<T> ll) {
			int size = ll.Count;
			T[] retVal = new T[size];
			ll.CopyTo(retVal, 0);
			return retVal;
		}

		public string LatestLogMessage {
			get {
				if (logs.Count > 0)
					return logs.First.Value.Msg;
				else return null;
			}
		}

		public string Name {
			get { return Schedule.Name; }
		}

		public TaskStatus Status {
			get {
				if (isRunning)
					return TaskStatus.Running;
				else
					return TaskManager.TaskIsScheduled(this) ? TaskStatus.Waitting : TaskStatus.Finished;
			}
		}

		/// <summary>
		/// Last time it is invoked
		/// </summary>
		public DateTime? LastInvokedTime {
			get { return Schedule.LastInovkedTime; }
		}

		public DateTime NextInvokeTime {
			get { return Schedule.NextInvokeTime; }
		}

		public DateTime StartTime {
			get { return Schedule.StartTime; }
		}

		public int LogSize {
			get { return logSize; }
			set { logSize = value; }
		}

		public bool IsOutDated {
			get {
				return Status == TaskStatus.Finished &&
				       DateTime.Now > LastPerformDone.Value + new TimeSpan(0, 30, 0);
			}
		}

		public event EventHandler ExceptionOccured;

		#endregion

		private void Schedule_OnTrigger(string scheduleName) {
			lock (this) {
				isRunning = true;
				try {
					AddLog("Task triggered");
					Perform();
				}
				catch (Exception e) {
					//To prevent the exception being swallowed, since it's running on an separate thread
					AddLog("An exception occured. \n " + BuildExceptionMessage(e), true);
					if (ExceptionOccured != null)
						ExceptionOccured(this, new EventArgs());
				}

				if (PostPerformed != null)
					PostPerformed(this, new EventArgs());
				lastPerformDone = DateTime.Now;
				isRunning = false;
				AddLog("Task done");
			}
		}

		private static string BuildExceptionMessage(Exception e) {
			StringBuilder message =
				new StringBuilder("\r\n\r\nUnhandledException logged by MindHarbor.Scheduler.dll:\r\n\r\n");
			for (Exception currentException = e;
			     currentException != null;
			     currentException = currentException.InnerException)
				message.AppendFormat("\r\n\r\ntype={0}\r\n\r\nmessage={1}\r\n\r\nstack=\r\n{2}\r\n\r\n",
				                     currentException.GetType().FullName,
				                     currentException.Message,
				                     currentException.StackTrace);
			return message.ToString();
		}

		protected void AddLog(string log) {
			AddLog(log, false);
		}

		protected void AddLog(string log, bool error) {
			logs.AddFirst(new TaskLogEntry(log));
			string schedulerLog = Name + " - " + log;

			if (error) {
				errlogs.AddFirst(new TaskLogEntry(log));
				Scheduler.Logger.Error(schedulerLog);
				ErrorAlert(schedulerLog);
			}
			else
				Scheduler.Logger.Info(schedulerLog);

			while (LogSize > 0 && logs.Count > LogSize)
				logs.RemoveLast();
			if (LogAdded != null)
				LogAdded(this, new TaskLogEventArgs(logs.First.Value));
		}

		private void ErrorAlert(string msg) {
			if(ErrorAlertEmails.Count  > 0) {
				SmtpClient sc = new System.Net.Mail.SmtpClient();
				try {
					if (!string.IsNullOrEmpty(errorAlertFrom))
						foreach (string s in errorAlertEmails) {
							sc.Send(errorAlertFrom, s, "Error Occurred in " + Schedule.Name + " - when performing " + this.Name , msg );
					}
				}catch(Exception e) {
					Scheduler.Logger.Error("Failed to send error alert Email due to exception: " + e);
				}
			}
		}

		protected abstract void Perform();

		/// <summary>
		/// Create the Schedule for this task
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// IMPORTANT NOTE: the name of the schedule will be used as the name of task. The name should be as unique as possible because no two tasks with the same name can exist in the same <see cref="TaskManager"/>
		/// </remarks>
		protected abstract Schedule CreateSchedule();
	}
}
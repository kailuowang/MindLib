using System;

namespace MindHarbor.Scheduler.Exceptions {
	/// <summary>
	/// Summary description for SchedulerException.
	/// </summary>
	public class SchedulerException : Exception {
		public SchedulerException() {}
		public SchedulerException(string msg) : base(msg) {}
	}
}
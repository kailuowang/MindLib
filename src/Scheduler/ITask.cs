using System;
using System.Collections.Generic;

namespace MindHarbor.Scheduler {
	public interface ITask {
		TaskLogEntry[] Logs { get; }

		string LatestLogMessage { get; }

		string Name { get; }

		TaskStatus Status { get; }

		DateTime? LastInvokedTime { get; }

		DateTime NextInvokeTime { get; }

		DateTime StartTime { get; }

		int LogSize { get; set; }

		Schedule Schedule { get; }

		ScheduleType ScheduleType { get; }

		bool IsOutDated { get; }
		TaskLogEntry[] ErrLogs { get; }
		IList<string> ErrorAlertEmails { get; set; }

		/// <summary>
		/// Gets and sets the Email address from which the error alerts are sent to.
		/// </summary>
		string ErrorAlertFrom { get; set; }

		event TaskLogEventHandler LogAdded;
		event EventHandler PostPerformed;

		event EventHandler ExceptionOccured;
	}
}
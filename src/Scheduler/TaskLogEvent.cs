using System;

namespace MindHarbor.Scheduler {
	public class TaskLogEventArgs : EventArgs {
		private readonly TaskLogEntry logEntry;

		public TaskLogEventArgs(TaskLogEntry log) {
			logEntry = log;
		}

		public TaskLogEntry LogEntry {
			get { return logEntry; }
		}
	}

	public delegate void TaskLogEventHandler(object sender, TaskLogEventArgs arg);
}
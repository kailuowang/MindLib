using System;

namespace MindHarbor.Scheduler {
	public class TaskLogEntry {
		private DateTime creationTime;
		private string msg;

		public TaskLogEntry(string msg) {
			this.msg = msg;
			creationTime = DateTime.Now;
		}

		public string Msg {
			get { return msg; }
			private set { msg = value; }
		}

		public DateTime CreationTime {
			get { return creationTime; }
			private set { creationTime = value; }
		}

		public override string ToString() {
			return creationTime + " - " + msg;
		}
	}
}
using System;

namespace MindHarbor.GenClassLib.ProgressTracking {
	///<summary>
	///</summary>
	public class ProgressChangedArgs : EventArgs {
		private string message;

		public ProgressChangedArgs(string message) {
			this.message = message;
		}

		public ProgressChangedArgs() {}

		public string Message {
			get { return message; }
			set { message = value; }
		}
	}
}
using System;

namespace MindHarbor.Scheduler {
	/// <summary>
	/// a onetime scheduler task for asynchronized process
	/// </summary>
	/// <remarks>
	/// inherit this class to create a task to be performed asynchronizly. 
	/// You'll need to override the Perform method and PercentageProgress
	/// if it is NHibernate related please remember to flush the Nhibernate session 
	/// </remarks>
	public abstract class AsynchronizedProcessTask : Task, ITaskWithProgressInfo {
		private readonly string name;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">must be unique application wide.</param>
		public AsynchronizedProcessTask(string name) {
			this.name = name;
		}

		#region ITaskWithProgressInfo Members

		public abstract float PercentageProgress { get; }

		#endregion

		public static T GetInstance<T>(string name) where T : AsynchronizedProcessTask {
			return (T) TaskManager.GetTaskByName(name);
		}

		public void Start() {
			TaskManager.AddTask(this, true);
		}

		protected override Schedule CreateSchedule() {
			return Schedules.OneTimeSchedule(name, DateTime.Now);
		}
	}
}
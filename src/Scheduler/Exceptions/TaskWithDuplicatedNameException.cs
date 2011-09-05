namespace MindHarbor.Scheduler.Exceptions {
	public class TaskWithDuplicatedNameException : SchedulerException {
		public TaskWithDuplicatedNameException() : base() {}
		public TaskWithDuplicatedNameException(string msg) : base(msg) {}
	}
}
namespace MindHarbor.Scheduler {
	public interface ITaskWithProgressInfo : ITask {
		float PercentageProgress { get; }
	}
}
namespace MindHarbor.Scheduler {
	public interface IInterceptor {
		/// <summary>
		/// Called on <see cref="Schedule.PreTrigger"/>
		/// </summary>
		/// <param name="scheduleName"></param>
		void SchedulePreTrigger(string scheduleName);

		/// <summary>
		/// Called on <see cref="Schedule.PostTrigger"/>
		/// </summary>
		/// <param name="scheduleName"></param>
		void SchedulePostTrigger(string scheduleName);
	}
}
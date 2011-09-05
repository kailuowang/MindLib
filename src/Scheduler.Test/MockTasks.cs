using System;

namespace MindHarbor.Scheduler.Test {
	public abstract class MockTaskBase : Task {
		private static int count = 0;

		public static int Count {
			get { return count; }
		}

		protected override void Perform() {
			Console.WriteLine(Schedule.Name + " performed at " + DateTime.Now);
			count++;
		}
	}

	public class MockIntervalTask : MockTaskBase {
		private static readonly MockIntervalTask instance = new MockIntervalTask();

		private MockIntervalTask() {}

		public static MockIntervalTask Instance {
			get { return instance; }
		}

		protected override Schedule CreateSchedule() {
			return
				new IntervalSchedule(" Interval Mock Task", DateTime.Now, 1, TimeSpan.Zero, new TimeSpan(24, 0, 0));
		}
	}

	public class MockDailyTask : MockTaskBase {
		protected override Schedule CreateSchedule() {
			return
				new DailySchedule(" Daily Mock Task", DateTime.Now);
		}
	}

	public class MockWeeklyTask : MockTaskBase {
		protected override Schedule CreateSchedule() {
			return
				new WeeklySchedule(" Weekly Mock Task", DateTime.Now);
		}
	}

	public class MockShutDownTask : MockTaskBase {
		protected override Schedule CreateSchedule() {
			return
				new IntervalSchedule(" ShutDown Mock Task", DateTime.Now, 1, TimeSpan.Zero, new TimeSpan(24, 0, 0));
		}

		protected override void Perform() {
			Console.WriteLine("Trying to Shutdown the Scheduler");
			TaskManager.ShutDown();
		}
	}
}
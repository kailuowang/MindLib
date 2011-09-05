using System.Threading;
using MindHarbor.Scheduler.Exceptions;
using NUnit.Framework;

namespace MindHarbor.Scheduler.Test {
	[TestFixture]
	public class TaskManagerFixture {
		[Test]
		public void MockTasksStartTest() {
			Assert.IsTrue(TaskManager.Tasks.Count > 0);
			int testSeconds = 3;
			Thread.Sleep(testSeconds*1000);
			Assert.IsTrue(MockTaskBase.Count > testSeconds - 1);
			Assert.IsTrue(MockDailyTask.Count > 0);
		}

		[Test, Explicit]
		public void ShutdownTest() {
			Assert.IsTrue(TaskManager.Tasks.Count > 0);
			TaskManager.ShutDown();
			try {
				TaskManager.StartAll();
				Assert.Fail("Scheduler Exception should've been caught.");
			}
			catch (SchedulerException) {}
			Assert.AreEqual(0, TaskManager.Tasks.Count);
		}

		[Test]
		public void TaskLoadTest() {
			Assert.IsTrue(TaskManager.Tasks.Count > 0);
			Assert.AreEqual("TestScheduler", Scheduler.Name);
			Assert.AreEqual(Scheduler.Count(), TaskManager.Tasks.Count);
			Assert.IsTrue(TaskManager.Tasks.Contains(MockIntervalTask.Instance));
		}

		[Test, Explicit]
		public void TaskShutDownSchedulerTest() {
			TaskManager.AddTask(new MockShutDownTask(), true);
			Thread.Sleep(3*1000);
			Assert.IsTrue(TaskManager.IsShutDown);
		}
	}
}
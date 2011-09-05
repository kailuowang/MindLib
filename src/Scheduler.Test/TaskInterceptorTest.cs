using System.Threading;
using NUnit.Framework;

namespace MindHarbor.Scheduler.Test {
	[TestFixture]
	public class TaskInterceptorTest {
		[Test]
		public void InterceptorLoadTest() {
			//This line is required to start the Scheduler
			Assert.IsTrue(TaskManager.Tasks.Count > 0);
			int testSeconds = 3;
			Thread.Sleep(testSeconds*1000);
			Assert.Greater(MockInterceptor2.PostTriggersHandled, testSeconds - 1, "PostTrigger didn't get handled");
			Assert.Greater(MockInterceptor2.PreTriggersHandled, testSeconds - 1, "PreTrigger didn't get handled");
		}
	}
}
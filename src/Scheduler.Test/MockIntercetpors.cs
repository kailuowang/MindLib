using System;
using log4net;

namespace MindHarbor.Scheduler.Test {
	public class MockIntercetporBase : IInterceptor {
		private static int postTriggersHandled = 0;
		private static int preTriggersHandled = 0;
		private ILog logger = LogManager.GetLogger("MockInterceptorLogger");

		public static int PreTriggersHandled {
			get { return preTriggersHandled; }
		}

		public static int PostTriggersHandled {
			get { return postTriggersHandled; }
		}

		#region IInterceptor Members

		public void SchedulePreTrigger(string scheduleName) {
			string message = GetType().Name + " called on pre trigger for schedule " + scheduleName;
			Console.WriteLine(message);
			logger.Info(message);
			preTriggersHandled++;
		}

		public void SchedulePostTrigger(string scheduleName) {
			string message = GetType().Name + " called on post trigger for schedule " + scheduleName;
			Console.WriteLine(message);
			logger.Info(message);
			postTriggersHandled++;
		}

		#endregion
	}

	public class MockInterceptor1 : MockIntercetporBase {}

	public class MockInterceptor2 : MockIntercetporBase {}
}
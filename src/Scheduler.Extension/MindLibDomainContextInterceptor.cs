using System.Collections.Specialized;
using NHibernate.Burrow;

namespace MindHarbor.Scheduler.Extension {
	/// <summary>
	/// Schedule Interceptor that manages a NHibernate Transaction for one schedule invokement 
	/// </summary>
	public class MindLibDomainContextInterceptor : IInterceptor {
		private BurrowFramework bf = new BurrowFramework();

		#region IInterceptor Members

		public void SchedulePreTrigger(string scheduleName) {
			bf.InitWorkSpace(true, new NameValueCollection(), string.Empty);
		}

		public void SchedulePostTrigger(string scheduleName) {
			bf.CloseWorkSpace();
		}

		#endregion
	}
}
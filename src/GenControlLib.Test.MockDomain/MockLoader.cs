using System;
using MindHarbor.GenInterfaces;

namespace MindHarbor.GenControlLib.Test.MockDomain {
	/// <summary>
	/// Summary description for MockLoader
	/// </summary>
	public class MockLoader : ILoader {
		private static int loadCount = 0;

		public MockLoader() {
			//
			// TODO: Add constructor logic here
			//
		}

		#region ILoader Members

		public object Load(Type t, object id) {
			loadCount++;
			MockObject retVal = new MockObject((int) id);
			retVal.LoadTimes = loadCount;
			retVal.LoadAsType = t.Name;
			return retVal;
		}

		#endregion
	}
}
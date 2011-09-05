namespace MindHarbor.GenControlLib.Test.MockDomain {
	/// <summary>
	/// Summary description for MockObject
	/// </summary>
	public class MockObject {
		private int id;
		private string loadAsType;
		private int loadTimes;

		public MockObject(int id) {
			this.id = id;
		}

		public string LoadAsType {
			get { return loadAsType; }
			set { loadAsType = value; }
		}

		public int LoadTimes {
			get { return loadTimes; }
			set { loadTimes = value; }
		}

		public int Id {
			get { return id; }
			set { id = value; }
		}
	}
}
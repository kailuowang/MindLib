namespace MindHarbor.SingletonUtil.Test {
	public class MockPersistantWithSingleton {
		private int id;

		private IStrategy strategy;

		public int Id {
			get { return id; }
			set { id = value; }
		}

		public IStrategy Strategy {
			get { return strategy; }
			set { strategy = value; }
		}
	}
}
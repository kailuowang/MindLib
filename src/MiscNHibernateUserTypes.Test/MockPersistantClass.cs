namespace MindHarbor.MiscNHibernateUserTypes.Test {
	public class MockPersistantClass {
		private int id;

		private string password;

		public int Id {
			get { return id; }
			set { id = value; }
		}

		public string Password {
			get { return password; }
			set { password = value; }
		}
	}
}
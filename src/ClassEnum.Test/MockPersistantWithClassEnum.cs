namespace MindHarbor.ClassEnum.Test {
	public class MockPersistantWithClassEnum {
		private int id;

		private MockEnum1 mEnum;

		public int Id {
			get { return id; }
			set { id = value; }
		}

		public MockEnum1 MEnum {
			get { return mEnum; }
			set { mEnum = value; }
		}
	}
}
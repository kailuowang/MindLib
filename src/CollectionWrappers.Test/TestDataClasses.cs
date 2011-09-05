namespace MindHarbor.CollectionWrappers.Test {
	public class Foo {
		private Bar myVar;

		public Bar MyProperty {
			get { return myVar; }
			set { myVar = value; }
		}
	}

	public class Bar {}
}
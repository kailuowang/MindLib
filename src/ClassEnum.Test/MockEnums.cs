using System.Collections.Generic;

namespace MindHarbor.ClassEnum.Test {
	public class MockEnum1 : ClassEnumGeneric<MockEnum1> {
		public static readonly MockEnum1 Item1 = new MockEnum1("item1");
		public static readonly MockEnum1 Item2 = new MockEnum1("item2");

		private MockEnum1(string name) : base(name) {}

		public static ICollection<MockEnum1> AllItems {
			get { return Items; }
		}
	}

	public class MockEnum1UserType : ClassEnumUserType<MockEnum1> {}

	public class MockEnum2 : ClassEnumGeneric<MockEnum2> {
		public static readonly MockEnum2 Item1 = new MockEnum2("item1");
		public static readonly MockEnum2 Item2 = new MockEnum2("item2");

		private MockEnum2(string name) : base(name) {}
	}

	public class CompositeEnum : ClassEnumGeneric<CompositeEnum> {
		public static readonly CompositeEnum Item1 = new CompositeEnum1("item1");
		public static readonly CompositeEnum Item2 = new CompositeEnum2("item2");
		public CompositeEnum(string name) : base(name) {}

		#region Nested type: CompositeEnum1

		private class CompositeEnum1 : CompositeEnum {
			public CompositeEnum1(string name) : base(name) {}
		}

		#endregion

		#region Nested type: CompositeEnum2

		private class CompositeEnum2 : CompositeEnum {
			public CompositeEnum2(string name) : base(name) {}
		}

		#endregion
	}

	public class ExtendableEnum : ClassEnumGeneric<ExtendableEnum> {
		protected ExtendableEnum(string name) : base(name) {}
	}

	public class ExtendedEnum : ExtendableEnum {
		public static ExtendedEnum Item1 = new ExtendedEnum("Item1");
		public static ExtendedEnum Item2 = new ExtendedEnum("Item2");
		private ExtendedEnum(string name) : base(name) {}
	}
}
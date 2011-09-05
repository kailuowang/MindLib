using System;
using NUnit.Framework;

namespace MindHarbor.ClassEnum.Test {
	[TestFixture]
	public class BasicFixture {
		[Test]
		public void CompositeEnumTest() {
			Assert.AreEqual(CompositeEnum.Items.Count, 2);
		}

		[Test]
		public void EqualityTest() {
			Assert.IsNotNull(MockEnum1.Item1);
			Assert.IsNotNull(MockEnum1.Item2);
			Assert.IsNotNull(MockEnum2.Item1);
			Assert.IsNotNull(MockEnum2.Item2);

			Assert.AreNotEqual(MockEnum1.Item1, MockEnum1.Item2);
			Assert.AreNotEqual(MockEnum2.Item1, MockEnum1.Item1);
			Assert.AreNotEqual(MockEnum2.Item1, MockEnum1.Item2);
			Assert.AreNotEqual(MockEnum1.Item1, MockEnum2.Item2);

			Assert.AreNotEqual(CompositeEnum.Item1, MockEnum2.Item2);
			Assert.AreNotEqual(MockEnum1.Item1, CompositeEnum.Item2);
		}

		[Test]
		public void ExtendableEnumAutoInitTest() {
			Assert.AreEqual(ExtendableEnum.Items.Count, 2);
			Assert.IsNotNull(ExtendableEnum.Parse("Item2"));
		}

		[Test]
		public void FindWithoutInitializingTest() {
			Type t = Type.GetType("MindHarbor.ClassEnum.Test.MockEnum1");
			Assert.IsNotNull(ClassEnumBase.Parse(t, "item1"));
		}

		[Test]
		public void ItemsTest() {
			Assert.AreEqual(2, MockEnum1.Items.Count);
		}

		[Test]
		public void ParseTest() {
			MockEnum1.Parse("item1");
			MockEnum1 parsedItem1 = MockEnum1.Parse(MockEnum1.Item1.Name);
			MockEnum1 parsedItem2 = MockEnum1.Parse(MockEnum1.Item2.Name);
			Assert.AreSame(parsedItem1, MockEnum1.Item1);
			Assert.AreSame(parsedItem2, MockEnum1.Item2);

			Assert.AreSame(CompositeEnum.Parse(CompositeEnum.Item2.Name), CompositeEnum.Item2);
			Assert.AreNotSame(CompositeEnum.Parse(CompositeEnum.Item1.Name), CompositeEnum.Item2);
		}
	}
}
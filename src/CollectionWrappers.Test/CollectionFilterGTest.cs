using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace MindHarbor.CollectionWrappers.Test {
	[TestFixture]
	public class CollectionFilterGTest {
		[Test]
		public void Test() {
			TestClass1 c1 = new TestClass1();
			TestClass2 c2 = new TestClass2();
			ICollection<TestInterface> toWrap = new List<TestInterface>();
			toWrap.Add(c1);
			toWrap.Add(c2);
			ICollection<TestClass1> test = new CollectionFilterG<TestClass1, TestInterface>(toWrap);
			Assert.AreEqual(1, test.Count);

			foreach (TestClass1 class1 in test) Assert.AreSame(c1, class1);
			IEnumerator ie = ((IEnumerable) test).GetEnumerator();
			while (ie.MoveNext())
				Assert.AreSame(c1, ie.Current);

			TestClass1[] array = new TestClass1[1];
			test.CopyTo(array, 0);
			foreach (TestClass1 class1 in array)
				Assert.AreSame(c1, class1);
		}
	}

	public class TestClass1 : TestInterface {}

	public class TestClass2 : TestInterface {}

	public interface TestInterface {}
}
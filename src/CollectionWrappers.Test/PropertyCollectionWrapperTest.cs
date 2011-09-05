using System.Collections.Generic;
using NUnit.Framework;

namespace MindHarbor.CollectionWrappers.Test {
	[TestFixture]
	public class PropertyCollectionWrapperTest : TestBaseWithTestEntity {
		[Test]
		public void CollectionTest() {
			ICollection<Bar> c = new PropertyCollectionWrapper<Foo, Bar>(list, "MyProperty");
			Assert.AreEqual(4, c.Count);
			Assert.IsTrue(c.IsReadOnly);
			Assert.IsTrue(c.Contains(te2));
		}
        
        [Test]
		public void ListTest() {
			IList<Bar> c = new PropertyListWrapper<Foo, Bar>(list, "MyProperty");
			Assert.AreEqual(4, c.Count);
			Assert.IsTrue(c.IsReadOnly);
			Assert.IsTrue(c.Contains(te2));
            Assert.AreEqual(te4, c[3]);
		}
	}
}
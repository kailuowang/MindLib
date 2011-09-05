using System.Collections.Generic;
using NUnit.Framework;

namespace MindHarbor.CollectionWrappers.Test {
	[TestFixture]
	public class SearchableCollectionFixture : TestBaseWithTestEntity {
		[Test]
		public void ChildrenCollectionTest() {
			Parent p = new Parent(list);
			Assert.AreEqual(tec4, p.SearchableChildren.Search(te4));
			Assert.AreEqual(tec3, p.SearchableChildren.Search(te3));
			Assert.AreEqual(tec2, p.SearchableChildren.Search(te2));
			Assert.AreEqual(tec1, p.SearchableChildren.Search(te1));
			Assert.IsNull(p.SearchableChildren.Search(new Bar()));
		}

		[Test]
		public void DecoratorTest() {
			SearchableCollectionDecorator<Bar, Foo> scd =
				new SearchableCollectionDecorator<Bar, Foo>(list, "MyProperty");
			Assert.AreEqual(tec4, scd.Search(te4));
			Assert.AreEqual(tec3, scd.Search(te3));
			Assert.AreEqual(tec2, scd.Search(te2));
			Assert.AreEqual(tec1, scd.Search(te1));
			Assert.IsNull(scd.Search(new Bar()));
		}
	}

	public class Parent {
		private ICollection<Foo> children;
		private SearchableChildrenCollection<Bar, Foo> searchableChildren;

		public Parent(ICollection<Foo> children) {
			this.children = children;
		}

		public ISearchableCollection<Bar, Foo> SearchableChildren {
			get {
				if (searchableChildren == null)
					searchableChildren = new SearchableChildrenCollection<Bar, Foo>("MyProperty", this, "children");
				return searchableChildren;
			}
		}
	}
}
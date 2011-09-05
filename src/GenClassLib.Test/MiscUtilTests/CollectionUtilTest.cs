using System.Collections.Generic;
using Iesi.Collections;
using MindHarbor.GenClassLib.MiscUtil;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class CollectionUtilTest {
		[Test]
		public void GetFirstLastTest() {
			ISet set = new SortedSet();
			set.Add("2");
			set.Add("1");
			set.Add("4");
			set.Add("3");
			Assert.AreEqual(CollectionUtil.GetFirst(set), "1");
			Assert.AreEqual(CollectionUtil.GetLast(set), "4");
		}

		[Test]
		public void SortTest() {
			MockSortable m1 = new MockSortable("a");
			MockSortable m2 = new MockSortable("b");
			MockSortable m3 = new MockSortable("c");
			MockSortable m4 = new MockSortable("d");

			IList<IMockSortable> toSort = new List<IMockSortable>();
			toSort.Add(m3);
			toSort.Add(m2);
			toSort.Add(m4);
			toSort.Add(m1);

			IList<IMockSortable> sorted = CollectionUtil.Sort(toSort, "Name");
			Assert.AreEqual(m1, sorted[0]);
			Assert.AreEqual(m2, sorted[1]);
			Assert.AreEqual(m3, sorted[2]);
			Assert.AreEqual(m4, sorted[3]);

			IList<IMockSortable> sorted2 = CollectionUtil.Sort(toSort, "Name DESC");
			Assert.AreEqual(m4, sorted2[0]);
			Assert.AreEqual(m3, sorted2[1]);
			Assert.AreEqual(m2, sorted2[2]);
			Assert.AreEqual(m1, sorted2[3]);
		}
	}

	public class MockSortable : IMockSortable {
		private string name;

		public MockSortable(string n) {
			Name = n;
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public override string ToString() {
			return Name;
		}
	}

	public interface IMockSortable {}
}
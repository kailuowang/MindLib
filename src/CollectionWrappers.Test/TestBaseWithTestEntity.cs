using System.Collections.Generic;
using NUnit.Framework;

namespace MindHarbor.CollectionWrappers.Test {
	public class TestBaseWithTestEntity {
		protected IList<Foo> list;
		protected Bar te1;
		protected Bar te2;
		protected Bar te3;
		protected Bar te4;
		protected Foo tec1;
		protected Foo tec2;
		protected Foo tec3;
		protected Foo tec4;

		[SetUp]
		public void Setup() {
			list = new List<Foo>();
			tec1 = new Foo();
			te1 = new Bar();
			tec1.MyProperty = te1;
			list.Add(tec1);
			tec2 = new Foo();
			te2 = new Bar();
			tec2.MyProperty = te2;
			list.Add(tec2);

			tec3 = new Foo();
			te3 = new Bar();
			tec3.MyProperty = te3;
			list.Add(tec3);

			tec4 = new Foo();
			te4 = new Bar();
			tec4.MyProperty = te4;
			list.Add(tec4);
		}
	}
}
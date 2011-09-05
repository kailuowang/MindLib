using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.LearningTests {
	[TestFixture]
	public class TypeFixture {
		[Test]
		public void Test() {
			MockSuper ms = new MockSuper();
			MockChild mc = new MockChild();
			MockGrandChild mgc = new MockGrandChild();

			Assert.IsTrue(typeof (MockChild).IsInstanceOfType(mc));
			Assert.IsTrue(typeof (MockChild).IsInstanceOfType(mgc));
			Assert.IsFalse(typeof (MockChild).IsInstanceOfType(ms));
		}
	}

	public class MockSuper {}

	public class MockChild : MockSuper {}

	public class MockGrandChild : MockChild {}
}
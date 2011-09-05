using MindHarbor.GenClassLib.MiscUtil;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class InterfaceCopyTest {
		[Test]
		public void CopyIContactInfoTest() {
			ContactInfo c1 = new ContactInfo();
			Address a = new Address();
			a.Street = "1231 blah";
			c1.Address = a;
			c1.Email = "gal@dfa.com";

			ContactInfo c2 = new ContactInfo();
			c2.Email = "gal@aaaaadfa.com";
			c2.Address = new Address();
			InterfaceCopier.CopyIContactInfo(c1, c2);
			Assert.AreEqual(c1.Email, c2.Email);
		}
	}
}
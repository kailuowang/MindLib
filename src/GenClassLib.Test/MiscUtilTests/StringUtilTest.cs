using MindHarbor.GenClassLib.MiscUtil;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class StringUtilTest {
		private static void TestNameMikeBlaze(string name1) {
			string[] result1 = StringUtil.SplitName(name1);
			Assert.AreEqual(result1[0], "Blaze");
			Assert.AreEqual(result1[1], "Mike");
		}

		[Test]
		public void AddressStringTest() {
			Address a = new Address();
			a.Street = "street1";
			a.State = "GA";
			a.City = "Dunwoody";
			a.Zip = "30338";
			Assert.AreEqual(StringUtil.ToString(a, false), "street1\r\nDunwoody GA 30338");
		}

		[Test]
		public void CamelCaseBreakTest() {
			Assert.AreEqual("Bank Of America", StringUtil.BreakCamelCase("BankOfAmerica"));
			Assert.AreEqual("Diet Coke", StringUtil.BreakCamelCase("DietCoke"));
			Assert.AreEqual("IBM Company", StringUtil.BreakCamelCase("IBMCompany"));
			Assert.AreEqual("Nanjing C", StringUtil.BreakCamelCase("NanjingC"));
			Assert.AreEqual("Company IBM", StringUtil.BreakCamelCase("Company IBM"));
		}

		[Test]
		public void NameSplitTest() {
			TestNameMikeBlaze("Mike Blaze");
			TestNameMikeBlaze("Blaze,Mike");
			TestNameMikeBlaze("Blaze, Mike");

			string[] result1 = StringUtil.SplitName("Jr. Mike Blaze");
			Assert.AreEqual(result1[0], "Blaze");
			Assert.AreEqual(result1[1], "Jr. Mike");
		}
	    [Test]
	    public void ToStringTest(){
	        Assert.AreEqual("12,332.4",StringUtil.ToString(12332.4f, "#,###.##"));
	        Assert.AreEqual("$12.40",StringUtil.ToString(12.4f, "C"));
	        Assert.AreEqual("334d",StringUtil.ToString("334d", ""));
	    }
	}
}
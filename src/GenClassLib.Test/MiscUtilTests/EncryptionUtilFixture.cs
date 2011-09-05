using MindHarbor.GenClassLib.MiscUtil;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class EncryptionUtilFixture : TestFixtrueBase {
		[Test, Explicit]
		public void PerformanceTest() {
			string key = "MHA234@@$AV^&*^(ere5MHcc";
			for (int i = 0; i < 10000; i++) {
				string testString = RandomName();
				string result = EncryptionUtil.Encrypt(testString, key);
				Assert.AreEqual(testString, EncryptionUtil.Decrypt(result, key));
			}
		}

		[Test]
		public void SymmetricEncryptionTest() {
			string key = "MHA234@@$AV^&*^(ere5MHcc";
			string testString = RandomName();

			string result = EncryptionUtil.Encrypt(testString, key);
			Assert.AreEqual(testString, EncryptionUtil.Decrypt(result, key));
		}
	}
}
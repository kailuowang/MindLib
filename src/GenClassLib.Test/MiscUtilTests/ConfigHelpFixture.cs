using MindHarbor.GenClassLib.MiscUtil;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class ConfigHelpFixture {
		[Test]
		public void StoreSettingTest() {
			Assert.AreEqual("defaultValue", ConfigHelperBase.GetSetting("k", "defaultValue"));
			ConfigHelperBase.SetSetting("k", "SetValue");
			Assert.AreEqual("SetValue", ConfigHelperBase.GetSetting("k"));
		}
	}
}
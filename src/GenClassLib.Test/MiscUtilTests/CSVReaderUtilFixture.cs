using System.Data;
using MindHarbor.GenClassLib.MiscUtil;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class CSVReaderUtilFixture  {
		[Test]
		public void ReadHeadLessTest() {
			CSVReaderUtil cu = new CSVReaderUtil("MiscUtilTests\\TestFile\\test.csv", CSVReaderUtil.DelimitedMode.CSV);
			DataSet ds = cu.Read(false);
			Assert.AreEqual(1, ds.Tables.Count);
			Assert.AreEqual(89, ds.Tables[0].Rows.Count);
			Assert.AreEqual("MAYRA MORLA MORLA?", ds.Tables[0].Rows[88][2]);
		}
	}
}
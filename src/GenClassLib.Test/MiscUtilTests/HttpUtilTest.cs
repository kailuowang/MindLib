using System;
using System.Web;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	[TestFixture]
	public class HttpUtilTest {
		[Test]
		public void HtmlEncodeTest() {
			Console.WriteLine(HttpUtility.UrlEncode("asdfas<br>dfasd"));
		}
	}
}
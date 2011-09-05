using System;
using NUnit.Framework;

namespace MindHarbor.TimeDataUtil.Test {
	[TestFixture]
	public class LearningFixture {
		[Test]
		public void DateTimeAddYearTest() {
			DateTime d = new DateTime(2004, 2, 28);
			Assert.AreEqual(new DateTime(2003, 2, 28), d.AddYears(-1));
			d = new DateTime(2004, 2, 29);
			Assert.AreEqual(new DateTime(2003, 2, 28), d.AddYears(-1));
		}
	}
}
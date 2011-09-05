using System;
using NUnit.Framework;

namespace MindHarbor.TimeDataUtil.Test {
	[TestFixture]
	public class DateRangeFixture {
		private DateTime? d1 = new DateTime(2000, 1, 1);
		private DateTime? d2 = new DateTime(2000, 1, 2);
		private DateTime? d3 = new DateTime(1999, 12, 31);

		[Test]
		public void ComparisonTest() {
			DateRange sd = new DateRange(d1, d1);
			Assert.IsTrue(sd.Includes(d1.Value.AddHours(12)));
			Assert.IsFalse(sd.Includes(d2.Value));
		}

		[Test]
		public void CreationTest() {
			try {
				DateRange sd = new DateRange(d2, d1);
				Assert.Fail("ArgumentOutOfRangeException should have been thrown");
			}
			catch (ArgumentException e) {
				Console.Write(e);
			}
		}

		[Test]
		public void EqualityTest() {
			DateRange sd = new DateRange(d1, d1);
			DateRange sd2 = new DateRange(d1, d1);
			Assert.AreEqual(sd, sd2);

			sd = new DateRange(d1, d2);
			sd2 = new DateRange(d1, d1);
			Assert.AreNotEqual(sd, sd2);

			sd = new DateRange(null, d1);
			sd2 = new DateRange(null, d1);
			Assert.AreEqual(sd, sd2);

			sd = new DateRange(d1, null);
			sd2 = new DateRange(d1, null);
			Assert.AreEqual(sd, sd2);

			sd = new DateRange(null, d1);
			sd2 = new DateRange(d1, null);
			Assert.AreNotEqual(sd, sd2);

			sd = new DateRange(null, null);
			sd2 = new DateRange(d1, null);
			Assert.AreNotEqual(sd, sd2);
		}

		[Test]
		public void LargerEqualTest() {
			DateRange dr1 = new DateRange(d1, d2);
			DateRange dr2 = new DateRange(d1, d2);
			DateRange dr3 = new DateRange(d3, d2);
			DateRange dr4 = new DateRange(d3, d1);

			Assert.IsTrue(dr1.LargerOrEqual(dr2));

			Assert.IsTrue(dr3.LargerOrEqual(dr1));
			Assert.IsTrue(dr3.LargerOrEqual(dr4));
			Assert.IsFalse(dr4.LargerOrEqual(dr3));
			Assert.IsFalse(dr2.LargerOrEqual(dr4));
			Assert.IsFalse(dr2.LargerOrEqual(dr3));
		}

		[Test]
		public void QuarterTest() {
			DateRange qt1 = new DateRange(new DateTime(2000, 1, 1), new DateTime(2000, 3, 31));
			DateRange qt2 = new DateRange(new DateTime(2000, 4, 1), new DateTime(2000, 6, 30));
			DateRange qt3 = new DateRange(new DateTime(2000, 7, 1), new DateTime(2000, 9, 30));
			DateRange qt4 = new DateRange(new DateTime(2000, 10, 1), new DateTime(2000, 12, 31));

			Assert.AreEqual(qt1, DateRange.Quarter(2000, 1));
			Assert.AreEqual(qt2, DateRange.Quarter(2000, 2));
			Assert.AreEqual(qt3, DateRange.Quarter(2000, 3));
			Assert.AreEqual(qt4, DateRange.Quarter(2000, 4));

			Assert.AreEqual(qt1, DateRange.QuarterOf(2000, 1));
			Assert.AreEqual(qt2, DateRange.QuarterOf(2000, 5));
			Assert.AreEqual(qt3, DateRange.QuarterOf(2000, 9));
			Assert.AreEqual(qt4, DateRange.QuarterOf(2000, 10));
		}

		[Test]
		public void SpecialDateRangeTest() {
			DateRange month = new DateRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 31));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2000, 1, 15)));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2000, 1, 1)));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2000, 1, 31)));

			month = new DateRange(new DateTime(2004, 2, 1), new DateTime(2004, 2, 29));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2004, 2, 29)));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2004, 2, 1)));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2004, 2, 2)));
			Assert.AreEqual(month, DateRange.MonthOf(new DateTime(2004, 2, 28)));

			DateTime? firstDayOfThisMonth = DateRange.ThisMonth.Start;
			DateTime? lastDayOfThisMonth = DateRange.ThisMonth.End;
			Assert.AreEqual(DateTime.Today.Month, firstDayOfThisMonth.Value.Month);
			Assert.AreEqual(DateTime.Today.Year, firstDayOfThisMonth.Value.Year);
			Assert.AreEqual(1, firstDayOfThisMonth.Value.Day);
			Assert.AreEqual(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), lastDayOfThisMonth.Value.Day);
		}
	}
}
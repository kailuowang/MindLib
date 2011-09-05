using System;

namespace MindHarbor.TimeDataUtil {
	/// <summary>
	/// A timeRange of days
	/// </summary>
	/// <remarks>
	/// the basic unit of this DateRange is day. 
	/// </remarks>
	[Serializable]
	public class DateRange : DateTimeRange {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public DateRange(DateTime? start, DateTime? end)
			: this(start, end, true) {}

		/// <summary>
		/// Note that the end date will actually be the start second of the next date
		/// so there is always an at least 24 hours TimeSpan for a DateTimeRange.
		/// </summary>
		/// <param name="end">the last day that should be included in the range</param>
		/// <param name="start">the first day that should be inlcuded in the range</param>
		/// <param name="includeDays"> whether to include the whole day time of the start and end date</param>
		/// <remarks>The last millisecond of the last day is out of the DateRange</remarks>
		private DateRange(DateTime? start, DateTime? end, bool includeDays)
			: base(start != null && includeDays ? start.Value.Date : start,
			       end != null && includeDays ? end.Value.Date.AddDays(1).AddMilliseconds(-1) : end) {}

		/// <summary>
		/// The default format string when ToString()
		/// </summary>
		protected override string DefaultFormatString {
			get { return "d"; }
		}

		///<summary>
		///</summary>
		public static DateRange ThisMonth {
			get { return MonthOf(DateTime.Today); }
		}

		///<summary>
		///</summary>
		public static DateRange LastMonth {
			get { return MonthOf(DateTime.Today.AddMonths(-1)); }
		}

		///<summary>
		///</summary>
		public static DateRange NextMonth {
			get { return MonthOf(DateTime.Today.AddMonths(1)); }
		}

		/// <summary>
		/// Parse the <see cref="DateRange"/> from a string representation <paramref name="val"/>
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static DateRange Parse(string val) {
			string[] values = val.Split(new string[] {STRING_SEPERATOR}, StringSplitOptions.None);
			if (values.Length != 2)
				throw new ArgumentException(val + " is not in a valide format");
			return new DateRange(NullSafeParse(values[0]), NullSafeParse(values[1]));
		}

		private static DateTime? NullSafeParse(string val) {
			if (string.IsNullOrEmpty(val) || val.Trim().Equals(NULL_DATETIME_STRING))
				return null;
			return DateTime.Parse(val);
		}

		/// <summary>
		/// Note that if you are using a special date range, this method will just add/remove years
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public DateRange AddYears(int v) {
			return new DateRange(AddYears(Start, v), AddYears(End, v));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="year"></param>
		/// <param name="numOfQuarter">1, 2, 3,4</param>
		/// <returns></returns>
		public static DateRange Quarter(int year, int numOfQuarter) {
			if (numOfQuarter > 4 || numOfQuarter < 1)
				throw new ArgumentOutOfRangeException("numOfQuarter must be within the range 1 - 4");
			int startMonth = 3*(numOfQuarter - 1) + 1;
			int endMonth = startMonth + 2;
			DateTime startDate = new DateTime(year, startMonth, 1);
			DateTime lastDate = new DateTime(year, endMonth, DateTime.DaysInMonth(year, endMonth));
			return new DateRange(startDate, lastDate);
		}

		/// <summary>
		/// Gets the quarter in which the day locates 
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		public static DateRange QuarterOf(int year, int month) {
			if (month > 12 || month < 1)
				throw new ArgumentOutOfRangeException("month must be within the range 1 - 12");
			return Quarter(year, (month - 1)/3 + 1);
		}

		/// <summary>
		/// Gets the quarter in which the <paramref name="dt"/> locates 
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateRange QuarterOf(DateTime dt) {
			return QuarterOf(dt.Year, dt.Month);
		}

		/// <summary>
		/// return the date range of the month the day belongs to
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		public static DateRange MonthOf(DateTime day) {
			int month = day.Month;
			int year = day.Year;
			DateRange retVal = new DateRange(new DateTime(year, month, 1),
			                                 new DateTime(year, month, DateTime.DaysInMonth(year, month)));
			return retVal;
		}

		/// <summary>
		/// return the date range reprensents the month
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
		public static DateRange Month(int year, int month) {
			return MonthOf(new DateTime(year, month, 10));
		}

		/// <summary>
		/// return the date range reprensents the whole year
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		public static DateRange Year(int year) {
			return new DateRange(
				new DateTime(year, 1, 1),
				new DateTime(year, 12, 31)
				);
		}

		private static DateTime? AddYears(DateTime? d, int v) {
			if (d == null) return null;
			return d.Value.AddYears(v);
		}
	}

	///<summary>
	/// A NHibernate user type for this class
	///</summary>
	public class DateRangeUserType : DateTimeRangeUserTypeGenericBase<DateRange> {
		protected override DateRange Create(DateTime? start, DateTime? end) {
			return new DateRange(start, end);
		}
	}
}
using System;

namespace MindHarbor.GenClassLib.MiscUtil {
	public class DateTimeUtil {
		public static TimeSpan FDaysToTimeSpan(Single days) {
			int d = Convert.ToInt32(days - 0.4999);
			int hr = Convert.ToInt32((days - d)*24);
			return new TimeSpan(d, hr, 0, 0);
		}

		/// <summary>
		/// Calculates the float number of days of a TimeSpan
		/// </summary>
		/// <param name="ts"></param>
		/// <returns></returns>
		public static Single TimeSpanToFDays(TimeSpan ts) {
			Single d = Convert.ToSingle(ts.Days);
			Single hr = (Convert.ToSingle(ts.Hours*100/24))/100;
			return d + hr;
		}

		public static TimeSpan FHoursToTimeSpan(Single hours) {
			int d = (int) (hours/24);
			int hr = (int) (hours - d*24);
			int min = (int) ((hours - ((int) hours))*60);
			return new TimeSpan(d, hr, min, 0);
		}

		public static Single TimeSpanToFHours(TimeSpan ts) {
			Single d = (Single) Convert.ToSingle(ts.Days)*24.0f;
			Single hr = (Convert.ToSingle(ts.Hours));
			Single min = ts.Minutes/60.0f;
			return d + hr + min;
		}

		public static DateTime IfNullSetToMax(object v) {
			if (v is DateTime) return (DateTime) v;
			return DateTime.MaxValue;
		}

		/// <summary>
		/// automatically return the date
		/// </summary>
		/// <param name="dt"></param>
		/// <returns>return "" when the datetime is equal to the max value</returns>
		public static string GetDate(DateTime dt) {
			if (dt.Year.Equals(DateTime.MaxValue.Year))
				return "";
			else return dt.ToShortDateString();
		}
	}
}
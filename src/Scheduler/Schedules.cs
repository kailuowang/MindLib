using System;
using System.Diagnostics;

namespace MindHarbor.Scheduler {
	/// <summary>
	/// Static factory for <see cref="Schedule"/>
	/// </summary>
	public abstract class Schedules {
		/// <summary>
		/// Get the caller's type name
		/// </summary>
		/// <returns></returns>
		private static string CallerName() {
			Type type = typeof (Schedule);
			foreach (StackFrame s in new StackTrace().GetFrames()) {
				type = s.GetMethod().DeclaringType;
				if (type.Assembly != typeof (Schedule).Assembly)
					break;
			}
			return type.FullName;
		}

		public static OneTimeSchedule OneTimeSchedule(string name, DateTime startTime) {
			return new OneTimeSchedule(name, startTime);
		}

		public static OneTimeSchedule OneTimeSchedule() {
			return OneTimeSchedule(CallerName() + DateTime.Now.Ticks, DateTime.Now);
		}

		public static IntervalSchedule IntervalSchedule(int interval) {
			return IntervalSchedule(CallerName(), interval, DateTime.Now, TimeSpan.MaxValue);
		}

		public static IntervalSchedule IntervalSchedule(string name, int intSecs, DateTime start, TimeSpan toTime) {
			TimeSpan fromTime = TimeSpan.MinValue;
			return new IntervalSchedule(name,
			                            start,
			                            intSecs,
			                            fromTime,
			                            toTime);
		}

		public static DailySchedule DailySchedule(string name, DateTime startTime) {
			return new DailySchedule(name, startTime);
		}

		public static MonthlySchedule MonthlySchedule(string name, DateTime startTime) {
			return new MonthlySchedule(name, startTime);
		}

		public static DailySchedule DailySchedule() {
			return DailySchedule(CallerName(), DateTime.Now);
		}
	}
}
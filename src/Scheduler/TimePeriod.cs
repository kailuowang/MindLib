using System;

namespace MindHarbor.Scheduler {
	/// <summary>
	/// Summary description for TimePeriod.
	/// </summary>
	public struct TimePeriod {
		private static TimePeriod nullObj = new TimePeriod(DateTime.MaxValue, DateTime.MaxValue);
		private DateTime end;
		private DateTime start;

		public TimePeriod(DateTime start, DateTime end) {
			if (start > end) throw new Exception("start cannot be later than end");
			this.start = start;

			this.end = end;
		}

		public DateTime Start {
			get { return start; }
		}

		public DateTime End {
			get { return end; }
		}

		public bool IsNull {
			get { return (start.Year == DateTime.MaxValue.Year); }
		}

		public static TimePeriod NullObj {
			get { return nullObj; }
		}
	}
}
using System;

namespace MindHarbor.Scheduler {
	/// <summary>
	/// OneTimeSchedule is used to schedule an event to run only once
	/// </summary>
	/// <remarks>
	/// Used by specific tasks to check self status
	/// </remarks>
	public class OneTimeSchedule : Schedule {
		public OneTimeSchedule(string name, DateTime startTime)
			: base(name, startTime, ScheduleType.ONETIME) {}

		internal override void CalculateNextInvokeTime() {
			// it does not matter, since this is a one time schedule
			m_nextTime = DateTime.MaxValue;
		}
	}

	/// <summary>
	/// IntervalSchedule is used to schedule an event to be invoked at regular intervals
	/// </summary>
	/// <remarks>
	/// the interval is specified in seconds. Useful mainly in checking status of threads
	/// and connections. Use an interval of 60 hours for an hourly schedule
	/// </remarks>
	public class IntervalSchedule : ScheduleWithWeekDaySetting {
		public IntervalSchedule(string name, DateTime startTime, int secs,
		                        TimeSpan fromTime, TimeSpan toTime) // time range for the day
			: base(name, startTime, ScheduleType.INTERVAL) {
			m_fromTime = fromTime;
			m_toTime = toTime;
			Interval = secs;
		}

		internal override void CalculateNextInvokeTime() {
			// add the interval of m_seconds
			m_nextTime = m_nextTime.AddSeconds(Interval);

			// if next invoke time is not within the time range, then set it to next start time
			if (! IsInvokeTimeInTimeRange())
				if (m_nextTime.TimeOfDay < m_fromTime)
					m_nextTime.AddSeconds(m_fromTime.Seconds - m_nextTime.TimeOfDay.Seconds);
				else
					m_nextTime.AddSeconds((24*3600) - m_nextTime.TimeOfDay.Seconds + m_fromTime.Seconds);

			// check to see if the next invoke time is on a working day
			while (! CanInvokeOnNextWeekDay())
				m_nextTime = m_nextTime.AddDays(1); // start checking on the next day
		}
	}

	// Daily schedule is used set off to the event every day
	// Mainly useful in maintanance, recovery, logging and report generation
	// Restictions can be imposed on the week days on which to run the schedule
	public class DailySchedule : ScheduleWithWeekDaySetting {
		public DailySchedule(string name, DateTime startTime)
			: base(name, startTime, ScheduleType.DAILY) {}

		internal override void CalculateNextInvokeTime() {
			// add a day, and check for any weekday restrictions and keep adding a day
			m_nextTime = m_nextTime.AddDays(1);
			while (! CanInvokeOnNextWeekDay())
				m_nextTime = m_nextTime.AddDays(1);
		}
	}

	/// <summary>
	/// Weekly schedules, useful generally in lazy maintanance jobs and
	/// restarting services and others major jobs
	/// </summary>
	public class WeeklySchedule : Schedule {
		public WeeklySchedule(string name, DateTime startTime)
			: base(name, startTime, ScheduleType.WEEKLY) {}

		// add a week (or 7 days) to the date
		internal override void CalculateNextInvokeTime() {
			m_nextTime = m_nextTime.AddDays(7);
		}
	}

	///<summary> 
	/// Monthly schedule - used to kick off an event every month on the same day as scheduled
	/// and also at the same hour and minute as given in start time
	///</summary>
	public class MonthlySchedule : Schedule {
		public MonthlySchedule(string name, DateTime startTime)
			: base(name, startTime, ScheduleType.MONTHLY) {}

		// add a month to the present time
		internal override void CalculateNextInvokeTime() {
			m_nextTime = m_nextTime.AddMonths(1);
		}
	}

	public abstract class ScheduleWithWeekDaySetting : Schedule {
		/// <summary>
		/// Array containing the 7 weekdays and their status
		/// Using DayOfWeek enumeration for index of this array
		/// </summary>
		private bool[] m_workingWeekDays = new bool[] {true, true, true, true, true, true, true};

		public ScheduleWithWeekDaySetting(string name, DateTime startTime, ScheduleType type)
			: base(name, startTime, type) {}

		/// <summary>
		/// check if no week days are active
		/// </summary>
		/// <returns></returns>
		public bool NoFreeWeekDay() {
			bool check = false;
			for (int index = 0; index < 7; check = check | m_workingWeekDays[index], index++) ;
			return check;
		}

		/// <summary>
		/// Setting the status of a week day
		/// </summary>
		/// <param name="day"></param>
		/// <param name="On"></param>
		public void SetWeekDay(DayOfWeek day, bool On) {
			m_workingWeekDays[(int) day] = On;
		}

		/// <summary>
		/// Return if the week day is set active
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		public bool WeekDayActive(DayOfWeek day) {
			return m_workingWeekDays[(int) day];
		}

		/// <summary>
		/// check to see if the Schedule can be invoked on the week day it is next scheduled 
		/// </summary>
		/// <returns></returns>
		protected bool CanInvokeOnNextWeekDay() {
			return m_workingWeekDays[(int) m_nextTime.DayOfWeek];
		}
	}
}
namespace MindHarbor.GenClassLib.Data {
	public enum MonthOfYear {
		January = 1,
		February = 2,
		March = 3,
		April = 4,
		May = 5,
		June = 6,
		July = 7,
		August = 8,
		September = 9,
		October = 10,
		November = 11,
		December = 12
	}

	public class MonthUtil {
		public static string GetAbbr(MonthOfYear month) {
			return month.ToString().Substring(0, 3);
		}
	}
}
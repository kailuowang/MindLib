using System;

namespace MindHarbor.GenClassLib.MiscUtil {
	/// <summary>
	/// Summary description for MathUtil.
	/// </summary>
	public class MathUtil {
		public MathUtil() {
			//
			// TODO: Add constructor logic here
			//
		}

		public static bool IsOdd(int n) {
			return Convert.ToBoolean(n & 1);
		}

		/// <summary>
		/// if n > 0 return n.ToString(), if n == 0 return "No"
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static string IntToNatrualLang(int n) {
			return n > 0 ? n.ToString() : "No";
		}
	}
}
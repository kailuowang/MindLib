using System;

namespace MindHarbor.ClassEnum {
	/// <summary>
	/// ClassEnum related exception
	/// </summary>
	public class ClassEnumException : Exception {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public ClassEnumException(string msg) : base(msg) {}

		/// <summary>
		/// 
		/// </summary>
		public ClassEnumException() : base() {}
	}
}
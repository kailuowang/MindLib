using System;
using System.Collections.Generic;
using System.Reflection;

namespace MindHarbor.GenClassLib.MiscUtil {
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericComparer<T> : IComparer<T> {
		private readonly bool asc = true;
		private readonly string propertyName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression">order expression: PROPERTY_NAME ASC|DESC </param>
		public GenericComparer(string expression) {
			if (string.IsNullOrEmpty(expression))
				throw new ArgumentNullException();
			string[] vals = expression.Split(new char[] {' '});
			propertyName = vals[0];
			asc = vals.Length == 1 || vals[1].ToUpper() == "ASC";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression">order expression: PROPERTY_NAME ASC|DESC </param>
		public GenericComparer(string expression, Type type) : this(expression) {
			ensurePropertyInfo(type);
		}

		#region IComparer<T> Members

		///<summary>
		///Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		///</summary>
		///
		///<returns>
		///Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
		///</returns>
		///
		///<param name="y">The second object to compare.</param>
		///<param name="x">The first object to compare.</param>
		public int Compare(T x, T y) {
			if (x == null || y == null)
				throw new ArgumentNullException();
			ensurePropertyInfo(x.GetType());
			IComparable xp = GetPropertyValue(x);
			IComparable yp = GetPropertyValue(y);
			if (xp == null && yp == null)
				return 0;
			int retVal;
			if (xp == null)
				retVal = 1;
			else
				retVal = xp.CompareTo(yp);
			if (!asc)
				retVal = -retVal;
			return retVal;
		}

		#endregion

		private void ensurePropertyInfo(Type type) {}

		private IComparable GetPropertyValue(T t) {
			PropertyInfo pi;
			pi = t.GetType().GetProperty(propertyName);
			if (pi == null)
				throw new Exception("Property  \"" + propertyName + "\" is not found in type " + t.GetType().Name);
			return pi.GetValue(t, null) as IComparable;
		}
	}
}
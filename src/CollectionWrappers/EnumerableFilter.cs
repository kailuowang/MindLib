using System;
using System.Collections;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// A Simple Wrapper for wrapping an regular Enumerable as a generic Enumberable&lt;T&gt
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <exception cref="InvalidCastException">
	/// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
	/// </exception>
	public class EnumerableFilter<T> : IEnumerable<T> {
		private bool _useFileter = false;
		private IEnumerable innerEnumerable;

		public EnumerableFilter(IEnumerable innerEnumerable) {
			this.innerEnumerable = innerEnumerable;
		}

		public EnumerableFilter(IEnumerable innerEnumerable, bool useFilter) : this(innerEnumerable) {
			_useFileter = useFilter;
		}

		public bool UserFilter {
			get { return _useFileter; }
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator() {
			return new EnumeratorFilter<T>(innerEnumerable.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new EnumeratorFilter<T>(innerEnumerable.GetEnumerator());
		}

		#endregion

		public override bool Equals(object obj) {
			if (!obj.GetType().Equals(GetType())) return false;
			if (obj == this) return true;
			EnumerableFilter<T> that = (EnumerableFilter<T>) obj;
			return innerEnumerable.Equals(that.innerEnumerable);
		}

		public override int GetHashCode() {
			int result = 22;
			result = 37*result + GetType().GetHashCode();
			result = 37*result + innerEnumerable.GetHashCode();
			return result;
		}
	}
}
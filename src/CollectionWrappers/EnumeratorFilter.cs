using System;
using System.Collections;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// Simple Wrapper for wrapping an regular Enumerator as a generic Enumberator&lt;T&gt
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <exception cref="InvalidCastException">
	/// If the wrapped has any item that is not of Type T, the item(s) will be hidden from the enumerator and count
	/// </exception>
	public struct EnumeratorFilter<T> : IEnumerator<T>, IEnumerator {
		private IEnumerator innerEnumerator;

		public EnumeratorFilter(IEnumerator toWrap) {
			innerEnumerator = toWrap;
		}

		#region IEnumerator<T> Members

		public T Current {
			get { return (T) innerEnumerator.Current; }
		}

		public void Dispose() {
			innerEnumerator = null;
		}

		object IEnumerator.Current {
			get { return innerEnumerator.Current; }
		}

		public bool MoveNext() {
			while (innerEnumerator.MoveNext())
				if (innerEnumerator.Current is T)
					return true;
			return false;
		}

		public void Reset() {
			innerEnumerator.Reset();
		}

		#endregion

		public override bool Equals(object obj) {
			if (!obj.GetType().Equals(GetType())) return false;
			return innerEnumerator.Equals(((EnumeratorFilter<T>) obj).innerEnumerator);
		}

		public override int GetHashCode() {
			return innerEnumerator.GetHashCode();
		}
	}
}
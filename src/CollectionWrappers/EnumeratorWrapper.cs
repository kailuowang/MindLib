using System;
using System.Collections;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// Simple Wrapper for wrapping an regular Enumerator as a generic Enumberator&lt;T&gt
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <exception cref="InvalidCastException">
	/// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
	/// </exception>
	public struct EnumeratorWrapper<T> : IEnumerator<T> {
		private IEnumerator innerEnumerator;

		public EnumeratorWrapper(IEnumerator toWrap) {
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
			return innerEnumerator.MoveNext();
		}

		public void Reset() {
			innerEnumerator.Reset();
		}

		#endregion

		public override bool Equals(object obj) {
			if (!obj.GetType().Equals(GetType())) return false;
			return innerEnumerator.Equals(((EnumeratorWrapper<T>) obj).innerEnumerator);
		}

		public override int GetHashCode() {
			int result = 22;
			result = 37*result + GetType().GetHashCode();
			result = 37*result + innerEnumerator.GetHashCode();
			return result;
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// A wrapper that can wrap a ISet as a generic ISet&lt;T&gt; 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks>
	/// In most operations, there is no copying of collections. The wrapper just delegate the function to the wrapped.
	/// The following functions' implementation may involve collection copying:
	/// Union, Intersect, Minus, ExclusiveOr, ContainsAll, AddAll, RemoveAll, RetainAll
	/// </remarks>
	/// <exception cref="InvalidCastException">
	/// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
	/// </exception>
	public sealed class SetWrapper<T> : ISet<T> {
		private ISet innerSet;

		private SetWrapper() {}

		public SetWrapper(ISet toWrap) {
			if (toWrap == null)
				throw new ArgumentNullException();
			innerSet = toWrap;
		}

		#region Operators

		public ISet<T> Union(ISet<T> a) {
			return getSetCopy().Union(a);
		}

		public ISet<T> Intersect(ISet<T> a) {
			return getSetCopy().Intersect(a);
		}

		public ISet<T> Minus(ISet<T> a) {
			return getSetCopy().Minus(a);
		}

		public ISet<T> ExclusiveOr(ISet<T> a) {
			return getSetCopy().ExclusiveOr(a);
		}

		#endregion

		#region ISet<T> Members

		public bool Contains(T o) {
			return innerSet.Contains(o);
		}

		public bool ContainsAll(ICollection<T> c) {
			return innerSet.ContainsAll(getSetCopy(c));
		}

		public bool IsEmpty {
			get { return innerSet.IsEmpty; }
		}

		public bool Add(T o) {
			return innerSet.Add(o);
		}

		public bool AddAll(ICollection<T> c) {
			return innerSet.AddAll(getSetCopy(c));
		}

		public bool Remove(T o) {
			return innerSet.Remove(o);
		}

		public bool RemoveAll(ICollection<T> c) {
			return innerSet.RemoveAll(getSetCopy(c));
		}

		public bool RetainAll(ICollection<T> c) {
			return innerSet.RemoveAll(getSetCopy(c));
		}

		public void Clear() {
			innerSet.Clear();
		}

		public int Count {
			get { return innerSet.Count; }
		}

		void ICollection<T>.Add(T item) {
			Add(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			innerSet.CopyTo(array, arrayIndex);
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public IEnumerator<T> GetEnumerator() {
			return new EnumeratorWrapper<T>(innerSet.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return innerSet.GetEnumerator();
		}

		object ICloneable.Clone() {
			return Clone();
		}

		#region private methods

		private Set<T> getSetCopy(ICollection<T> c) {
			return new HashedSet<T>(c);
		}

		private Set<T> getSetCopy(ICollection c) {
			Set<T> retVal = new HashedSet<T>();
			((ISet) retVal).AddAll(c);
			return retVal;
		}

		private Set<T> getSetCopy() {
			return getSetCopy(innerSet);
		}

		#endregion

		#endregion

		public ISet<T> Clone() {
			return new SetWrapper<T>((ISet) innerSet.Clone());
		}
	}
}
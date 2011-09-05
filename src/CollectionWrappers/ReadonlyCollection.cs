using System;
using System.Collections;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// Provides the wrapper class for a generic read-only collection. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks>
	/// An instance of the ReadOnlyCollection generic class is always read-only. 
	/// A collection that is read-only is simply a collection with a wrapper that prevents modifying the collection;
	///  therefore, if changes are made to the underlying collection, the read-only collection reflects those changes. 
	/// </remarks>
	public class ReadonlyCollection<T> : ICollection<T> {
		private ICollection<T> innerCollection;

		public ReadonlyCollection(ICollection<T> toWrap) {
			innerCollection = toWrap;
		}

		#region ICollection<T> Members

		/// <summary>
		/// always throw a NotSupportedException
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item) {
			ThrowReadOnlyException();
		}

		/// <summary>
		/// always throw a NotSupportedException
		/// </summary>
		/// <param name="item"></param>
		public void Clear() {
			ThrowReadOnlyException();
		}

		public bool Contains(T item) {
			return innerCollection.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			innerCollection.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return innerCollection.Count; }
		}

		/// <summary>
		/// always true
		/// </summary>
		public bool IsReadOnly {
			get { return true; }
		}

		/// <summary>
		/// always throw a NotSupportedException
		/// </summary>
		/// <param name="item"></param>
		public bool Remove(T item) {
			return ThrowReadOnlyException();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return innerCollection.GetEnumerator();
		}

		public IEnumerator GetEnumerator() {
			return innerCollection.GetEnumerator();
		}

		#endregion

		private bool ThrowReadOnlyException() {
			throw new NotSupportedException("The ICollection is read-only.");
		}
	}
}
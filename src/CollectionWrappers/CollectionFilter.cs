using System;
using System.Collections;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	public class CollectionFilter<T> : EnumerableFilter<T>, ICollection<T> {
		private ICollection innerCollection;

		public CollectionFilter(ICollection toWrap) : base(toWrap) {
			innerCollection = toWrap;
		}

		#region ICollection<T> Members

		public void Add(T item) {
			ThrowReadOnlyException();
		}

		public void Clear() {
			ThrowReadOnlyException();
		}

		public bool Contains(T item) {
			foreach (object o in innerCollection)
				if (item.Equals(o)) return true;
			return false;
		}

		public void CopyTo(T[] array, int arrayIndex) {
			IList temp = new ArrayList();
			foreach (object o in innerCollection)
				if (o is T)
					temp.Add(o);
			temp.CopyTo(array, arrayIndex);
		}

		public int Count {
			get {
				int count = 0;
				foreach (object o in innerCollection)
					if (o is T)
						count++;
				return count;
			}
		}

		public bool IsReadOnly {
			get { return true; //always return true since the old ICollection does not support mutation 
			}
		}

		public bool Remove(T item) {
			return ThrowReadOnlyException();
		}

		#endregion

		private bool ThrowReadOnlyException() {
			throw new NotSupportedException("The ICollection is read-only.");
		}
	}
}
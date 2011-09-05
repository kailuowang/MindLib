using System;
using System.Collections;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	public class NonGCollectionWrapper<WrappeeT> : ICollection {
		private ICollection<WrappeeT> inner;

		public NonGCollectionWrapper(ICollection<WrappeeT> toWrap) {
			inner = toWrap;
		}

		#region ICollection Members

		public void CopyTo(Array array, int index) {
			WrappeeT[] tocopy = new WrappeeT[array.Length];
			inner.CopyTo(tocopy, index);
			for (int i = 0; i < array.Length; i++)
				array.SetValue(tocopy[i], i);
		}

		public int Count {
			get { return inner.Count; }
		}

		public object SyncRoot {
			get { return null; }
		}

		public bool IsSynchronized {
			get { return false; }
		}

		public IEnumerator GetEnumerator() {
			return inner.GetEnumerator();
		}

		#endregion
	}
}
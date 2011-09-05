using System;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	public class CollectionTypeWrapper<TargetT, ToCastT> : EnumerableTypeCastWrapper<TargetT, ToCastT>,
	                                                       ICollection<TargetT> {
		private ICollection<ToCastT> innerCollection;

		public CollectionTypeWrapper(ICollection<ToCastT> toWrap)
			: base(toWrap) {
			innerCollection = toWrap;
		}

		#region ICollection<TargetT> Members

		public void Add(TargetT item) {
			ThrowReadOnlyException();
		}

		public void Clear() {
			ThrowReadOnlyException();
		}

		public bool Contains(TargetT item) {
			foreach (object o in innerCollection)
				if (item.Equals(o)) return true;
			return false;
		}

		public void CopyTo(TargetT[] array, int arrayIndex) {
			ToCastT[] tmp = new ToCastT[array.Length];
			innerCollection.CopyTo(tmp, arrayIndex);
			Array.Copy(tmp, array, tmp.Length);
		}

		public int Count {
			get { return innerCollection.Count; }
		}

		public bool IsReadOnly {
			get { return true; //always return true since the old ICollection does not support mutation 
			}
		}

		public bool Remove(TargetT item) {
			return ThrowReadOnlyException();
		}

		#endregion

		protected bool ThrowReadOnlyException() {
			throw new NotSupportedException("The ICollection is read-only.");
		}
	                                                       }
}
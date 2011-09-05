using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	public class ListTypeCastWrapper<TargetT, ToCastT> : CollectionTypeWrapper<TargetT, ToCastT>, IList<TargetT>
		where ToCastT : TargetT {
		private IList<ToCastT> innerList;

		public ListTypeCastWrapper(IList<ToCastT> toWrap) : base(toWrap) {
			innerList = toWrap;
		}

		#region IList<TargetT> Members

		///<summary>
		///Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
		///</summary>
		///
		///<returns>
		///The index of item if found in the list; otherwise, -1.
		///</returns>
		///
		///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		public int IndexOf(TargetT item) {
			return innerList.IndexOf((ToCastT) item);
		}

		///<summary>
		///Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
		///</summary>
		///
		///<param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		///<param name="index">The zero-based index at which item should be inserted.</param>
		///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void Insert(int index, TargetT item) {
			ThrowReadOnlyException();
		}

		///<summary>
		///Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
		///</summary>
		///
		///<param name="index">The zero-based index of the item to remove.</param>
		///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void RemoveAt(int index) {
			ThrowReadOnlyException();
		}

		///<summary>
		///Gets or sets the element at the specified index.
		///</summary>
		///
		///<returns>
		///The element at the specified index.
		///</returns>
		///
		///<param name="index">The zero-based index of the element to get or set.</param>
		///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		public TargetT this[int index] {
			get { return innerList[index]; }
			set { ThrowReadOnlyException(); }
		}

		#endregion
		}
}
using System.Collections;
using System.Collections.Generic;
using MindHarbor.CollectionWrappers.Util;

namespace MindHarbor.CollectionWrappers {
	public class SearchableChildrenCollection<PropT, CollectionItemT> : ISearchableCollection<PropT, CollectionItemT>
		where CollectionItemT : class {
		private readonly PropFieldReflection pfi;
		private readonly string searchPropertyName;
		private SearchableCollectionDecorator<PropT, CollectionItemT> inner;

		public SearchableChildrenCollection(string searchPropertyName, object parent, string childrenPropertyName) {
			this.searchPropertyName = searchPropertyName;
			pfi = new PropFieldReflection(childrenPropertyName, parent);
		}

		private SearchableCollectionDecorator<PropT, CollectionItemT> Inner {
			get {
				if (inner == null || !inner.IsDecorating(GetChildrenCollection()))
					inner =
						new SearchableCollectionDecorator<PropT, CollectionItemT>(GetChildrenCollection(),
						                                                          searchPropertyName);
				return inner;
			}
		}

		#region ISearchableCollection<PropT,CollectionItemT> Members

		public int Count {
			get { return Inner.Count; }
		}

		public bool IsReadOnly {
			get { return Inner.IsReadOnly; }
		}

		public void Add(CollectionItemT item) {
			Inner.Add(item);
		}

		public void Clear() {
			Inner.Clear();
		}

		public bool Contains(CollectionItemT item) {
			return Inner.Contains(item);
		}

		public void CopyTo(CollectionItemT[] array, int arrayIndex) {
			Inner.CopyTo(array, arrayIndex);
		}

		public bool Remove(CollectionItemT item) {
			return Inner.Remove(item);
		}

		public IEnumerator<CollectionItemT> GetEnumerator() {
			return Inner.GetEnumerator();
		}

		public CollectionItemT Search(PropT key, bool ensure) {
			return Inner.Search(key, ensure);
		}

		public CollectionItemT Search(PropT key) {
			return Inner.Search(key);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Inner.GetEnumerator();
		}

		#endregion

		private ICollection<CollectionItemT> GetChildrenCollection() {
			return (ICollection<CollectionItemT>) pfi.Val;
		}

		public bool IsDecorating(ICollection<CollectionItemT> c) {
			return Inner.IsDecorating(c);
		}
		}
}
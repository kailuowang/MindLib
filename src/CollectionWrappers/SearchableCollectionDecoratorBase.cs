using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MindHarbor.CollectionWrappers {
	public abstract class SearchableCollectionDecoratorBase<PropT, CollectionItemT> :
		ISearchableCollection<PropT, CollectionItemT> where CollectionItemT : class {
		private readonly PropertyInfo pi;

		private Dictionary<PropT, CollectionItemT> index;

		public SearchableCollectionDecoratorBase(string searchPropertyName) {
			pi = typeof (CollectionItemT).GetProperty(searchPropertyName);
			if (pi == null || pi.PropertyType != typeof (PropT))
				throw new ArgumentException(typeof (CollectionItemT).Name
				                            + " does not have a " + typeof (PropT).Name
				                            + " property named " + searchPropertyName);
		}

		protected abstract ICollection<CollectionItemT> InnerCollection { get; }

		protected string SearchPropertyName {
			get { return pi.Name; }
		}

		#region ISearchableCollection<PropT,CollectionItemT> Members

		public void Add(CollectionItemT item) {
			InnerCollection.Add(item);
			if (index != null)
				index.Add(PropVal(item), item);
		}

		public void Clear() {
			InnerCollection.Clear();
			if (index != null)
				index.Clear();
		}

		public bool Contains(CollectionItemT item) {
			return InnerCollection.Contains(item);
		}

		public void CopyTo(CollectionItemT[] array, int arrayIndex) {
			InnerCollection.CopyTo(array, arrayIndex);
		}

		public bool Remove(CollectionItemT item) {
			bool retVal = InnerCollection.Remove(item);
			if (retVal && index != null)
				index.Remove(PropVal(item));
			return retVal;
		}

		public int Count {
			get { return InnerCollection.Count; }
		}

		public bool IsReadOnly {
			get { return InnerCollection.IsReadOnly; }
		}

		public IEnumerator<CollectionItemT> GetEnumerator() {
			return InnerCollection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable) InnerCollection).GetEnumerator();
		}

		/// <summary>
		/// Search for the <typeparamref name="CollectionItemT"/> for an item has the property equals key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="ensure">switch to turn on and off the ensure mode (whether to perform a linear search if not found in the index
		/// </param>
		/// <returns></returns>
		public CollectionItemT Search(PropT key, bool ensure) {
			CollectionItemT retVal;
			if (index == null ||
			    (!index.TryGetValue(key, out retVal) && ensure))
				retVal = BuildIndex(key);
			return retVal;
		}

		/// <summary>
		/// Search for the <typeparamref name="CollectionItemT"/> for an item has the property equals key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <remarks>
		/// it will first use the index to search, if not found use the linear search
		/// </remarks>
		public CollectionItemT Search(PropT key) {
			return Search(key, true);
		}

		#endregion

		public bool IsDecorating(ICollection<CollectionItemT> c) {
			return InnerCollection == c;
		}

		private CollectionItemT BuildIndex(PropT key) {
			index = new Dictionary<PropT, CollectionItemT>();
			CollectionItemT retVal = null;
			foreach (CollectionItemT child in InnerCollection) {
				PropT pv = PropVal(child);
				if (pv == null)
					throw new Exception("the item" + child.ToString() + " has a null " + pi.Name);
				if (index.ContainsKey(pv))
					throw new Exception("There are more than one items in the collection that has the same " + pi.Name +
					                    " value");
				index[pv] = child;
				if (pv.Equals(key))
					retVal = child;
			}
			return retVal;
		}

		private PropT PropVal(CollectionItemT child) {
			return (PropT) pi.GetValue(child, null);
		}
		}
}
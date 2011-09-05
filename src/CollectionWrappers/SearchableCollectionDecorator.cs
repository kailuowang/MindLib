using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// a collection wrapper that wrappes a ICollection but enable search by property
	/// </summary>
	/// <remarks>
	/// it utilizes an index dictionary (which is built when first serach)
	/// </remarks>
	public class SearchableCollectionDecorator<PropT, CollectionItemT> :
		SearchableCollectionDecoratorBase<PropT, CollectionItemT> where CollectionItemT : class {
		protected readonly ICollection<CollectionItemT> innerCollection;

		public SearchableCollectionDecorator(ICollection<CollectionItemT> innerCollection, string searchPropertyName)
			: base(searchPropertyName) {
			this.innerCollection = innerCollection;
		}

		protected override ICollection<CollectionItemT> InnerCollection {
			get { return innerCollection; }
		}
		}

	public class SearchableSetDecorator<PropT, CollectionItemT> : SearchableCollectionDecoratorBase<PropT, CollectionItemT>,
	                                                              ISet<CollectionItemT> where CollectionItemT : class {
		protected readonly ISet<CollectionItemT> innerCollection;

		public SearchableSetDecorator(ISet<CollectionItemT> innerCollection, string searchPropertyName)
			: base(searchPropertyName) {
			this.innerCollection = innerCollection;
		}

		protected override ICollection<CollectionItemT> InnerCollection {
			get { return innerCollection; }
		}

		#region ISet<CollectionItemT> Members

		public ISet<CollectionItemT> Union(ISet<CollectionItemT> a) {
			return innerCollection.Union(a);
		}

		public ISet<CollectionItemT> Intersect(ISet<CollectionItemT> a) {
			return innerCollection.Intersect(a);
		}

		public ISet<CollectionItemT> Minus(ISet<CollectionItemT> a) {
			return innerCollection.Minus(a);
		}

		public ISet<CollectionItemT> ExclusiveOr(ISet<CollectionItemT> a) {
			return innerCollection.ExclusiveOr(a);
		}

		public bool ContainsAll(ICollection<CollectionItemT> c) {
			return innerCollection.ContainsAll(c);
		}

		bool ISet<CollectionItemT>.Add(CollectionItemT o) {
			return innerCollection.Add(o);
		}

		public bool AddAll(ICollection<CollectionItemT> c) {
			return innerCollection.AddAll(c);
		}

		public bool RemoveAll(ICollection<CollectionItemT> c) {
			return innerCollection.RemoveAll(c);
		}

		public bool RetainAll(ICollection<CollectionItemT> c) {
			return innerCollection.RetainAll(c);
		}

		public bool IsEmpty {
			get { return innerCollection.IsEmpty; }
		}

		public object Clone() {
			return
				new SearchableSetDecorator<PropT, CollectionItemT>((ISet<CollectionItemT>) innerCollection.Clone(),
				                                                   SearchPropertyName);
		}

		#endregion
	                                                              }
}
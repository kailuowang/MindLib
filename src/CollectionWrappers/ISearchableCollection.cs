using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	public interface ISearchableCollection<PropT, CollectionItemT> : ICollection<CollectionItemT> {
		/// <summary>
		/// Search for the <typeparamref name="CollectionItemT"/> for an item has the property equals key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="ensure">switch to turn on and off the ensure mode (whether to perform a linear search if not found in the index
		/// </param>
		/// <returns></returns>
		CollectionItemT Search(PropT key, bool ensure);

		/// <summary>
		/// Search for the <typeparamref name="CollectionItemT"/> for an item has the property equals key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <remarks>
		/// it will first use the index to search, if not found use the linear search
		/// </remarks>
		CollectionItemT Search(PropT key);
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.MiscNHibernateUserTypes
{
	/// <summary>
	/// a property dictionary to store heterogeneous value type
	/// </summary>
	/// <remarks>
	/// To use this in NHibernate
	/// 
	/// <example>
	/// Mapping File
	/// <![CDATA[
	/// <map name="Props" access="field.camelcase">
	///        <key />
	///        <index type="string"/>
	///        <composite-element class="MindHarbor.MiscNHibernateUserTypes.HeterogeneousProperty,MindHarbor.MiscNHibernateUserTypes">
	///            <property name="Name"  />
	///            <property name="StringValue" access="field.camelcase" />
	///            <property name="ValueType" />
	///        </composite-element>
	/// </map>
	/// ]]>
	/// 
	/// Code file:
	/// <code>
	/// private IDictionary<string, HeterogeneousProperty> props = new Dictionary<string, HeterogeneousProperty>();
	///	public HeterogeneousPropertyDict Props
	///	{
	///		get { return new HeterogeneousPropertyDict(props); }
	///	}
	/// </code>
	/// </example>
	/// 
	/// </remarks>
	public class HeterogeneousPropertyDict : IDictionary<string, object>, IEquatable<HeterogeneousPropertyDict> {
		private readonly IDictionary<string, HeterogeneousProperty> wrapped;

		///<summary>
		///</summary>
		///<param name="wrapped"></param>
		public HeterogeneousPropertyDict(IDictionary<string, HeterogeneousProperty> wrapped) {
			this.wrapped = wrapped;
		}

		///<summary>
		///</summary>
		public HeterogeneousPropertyDict() :this( new Dictionary<string, HeterogeneousProperty>()){}

	    ///<summary>
	    ///Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.
	    ///</returns>
	    ///
	    ///<param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
	    ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
	    public bool ContainsKey(string key) {
			return Wrapped.ContainsKey(key);
		}

	    ///<summary>
	    ///Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</summary>
	    ///
	    ///<param name="value">The object to use as the value of the element to add.</param>
	    ///<param name="key">The object to use as the key of the element to add.</param>
	    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
	    ///<exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</exception>
	    ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
	    public void Add(string key, object value) {
			Wrapped.Add(key, new HeterogeneousProperty(key, value));
		}

		///<summary>
		///</summary>
		///<param name="key"></param>
		///<returns></returns>
		public Type GetValueType(string key) {
			return Wrapped[key].ValueType;
		}

	    ///<summary>
	    ///Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</returns>
	    ///
	    ///<param name="key">The key of the element to remove.</param>
	    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
	    ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
	    public bool Remove(string key) {
			return Wrapped.Remove(key);
		}

		///<summary>
		///</summary>
		///<param name="key"></param>
		///<param name="value"></param>
		///<returns></returns>
		public bool TryGetValue(string key, out object value) {
			HeterogeneousProperty ph;
			bool retVal = Wrapped.TryGetValue(key, out ph);
		    value = retVal ? ph.Value : null;
			return retVal;
		}

	    ///<summary>
	    ///Gets or sets the element with the specified key.
	    ///</summary>
	    ///
	    ///<returns>
	    ///The element with the specified key. Returns null if not found
	    ///</returns>
	    ///
	    ///<param name="key">The key of the element to get or set.</param>
	    ///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
	    ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
	    /// 
	    public Object this[string key] {
			get {
			    HeterogeneousProperty hp;
			    if(Wrapped.TryGetValue(key, out hp))
			        return hp.Value;
			    return null;
			}
			set { Wrapped[key] = new HeterogeneousProperty(key,value); }
		}

	    ///<summary>
	    ///Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</summary>
	    ///
	    ///<returns>
	    ///An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</returns>
	    ///
	    public ICollection<string> Keys {
			get { return Wrapped.Keys; }
		}

	    ///<summary>
	    ///Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</summary>
	    ///
	    ///<returns>
	    ///An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
	    ///</returns>
	    ///
	    public ICollection<Object> Values {
			get { return new CollectionWrappers.PropertyCollectionWrapper<HeterogeneousProperty, object>(  Wrapped.Values, "Value"); }
		}

	    ///<summary>
	    ///Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
	    ///</summary>
	    ///
	    ///<param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
	    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
	    public void Add(KeyValuePair<string, object> item) {
			Add(item.Key, item.Value);
		}

	    ///<summary>
	    ///Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
	    ///</summary>
	    ///
	    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
	    public void Clear() {
			Wrapped.Clear();
		}

	    ///<summary>
	    ///Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
	    ///</returns>
	    ///
	    ///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
	    public bool Contains(KeyValuePair<string, object> item) {
			return Wrapped.Contains(GetItem(item));
		}

	    ///<summary>
	    ///Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
	    ///</summary>
	    ///
	    ///<param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
	    ///<param name="arrayIndex">The zero-based index in array at which copying begins.</param>
	    ///<exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
	    ///<exception cref="T:System.ArgumentNullException">array is null.</exception>
	    ///<exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
	    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
			KeyValuePair<string, HeterogeneousProperty>[] ar = new KeyValuePair<string, HeterogeneousProperty>[array.Length];
			Wrapped.CopyTo(ar,arrayIndex);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new KeyValuePair<string, object>(ar[i].Key, ar[i].Value.Value);
			}
		}

	    ///<summary>
	    ///Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
	    ///</returns>
	    ///
	    ///<param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
	    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
	    public bool Remove(KeyValuePair<string, object> item) {
			return Wrapped.Remove(GetItem(item));
		}

	    ///<summary>
	    ///Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
	    ///</summary>
	    ///
	    ///<returns>
	    ///The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
	    ///</returns>
	    ///
	    public int Count {
			get { return Wrapped.Count; }
		}

	    ///<summary>
	    ///Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
	    ///</returns>
	    ///
	    public bool IsReadOnly {
			get { return Wrapped.IsReadOnly; }
		}

		internal IDictionary<string, HeterogeneousProperty> Wrapped {
			get { return wrapped; }
		}

	    ///<summary>
	    ///Returns an enumerator that iterates through the collection.
	    ///</summary>
	    ///
	    ///<returns>
	    ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
	    ///</returns>
	    ///<filterpriority>1</filterpriority>
	    public IEnumerator<KeyValuePair<string, object >> GetEnumerator() {
			return new HeterogeneousPropertyDictEnumerator( Wrapped.GetEnumerator());
		}

		 IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		private static KeyValuePair<string, HeterogeneousProperty> GetItem(KeyValuePair<string, object> item) {
			return new KeyValuePair<string, HeterogeneousProperty>(item.Key, new HeterogeneousProperty(item.Key, item.Value));
		}

	    ///<summary>
	    ///Indicates whether the current object is equal to another object of the same type.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if the current object is equal to the other parameter; otherwise, false.
	    ///</returns>
	    ///
	    ///<param name="other">An object to compare with this object.</param>
	    public bool Equals(HeterogeneousPropertyDict other) {
			if (other == null) return false;
			return Equals(wrapped, other.wrapped);
		}

	    ///<summary>
	    ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
	    ///</returns>
	    ///
	    ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
	    public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as HeterogeneousPropertyDict);
		}

	    ///<summary>
	    ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
	    ///</summary>
	    ///
	    ///<returns>
	    ///A hash code for the current <see cref="T:System.Object"></see>.
	    ///</returns>
	    ///<filterpriority>2</filterpriority>
	    public override int GetHashCode() {
			return wrapped.GetHashCode();
		}
	}

	internal class HeterogeneousPropertyDictEnumerator : IEnumerator<KeyValuePair<string,object>>, IEnumerator {
		public HeterogeneousPropertyDictEnumerator(IEnumerator<KeyValuePair<string, HeterogeneousProperty>> wrapped) {
			this.wrapped = wrapped;
		}

		private IEnumerator<KeyValuePair<string, HeterogeneousProperty>> wrapped;

		public KeyValuePair<string, object > Current {
			get { return new KeyValuePair<string, object>(wrapped.Current.Key,wrapped.Current.Value.Value); }
		}

		public void Dispose() {
			wrapped.Dispose();
		}

		public bool MoveNext() {
			return wrapped.MoveNext();
		}

		public void Reset() {
			wrapped.Reset();
		}

		object IEnumerator.Current {
			get { return  Current; }
		}
	}
}

using System;
using System.Collections.Generic;

namespace MindHarbor.CollectionWrappers {
	/// <summary>
	/// A wrapper that expose a specified property of the item of the wrapped collection as the item of the wrapper collection
	/// </summary>
	/// <typeparam name="ContainerT"></typeparam>
	/// <typeparam name="PropertyT"></typeparam>
	/// <remarks>
	/// This Class is designed to wrap the crosslink entity collection in the many-to-many scenario. 
	/// Sometime a many-to-many association is devided to two one-to-many association with a cross link entity.
	/// For example the association between message and people is a many-to-many association. 
	/// <sample>
	/// <code>
	/// class Message{
	///    public ICollection&lt;People&gt; Receipents;
	/// }
	/// class People{
	///   public ICollection&lt;Message&gt; Messages;
	/// }
	/// </code>
	/// </sample>
	/// One of the good design might be dividing it into two one-to-many associations with a crosslink entity,
	/// this wrapper can appear to be a collection of the associated entity in the many.
	/// <sample>
	/// <code>
	/// class Message{
	///     public ICollection&lt;MessageReciepent&gt; MessageReciepent;
	///     public ICollection&lt;People&gt; Receipents{
	///        get{ return new PropertyCollectionWrapper&lt;MessageReciepent, People&gt;(MessageReciepent, "Receipent");}
	///     }
	/// }
	/// class People{
	///     public ICollection&lt;MessageReciepent&gt; MessageReciepent;
	///     public ICollection&lt;Message&gt; Messages{
	///        get{ return new PropertyCollectionWrapper&lt;MessageReciepent, Message&gt;(MessageReciepent, "Message");}
	///     }
	/// }
	/// class MessageReciepent{
	///     public Message; Message;
	///     public People; Receipent;
	/// }
	/// </code>
	/// </sample>
	/// </remarks>
	public class PropertyCollectionWrapper<ContainerT, PropertyT> : PropertyEnumerableWrapper<ContainerT, PropertyT>,
	                                                                ICollection<PropertyT> {
		protected ICollection<ContainerT> innerCollection;

		public PropertyCollectionWrapper(ICollection<ContainerT> toWrap,
		                                 string propertyName)
			: base(toWrap, propertyName) {
			innerCollection = toWrap;
		}

		#region ICollection<PropertyT> Members

		public void Add(PropertyT item) {
			ThrowReadOnlyException();
		}

		public void Clear() {
			ThrowReadOnlyException();
		}

		public bool Contains(PropertyT item) {
			foreach (PropertyT i in this)
				if (i.Equals(item))
					return true;
			return false;
		}

		public void CopyTo(PropertyT[] array, int arrayIndex) {
			if (array == null)
				throw new ArgumentNullException();
			if (arrayIndex < 0) throw new ArgumentOutOfRangeException();
			if ((array.Length - arrayIndex) < Count)
				throw new ArgumentException();

			int i = arrayIndex;
			foreach (PropertyT item in this) {
				array[i] = item;
				i++;
			}
		}

		public bool Remove(PropertyT item) {
			return ThrowReadOnlyException();
		}

		public int Count {
			get { return innerCollection.Count; }
		}

		public bool IsReadOnly {
			get { return true; }
		}

		#endregion

		protected bool ThrowReadOnlyException() {
			throw new NotSupportedException("The ICollection is read-only.");
		}
	                                                                }
}
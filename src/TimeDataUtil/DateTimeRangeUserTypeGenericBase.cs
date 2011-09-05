using System;
using System.Data;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace MindHarbor.TimeDataUtil {
	/// <summary>
	/// A base class for <see cref="DateTimeRange"/>  NHibernate UserTypes
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class DateTimeRangeUserTypeGenericBase<T> : ICompositeUserType where T : DateTimeRange {
		/// <summary>
		/// Meaningless datetime for storing null daterange into the database. <seealso cref="NullRangeStart"/>
		/// </summary>
		private readonly DateTime NullRangeEnd = new DateTime(1899, 1, 3, 6, 4, 21);

		/// <summary>
		/// Meaningless <see cref="DateTime"/> for storing null <see cref="DateTimeRange"/> into the database
		/// </summary>
		/// <remarks>
		/// The NullRangeStart is larger than the NullRangeEnd, so this DateTime pair will never appear in the real scenario.
		/// DO NOT CHANGE THESE VALUES EVER.
		/// </remarks>
		private readonly DateTime NullRangeStart = new DateTime(2099, 9, 8, 5, 3, 39);

		private string[] propertyNames = new string[] {"Start", "End"};
		private IType[] propertyTypes = new IType[] {NHibernateUtil.DateTime, NHibernateUtil.DateTime};

		#region ICompositeUserType Members

		///<summary>
		///   Get the value of a property
		///</summary>
		///<param name="component">an instance of class mapped by this "type"</param>
		///<param name="property"></param>
		///<returns>
		///the property value
		///</returns>
		public object GetPropertyValue(object component, int property) {
			T dr = (T) component;
			if (property == 0)
				return dr.Start;
			else
				return dr.End;
		}

		///<summary>
		///
		///            Set the value of a property
		///            
		///</summary>
		///
		///<param name="component">an instance of class mapped by this "type"</param>
		///<param name="property"></param>
		///<param name="value">the value to set</param>
		public void SetPropertyValue(object component, int property, object value) {
			throw new NotSupportedException("Immutable");
		}

		///<summary>
		///
		///            Compare two instances of the class mapped by this type for persistence
		///            "equality", ie. equality of persistent state.
		///            
		///</summary>
		///
		///<param name="x"></param>
		///<param name="y"></param>
		///<returns>
		///
		///</returns>
		///
		public new bool Equals(object x, object y) {
			return DateTimeRange.Equals(x, y);
		}

		///<summary>
		///
		///            Get a hashcode for the instance, consistent with persistence "equality"
		///            
		///</summary>
		///
		public int GetHashCode(object x) {
			return x.GetHashCode();
		}

		///<summary>
		///
		///            Retrieve an instance of the mapped class from a IDataReader. Implementors
		///            should handle possibility of null values.
		///            
		///</summary>
		///
		///<param name="dr">IDataReader</param>
		///<param name="names">the column names</param>
		///<param name="session"></param>
		///<param name="owner">the containing entity</param>
		///<returns>
		///
		///</returns>
		///
		public object NullSafeGet(IDataReader dr, string[] names, ISessionImplementor session, object owner) {
			DateTime? start = (DateTime?) NHibernateUtil.DateTime.NullSafeGet(dr, names[0], session, owner);
			DateTime? end = (DateTime?) NHibernateUtil.DateTime.NullSafeGet(dr, names[1], session, owner);
			if (start != null && end != null && start.Value == NullRangeStart && end.Value == NullRangeEnd)
				return null;
			return Create(start, end);
		}

		///<summary>
		///
		///            Write an instance of the mapped class to a prepared statement.
		///            Implementors should handle possibility of null values.
		///            A multi-column type should be written to parameters starting from index.
		///            
		///</summary>
		///
		///<param name="cmd"></param>
		///<param name="value"></param>
		///<param name="index"></param>
		///<param name="session"></param>
		public void NullSafeSet(IDbCommand cmd, object value, int index, ISessionImplementor session) {
			T dr = (T) value;
			if (value == null) {
				NHibernateUtil.DateTime.NullSafeSet(cmd, NullRangeStart, index, session);
				NHibernateUtil.DateTime.NullSafeSet(cmd, NullRangeEnd, index + 1, session);
			}
			else {
				NHibernateUtil.DateTime.NullSafeSet(cmd, dr.Start, index, session);
				NHibernateUtil.DateTime.NullSafeSet(cmd, dr.End, index + 1, session);
			}
		}

		///<summary>
		///
		///            Return a deep copy of the persistent state, stopping at entities and at collections.
		///            
		///</summary>
		///
		///<param name="value">generally a collection element or entity field</param>
		///<returns>
		///
		///</returns>
		///
		public object DeepCopy(object value) {
			T dr = (T) value;
			if (dr == null)
				return null;
			return Create(dr.Start, dr.End);
		}

		///<summary>
		///
		///            Transform the object into its cacheable representation.
		///            At the very least this method should perform a deep copy.
		///            That may not be enough for some implementations, method should perform a deep copy. That may not be enough for some implementations, however; for example, associations must be cached as identifier values. (optional operation)
		///            
		///</summary>
		///
		///<param name="value">the object to be cached</param>
		///<param name="session"></param>
		///<returns>
		///
		///</returns>
		///
		public object Disassemble(object value, ISessionImplementor session) {
			return DeepCopy(value);
		}

		///<summary>
		///
		///            Reconstruct an object from the cacheable representation.
		///            At the very least this method should perform a deep copy. (optional operation)
		///            
		///</summary>
		///
		///<param name="cached">the object to be cached</param>
		///<param name="session"></param>
		///<param name="owner"></param>
		///<returns>
		///
		///</returns>
		///
		public object Assemble(object cached, ISessionImplementor session, object owner) {
			return DeepCopy(cached);
		}

		///<summary>
		///
		///            During merge, replace the existing (target) value in the entity we are merging to
		///            with a new (original) value from the detached entity we are merging. For immutable
		///            objects, or null values, it is safe to simply return the first parameter. For
		///            mutable objects, it is safe to return a copy of the first parameter. However, since
		///            composite user types often define component values, it might make sense to recursively 
		///            replace component values in the target object.
		///            
		///</summary>
		///
		public object Replace(object original, object target, ISessionImplementor session, object owner) {
			return original;
		}

		///<summary>
		///
		///            Get the "property names" that may be used in a query. 
		///            
		///</summary>
		///
		public string[] PropertyNames {
			get { return propertyNames; }
		}

		///<summary>
		///
		///            Get the corresponding "property types"
		///            
		///</summary>
		///
		public IType[] PropertyTypes {
			get { return propertyTypes; }
		}

		///<summary>
		///
		///            The class returned by NullSafeGet().
		///            
		///</summary>
		///
		public Type ReturnedClass {
			get { return typeof (T); }
		}

		///<summary>
		///
		///            Are objects of this type mutable?
		///            
		///</summary>
		///
		public bool IsMutable {
			get { return false; }
		}

		#endregion

		protected abstract T Create(DateTime? start, DateTime? end);

		///<summary>
		///
		///            During merge, replace the existing (<paramref name="target" />) value in the entity
		///            we are merging to with a new (<paramref name="original" />) value from the detached
		///            entity we are merging. For immutable objects, or null values, it is safe to simply
		///            return the first parameter. For mutable objects, it is safe to return a copy of the
		///            first parameter. For objects with component values, it might make sense to
		///            recursively replace component values.
		///            
		///</summary>
		///
		///<param name="original">the value from the detached entity being merged</param>
		///<param name="target">the value in the managed entity</param>
		///<param name="owner">the managed entity</param>
		///<returns>
		///the value to be merged
		///</returns>
		///
		public object Replace(object original, object target, object owner) {
			return original;
		}

		///<summary>
		///
		///            Reconstruct an object from the cacheable representation. At the very least this
		///            method should perform a deep copy if the type is mutable. (optional operation)
		///            
		///</summary>
		///
		///<param name="cached">the object to be cached</param>
		///<param name="owner">the owner of the cached object</param>
		///<returns>
		///a reconstructed object from the cachable representation
		///</returns>
		///
		public object Assemble(object cached, object owner) {
			return cached;
		}

		///<summary>
		///
		///            Transform the object into its cacheable representation. At the very least this
		///            method should perform a deep copy if the type is mutable. That may not be enough
		///            for some implementations, however; for example, associations must be cached as
		///            identifier values. (optional operation)
		///            
		///</summary>
		///
		///<param name="value">the object to be cached</param>
		///<returns>
		///a cacheable representation of the object
		///</returns>
		///
		public object Disassemble(object value) {
			return value;
		}
	}
}
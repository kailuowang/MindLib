using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MindHarbor.CollectionWrappers {
	public class PropertyEnumerableWrapper<ContainerT, PropertyT> : IEnumerable<PropertyT> {
		private IEnumerable<ContainerT> innerEnumerable;
		private string propertyName;

		public PropertyEnumerableWrapper(IEnumerable<ContainerT> toWrap, string propertyName) {
			innerEnumerable = toWrap;
			this.propertyName = propertyName;
			CheckType();
		}

		#region IEnumerable<PropertyT> Members

		public IEnumerator<PropertyT> GetEnumerator() {
			return new PropertyEnumeratorWrapper<ContainerT, PropertyT>(innerEnumerable.GetEnumerator(), propertyName);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new PropertyEnumeratorWrapper<ContainerT, PropertyT>(innerEnumerable.GetEnumerator(), propertyName);
		}

		#endregion

		public override bool Equals(object obj) {
			if (!obj.GetType().Equals(GetType())) return false;
			if (obj == this) return true;
			return innerEnumerable.Equals(((PropertyEnumerableWrapper<ContainerT, PropertyT>) obj).innerEnumerable);
		}

		public override int GetHashCode() {
			return innerEnumerable.GetHashCode();
		}

		/// <summary>
		/// check if the ContainerT's property is of the propertyType 
		/// </summary>
		private void CheckType() {
			PropertyInfo pi = typeof (ContainerT).GetProperty(propertyName);
			if (pi == null
			    || !pi.PropertyType.Equals(typeof (PropertyT)))
				throw new ArgumentException(typeof (ContainerT).Name
				                            + " does not have a " + typeof (PropertyT).Name
				                            + " property named " + propertyName);
		}
	}
}
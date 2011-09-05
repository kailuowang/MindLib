using System;
using System.Collections.Generic;
using System.Reflection;

namespace MindHarbor.CollectionWrappers {
	public class PropertyEnumeratorWrapper<ContainerT, PropertyT> : IEnumerator<PropertyT> {
		private IEnumerator<ContainerT> innerEnumerator;
		private PropertyInfo pi;
		private string propertyName;

		public PropertyEnumeratorWrapper(IEnumerator<ContainerT> toWrap, string propertyName) {
			innerEnumerator = toWrap;
			this.propertyName = propertyName;
			CheckType();
		}

		#region IEnumerator<PropertyT> Members

		PropertyT IEnumerator<PropertyT>.Current {
			get { return (PropertyT) Current; }
		}

		public void Dispose() {
			innerEnumerator.Dispose();
			innerEnumerator = null;
			propertyName = null;
		}

		public bool MoveNext() {
			return innerEnumerator.MoveNext();
		}

		public void Reset() {
			innerEnumerator.Reset();
		}

		public object Current {
			get {
				ContainerT cc = innerEnumerator.Current;
				return GetPropVal(cc);
			}
		}

	

	    #endregion

        internal PropertyT GetPropVal(ContainerT cc)
        {
            return (PropertyT)pi.GetValue(cc, null);
        }

		public override bool Equals(object obj) {
			if (!obj.GetType().Equals(GetType())) return false;
			if (obj == this) return true;
			return innerEnumerator.Equals(((PropertyEnumeratorWrapper<ContainerT, PropertyT>) obj).innerEnumerator);
		}

		public override int GetHashCode() {
			return innerEnumerator.GetHashCode();
		}

		/// <summary>
		/// check if the ContainerT's property is of the propertyType 
		/// </summary>
		private void CheckType() {
			pi = typeof (ContainerT).GetProperty(propertyName);
			if (pi == null || pi.PropertyType != typeof (PropertyT))
				throw new ArgumentException(typeof (ContainerT).Name
				                            + " does not have a " + typeof (PropertyT).Name
				                            + " property named " + propertyName);
		}
	}
}
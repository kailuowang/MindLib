using System;

namespace MindHarbor.MiscNHibernateUserTypes {
	/// <summary>
	/// a heterogeneous custom property
	/// </summary>
	/// <remarks>
	/// it was used in <see cref="HeterogeneousPropertyDict"/>
	/// </remarks>
    public class HeterogeneousProperty : IEquatable<HeterogeneousProperty> {
		private string name;
		private string stringValue;
		private Type valueType = typeof(string);
        
        /// <summary>
        /// Gets and sets the value 
        /// </summary>
		public object Value {
			get {
				if (string.IsNullOrEmpty(stringValue))
					return default(ValueType);
				return Convert.ChangeType(stringValue, valueType);
			}
			set {
				if (value == null)
					stringValue = null;
				else {
					stringValue = value.ToString();
					ValueType = value.GetType();
				}
			}
		}

        /// <summary>
        /// Gets the Type of this property
        /// </summary>
		public Type ValueType {
			get { return valueType; }
			private set { valueType = value; }
		}

        /// <summary>
        /// Gets the name of the property
        /// </summary>
		public string Name {
			get { return name; }
			private set { name = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
		public HeterogeneousProperty(string name, object val) {
			this.name = name;
			Value = val;
		}

		private HeterogeneousProperty() {}

	    ///<summary>
	    ///Indicates whether the current object is equal to another object of the same type.
	    ///</summary>
	    ///
	    ///<returns>
	    ///true if the current object is equal to the other parameter; otherwise, false.
	    ///</returns>
	    ///
	    ///<param name="other">An object to compare with this object.</param>
	    public bool Equals(HeterogeneousProperty other) {
			if (other == null) return false;
			if (!Equals(name, other.name)) return false;
			if (!Equals(stringValue, other.stringValue)) return false;
			if (!Equals(valueType, other.valueType)) return false;
			return true;
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
			return Equals(obj as HeterogeneousProperty);
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
			int result = name.GetHashCode();
			result = 29*result + (stringValue != null ? stringValue.GetHashCode() : 0);
			result = 29*result + valueType.GetHashCode();
			return result;
		}
	}
}
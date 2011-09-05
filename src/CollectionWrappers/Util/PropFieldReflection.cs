using System;
using System.Reflection;

namespace MindHarbor.CollectionWrappers.Util {
	public class PropFieldReflection {
		private readonly FieldInfo fi;
		private readonly string name;
		private readonly object o;

		private readonly PropertyInfo pi;

		public PropFieldReflection(string name, object o) {
			this.name = name;
			this.o = o;
			pi = o.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (pi == null)
				fi = o.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (fi == null)
				throw new Exception(o.GetType() + " does not have the propery or field with the name " + name);
		}

		public object Val {
			get {
				if (pi != null)
					return pi.GetValue(o, null);
				else
					return fi.GetValue(o);
			}
		}
	}
}
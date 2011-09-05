using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace MindHarbor.ClassEnum {
	/// <summary>
	/// The base class for Class Enumeration
	/// </summary>
	/// <remarks>
	/// Do not inherit from this class, inherit from <see cref="ClassEnumGeneric"/>.
	/// Types can be automatically initialized 
	/// or you can use a AppSetting with the name "MindHarbor.ClassEnum.ClassEnumTypes" to define which ClassEnum types to be included.
	/// 
	/// </remarks>
	[Serializable]
	public abstract class ClassEnumBase : IClassEnum {
		private static readonly ClassEnumRepo repo = new ClassEnumRepo();
		private string name;

		protected ClassEnumBase(string name) {
			Name = name;
			repo.Add(this);
		}

		#region IClassEnum Members

		public string Name {
			get { return name; }
			private set { name = value; }
		}

		#endregion

		public override string ToString() {
			return Name;
		}

		protected static T Parse<T>(string enumName) where T : IClassEnum {
			return (T) Parse(typeof (T), enumName);
		}

		protected static ICollection<IClassEnum> All(Type t) {
			return repo.Find(t);
		}

		public static bool operator !=(ClassEnumBase classEnumBase1, ClassEnumBase classEnumBase2) {
			return !Equals(classEnumBase1, classEnumBase2);
		}

		public static bool operator ==(ClassEnumBase classEnumBase1, ClassEnumBase classEnumBase2) {
			return Equals(classEnumBase1, classEnumBase2);
		}

		protected bool Equals(ClassEnumBase classEnumBase) {
			if (classEnumBase == null) return false;
			return GetType().Equals(classEnumBase.GetType()) && Equals(name, classEnumBase.name);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as ClassEnumBase);
		}

		public override int GetHashCode() {
			return name.GetHashCode();
		}

		/// <summary>
		/// Parse the name to the T type of enumeration
		/// </summary>
		/// <param name="t"></param>
		/// <param name="enumName"></param>
		/// <returns></returns>
		/// <remarks>
		/// This method is only for ClassEnumUserType to use
		/// </remarks>
		public static IClassEnum Parse(Type t, string enumName) {
			return repo.Find(t, enumName);
		}

		#region Nested type: ClassEnumRepo

		private class ClassEnumRepo {
			private static readonly IDictionary<Type, IDictionary<string, IClassEnum>> repos =
				new Dictionary<Type, IDictionary<string, IClassEnum>>();

			private static bool typesInConfigInitialized;

			public void Add(IClassEnum item) {
				Type t = FindBaseType(item);

				string name = item.Name;
				if (!repos.ContainsKey(t))
					repos.Add(t, new Dictionary<string, IClassEnum>());
				if (repos[t].ContainsKey(name))
					throw new ClassEnumException("The name " + name + " is already used.");
				repos[t].Add(name, item);
			}

			private static Type FindBaseType(IClassEnum item) {
				Type retVal = item.GetType();
				while (retVal.BaseType.BaseType != typeof (ClassEnumBase))
					retVal = retVal.BaseType;
				return retVal;
			}

			public IClassEnum Find(Type t, string name) {
				CheckType(t);
				if (string.IsNullOrEmpty(name))
					throw new ArgumentNullException("Cannot find ClassEnum with null Name");
				if (!repos[t].ContainsKey(name))
					throw new ClassEnumException("Cannot find enum with name " + name);
				return repos[t][name];
			}

			private void CheckType(Type t) {
				InitializeTypesInConfig();
				if (!repos.ContainsKey(t))
					InitializeType(t);
				if (!repos.ContainsKey(t))
					TypeNotFound(t);
			}

			/// <summary>
			/// To initialize the ClassEnum type
			/// </summary>
			/// <param name="t"></param>
			public static void InitializeType(Type t) {
				if (!t.IsSubclassOf(typeof (ClassEnumBase)))
					return;
				FieldInfo[] fis = t.GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
				if (fis.Length > 0)
					fis[0].GetValue(null);

				InitializeType(t.BaseType);
			}

			private static void InitializeTypesInConfig() {
				if (typesInConfigInitialized)
					return;
				typesInConfigInitialized = true;
				foreach (Type t in TypesInConfig()) InitializeType(t);
			}

			private static IEnumerable<Type> TypesInConfig() {
				string namesSetting = ConfigurationManager.AppSettings["MindHarbor.ClassEnum.ClassEnumTypes"];
				if (!string.IsNullOrEmpty(namesSetting)) {
					string[] names = namesSetting.Split(new char[] {';'});
					Type[] retVal = new Type[names.Length];
					for (int i = 0; i < names.Length; i++)
						retVal[i] = Type.GetType(names[i], true);
					return retVal;
				}
				return new Type[0];
			}

			private static void TypeNotFound(Type t) {
				throw new ClassEnumException("Type " + t.ToString() + " not found");
			}

			public ICollection<IClassEnum> Find(Type t) {
				CheckType(t);
				return repos[t].Values;
			}
		}

		#endregion
	}
}
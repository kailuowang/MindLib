using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MindHarbor.SingletonUtil {
	/// <summary>
	/// A repository for storing singletons of types
	/// </summary>
	/// <remarks>
	/// This class works as a global repository for a number of classes whose only singleton is needed for an assembly. 
	/// For this repository to work, a setting xml file must be included in the client assembly as an embedded resource.
	/// The setting file should be named as SingletonRepositorySettings.xml
	/// This repository can only be used upon those who can apply the singleton pattern.
	/// <example>
	/// <![CDATA[
	///<?xml version="1.0" encoding="utf-8" ?>
	/// <SingletonRepositorySettings>
	///     <registeredTypes namespace="mmpire.Domain">
	///         <type name="Work.WorkManager" />
	///         <type name="People.GroupRepository" />
	///     </retisteredTypes>
	/// </SingletonRepositorySettings>
	/// ]]>
	/// </example>
	/// 
	/// Call <see cref="AddAssembly(Assembly a)"/> to add assembly first.
	/// Or, assembly names can be add to the AppSettings under the key "SingletonRepositoryAssemblies", so that this repository will automatically add them.
	/// multiple assemblies can be set in the appSetting seperated by ',' or ' ' or ';'
	/// 
	/// Types that are not registered in the repository but have a static Instance property or
	///  a parameterless constructor will also be found through this repository. 
	/// However they won't be found through the polymorphic <see cref="FindAll<T>()"/>
	/// </remarks>
	public class SingletonRepository {
		#region private members

		#region fields

		private const string AssemblySettingKeyName = "SingletonRepositoryAssemblies";
		private const string NamespaceKeyName = "namespace";
		private const string ResourceName = "SingletonRepositorySettings.xml";

		private static readonly SingletonRepository instance = new SingletonRepository();
		private static readonly IDictionary<Type, object> items = new Dictionary<Type, object>();
		private static readonly object lockObj = new object();

		#endregion

		private SingletonRepository() {}

		static SingletonRepository()
		{
			foreach (Assembly a in Assemblies())
				UnsafeAddAssembly(a);
		}

		private static ICollection<Assembly> Assemblies() {
			IList<Assembly> retVal = new List<Assembly>();
			string dan = ConfigurationManager.AppSettings[AssemblySettingKeyName];
			if (!string.IsNullOrEmpty(dan)) {
				string[] dans = dan.Split(new char[] {',', ' ', ';'});
				foreach (string name in dans)
					retVal.Add(Assembly.Load(name));
			}
			if (retVal.Count == 0)
				return AssembliesFromStackTrace();
			return retVal;
		}

		private static ICollection<Assembly> AssembliesFromStackTrace() {
			IList<Assembly> retVal = new List<Assembly>();
			foreach (StackFrame s in new StackTrace().GetFrames()) {
				Assembly a = Assembly.GetAssembly(s.GetMethod().DeclaringType);
				retVal.Add(a);
			}
			return retVal;
		}

		private static void UnsafeAddAssembly(Assembly a) {
			foreach (string typeName in DefinedTypeNames(a)) {
				Type t = Type.GetType(typeName, true);
				UnsafeRegister(t);
			}
		}

		private static IEnumerable<string> DefinedTypeNames(Assembly a) {
			List<string> retVal = new List<string>();
			XmlDocument xd = SettingXml(a, ResourceName);
			if (xd == null)
				return new string[0];
			foreach (XmlNode xn in xd.ChildNodes[1].ChildNodes)
				retVal.AddRange(ParseNamespace(a, xn));

			return retVal;
		}

		/// <summary>
		/// register a type which is not threadsafe
		/// </summary>
		/// <param name="t"></param>
		private static void UnsafeRegister(Type t) {
			if (t.IsInterface)
				throw new ArgumentException(t.Name +
				                            " is an interface. only class type can be registered into the repostory");
			if (items.Keys.Contains(t))
				return;

			//only store the instance when the type does not have a static "Instance" property
			if (SingletonInstanceLoader.LoadByInstanceProperty(t) == null)
				items.Add(t, SingletonInstanceLoader.LoadByConstrutor(t));
			else
				items.Add(t, null);
		}

		#region parsing Xml

		private static List<string> ParseNamespace(Assembly a, XmlNode xn) {
			List<string> retVal = new List<string>();
			if (xn.Name == "#comment")
				return retVal;
			if (xn.Name != "registeredTypes")
				throw new SingletonUtilException("Parsing setting xml failed. Coult not parse node: " + xn.Name +
				                                 ". Expected Node: registeredTypes");
			string defaultNamespace = string.Empty;
			if (xn.Attributes[NamespaceKeyName] != null)
				defaultNamespace = xn.Attributes[NamespaceKeyName].Value;
			foreach (XmlNode n in xn.ChildNodes) {
				if (n.Name == "#comment")
					continue;
				ValidateTypeNode(n);
				string typeName = n.Attributes["name"].Value;
				retVal.Add(defaultNamespace + "." + typeName + "," + a.GetName().Name);
			}
			return retVal;
		}

		private static void ValidateTypeNode(XmlNode n) {
			if (n.Name != "type")
				throw new SingletonUtilException("Parsing setting xml failed. Coult not parse node: " + n.Name +
				                                 ". Expected Node: type");
			if (n.Attributes["name"] == null)
				throw new SingletonUtilException(
					"Parsing setting xml failed. Coult not find the attribute \"name\" in the type node");
		}

		private static XmlDocument SettingXml(Assembly a, string resourceName) {
			Stream s = a.GetManifestResourceStream(a.GetName().Name + "." + resourceName);
			if (s == null)
				return null;
			XmlReader xr = new XmlTextReader(s);
			try {
				XmlDocument retVal = new XmlDocument();
				retVal.Load(xr);
				return retVal;
			}
			finally {
				xr.Close();
			}
		}

		#endregion

		#endregion

		/// <summary>
		/// Get the singleton instance of the repository
		/// </summary>
		public static SingletonRepository Instance {
			get { return instance; }
		}

		/// <summary>
		/// Find the singleton of the type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		/// <see cref="Find(Type t)"/>
		public T Find<T>() where T : class {
			return (T) Find(typeof (T));
		}

		/// <summary>
		/// Find the singleton of the Type t
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		/// <remarks>
		/// This is a none generic version of Find&lt;T&gt;()
		/// This will even found singelton for types that are not registered in the system but have a static Instance property
		/// </remarks>
		public object Find(Type t) {
			object retVal = SingletonInstanceLoader.LoadByInstanceProperty(t);
			if (retVal != null)
				return retVal;
			if (!items.ContainsKey(t))
				if (SingletonInstanceLoader.Loadable(t))
					Register(t);
				else
					throw new SingletonUtilException("Type " + t.ToString() +
					                                 " is not a valide class that can be deemed as singleton");
			return items[t];
		}

		/// <summary>
		/// polymorphically find all singletons that assignable to T 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>the singletons of the Registered types in the repository</returns>
		public ICollection<T> FindAll<T>() {
			IList<T> retVal = new List<T>();
			foreach (Type t in items.Keys)
				if (typeof (T).IsAssignableFrom(t))
					retVal.Add((T) Find(t));
			return retVal;
		}

		/// <summary>
		/// Dynamically register a type to the repository
		/// </summary>
		/// <param name="t"></param>
		/// <remarks>
		/// if the t is already registered, this method will do nothing. 
		/// </remarks>
		public void Register(Type t) {
			lock (lockObj) {
				UnsafeRegister(t);
			}
		}

		/// <summary>
		/// Add an assembly to under the management of the repository
		/// </summary>
		/// <param name="a"></param>
		/// <remarks>
		/// This will register all the types in the setting file in this assembly
		/// </remarks>
		public void AddAssembly(Assembly a) {
			lock (lockObj) {
				UnsafeAddAssembly(a);
			}
		}
	}
}
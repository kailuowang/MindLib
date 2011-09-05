using System;
using System.Collections.Generic;
using MindHarbor.CollectionWrappers;

namespace MindHarbor.ClassEnum {
	/// <summary>
	/// Base class for ClassEnum inheritances
	/// </summary>
	/// <typeparam name="InheritanceT">the type of the inheritance</typeparam>
	/// <remarks>
	/// <example>
	/// <code>
	///  public class MockEnum1 : ClassEnumGeneric<MockEnum1>{
	///    public readonly  static MockEnum1 Item1 = new MockEnum1("item1");
	///    public readonly  static MockEnum1 Item2 = new MockEnum1("item2");
	///    private MockEnum1(string name):base(name) {}
	/// }
	/// </code>
	/// </example>
	/// </remarks>
	[Serializable]
	public abstract class ClassEnumGeneric<InheritanceT> : ClassEnumBase where InheritanceT : IClassEnum {
		protected ClassEnumGeneric(string name) : base(name) {}

		/// <summary>
		/// Gets the collection of all the items
		/// </summary>
		public static ICollection<InheritanceT> Items {
			get {
				return
					new CollectionTypeWrapper<InheritanceT, IClassEnum>(All(typeof (InheritanceT)));
			}
		}

		/// <summary>
		/// Parse the enum by name
		/// </summary>
		/// <param name="enumName"></param>
		/// <returns></returns>
		public static InheritanceT Parse(string enumName) {
			return Parse<InheritanceT>(enumName);
		}
	}
}
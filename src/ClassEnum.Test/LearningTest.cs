using System;
using System.Reflection;
using NUnit.Framework;

namespace MindHarbor.ClassEnum.Test {
	[TestFixture, Explicit]
	public class LearningTest {
		[Test]
		public void GetPropertyTest() {
			Type type = typeof (MockEnum1);

			foreach (FieldInfo info in type.GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public))
				Console.WriteLine(info.Name);
		}
	}
}
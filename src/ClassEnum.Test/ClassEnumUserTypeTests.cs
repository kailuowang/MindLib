using System;
using System.Collections;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;

namespace MindHarbor.ClassEnum.Test {
	[TestFixture]
	public class ClassEnumUserTypeTests :  TestCase {
		protected override IList Mappings {
			get { return new string[] {"MockPersistantWithClassEnum.hbm.xml"}; }
		}

		protected override string MappingsAssembly {
			get { return GetType().Assembly.GetName().Name; }
		}

		[Test]
		public void CRUDTest() {
			ISession sess = OpenSession();

			MockPersistantWithClassEnum m = new MockPersistantWithClassEnum();
			m.MEnum = MockEnum1.Item1;
			sess.Save(m);

			sess.Flush();
			sess.Clear();
			m = sess.Get<MockPersistantWithClassEnum>(m.Id);
			Assert.AreEqual(m.MEnum, MockEnum1.Item1);

			sess.Flush();
			sess.Clear();
			sess.Delete(m);
			sess.Flush();
			sess.Close();
		}

		[Test]
		public void QueryTest() {
			ISession sess = OpenSession();

			MockPersistantWithClassEnum m = new MockPersistantWithClassEnum();
			m.MEnum = MockEnum1.Item1;
			sess.Save(m);

			sess.Flush();
			sess.Clear();
			IList result = sess.CreateCriteria(typeof (MockPersistantWithClassEnum))
				.Add(Expression.Eq("MEnum", MockEnum1.Item1))
				.List();
			Assert.IsTrue(result.Count >= 1);
			Console.WriteLine(result.Count);
			Assert.IsTrue(result.Contains(sess.Get<MockPersistantWithClassEnum>(m.Id)));
			foreach (MockPersistantWithClassEnum item in result) Assert.AreEqual(MockEnum1.Item1, item.MEnum);
			sess.Flush();
			sess.Clear();
			sess.Delete(m);
			sess.Flush();
			sess.Close();
		}
	}
}
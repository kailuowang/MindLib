using System.Collections;
using NHibernate;
using NUnit.Framework;
using TestCase=NHibernate.Test.TestCase;

namespace MindHarbor.SingletonUtil.Test {
	[TestFixture]
	public class SingletonUserTypeTests : TestCase {
		protected override IList Mappings {
			get { return new string[] {"MockPersistantWithSingleton.hbm.xml"}; }
		}

		protected override string MappingsAssembly {
			get { return GetType().Assembly.GetName().Name; }
		}

		[Test]
		public void CRUDTest() {
			ISession sess = OpenSession();

			MockPersistantWithSingleton m = new MockPersistantWithSingleton();
			IStrategy s = SingletonRepository.Instance.Find<StrategyA>();
			Assert.IsNotNull(s);
			m.Strategy = s;
			sess.Save(m);

			sess.Flush();
			sess.Clear();
			m = sess.Get<MockPersistantWithSingleton>(m.Id);
			Assert.AreEqual(m.Strategy, s);

			sess.Flush();
			sess.Clear();
			sess.Delete(m);
			sess.Flush();
			sess.Close();
		}

		[Test]
		public void QueryTest() {
			ISession sess = OpenSession();

			MockPersistantWithSingleton m = new MockPersistantWithSingleton();
			IStrategy s = SingletonRepository.Instance.Find<StrategyA>();
			Assert.IsNotNull(s);
			m.Strategy = s;
			sess.Save(m);

			sess.Flush();
			sess.Clear();
			IList result = sess.CreateQuery("from MockPersistantWithSingleton ms where ms.Strategy = :s")
				.SetParameter("s", s, SingletonUserType.CustomeType).List();
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Contains(sess.Get<MockPersistantWithSingleton>(m.Id)));
			foreach (MockPersistantWithSingleton item in result) Assert.AreEqual(s, item.Strategy);
			sess.Flush();
			sess.Clear();
			sess.Delete(m);
			sess.Flush();
			sess.Close();
		}
	}
}
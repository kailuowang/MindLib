using System;
using System.Collections;
using System.Collections.Generic;
using MindHarbor.GenClassLib.Data;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using TestCase=NHibernate.Test.TestCase;

namespace MindHarbor.MiscNHibernateUserTypes.Test {
	[TestFixture]
	public class EncryptedStringTypeTests : TestCase {
		private MockPersistantClass m;
		private ISession sess;
		private readonly string password = RandomName();

		protected override IList Mappings {
			get { return new string[] {"MockPersistantCalss.hbm.xml"}; }
		}

		protected override string MappingsAssembly {
			get { return GetType().Assembly.GetName().Name; }
		}

		protected static string RandomName() {
			return RandomStringGenerator.GenerateLetterStrings(4);
		}

		protected static int RandomInt() {
			return Math.Abs(RandomStringGenerator.GenerateLetterStrings(8).GetHashCode()/1000);
		}

		protected override void OnSetUp() {
			sess = OpenSession();
			m = new MockPersistantClass();
			m.Password = password;
			sess.Save(m);
			CommitAndReloadMock();
		}

		private void CommitAndReloadMock() {
			sess.Flush();
			sess.Clear();
			m = sess.Get<MockPersistantClass>(m.Id);
		}

		protected override void OnTearDown() {
			sess.Flush();
			sess.Clear();
			sess.Delete(m);
			sess.Flush();
			sess.Close();
		}

		private void TestSearchResult(IList<MockPersistantClass> result) {
			Assert.AreEqual(1, result.Count);

			Assert.IsTrue(result.Contains(m));
			Assert.AreEqual(password, result[0].Password);
		}

		[Test]
		public void CriteriaTest() {
			IList<MockPersistantClass> result = sess.CreateCriteria(typeof (MockPersistantClass))
				.Add(Expression.Eq("Password", password)).List<MockPersistantClass>();
			TestSearchResult(result);
		}

		[Test]
		public void CRUDTest() {
			Assert.AreEqual(password, m.Password);
		}

		[Test]
		public void QueryTest() {
			IList<MockPersistantClass> result = sess.CreateQuery("from MockPersistantClass ms where ms.Password = :d")
				.SetParameter("d", password, EncryptedStringType.CustomeType).List<MockPersistantClass>();

			TestSearchResult(result);
		}
	}
}
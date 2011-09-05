using System;
using System.Collections;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using TestCase=NHibernate.Test.TestCase;

namespace MindHarbor.TimeDataUtil.Test {
	[TestFixture]
	public class TimeDataUtilUserTypeTests : TestCase {
		private DateTime? d1 = new DateTime(2000, 1, 1);
		private DateTime? d2 = new DateTime(2000, 1, 2);
		private DateTime? d4 = new DateTime(2000, 1, 4);
		private DateTime? d5 = new DateTime(2000, 1, 5);
		private DateTime? d6 = new DateTime(2000, 1, 6);
		private MockPersistantClass m;
		private ISession sess;

		protected override IList Mappings {
			get { return new String[] {"MockPersistantCalss.hbm.xml"}; }
		}

		protected override string MappingsAssembly {
			get { return "MindHarbor.TimeDataUtil.Test"; }
		}

		protected override void OnSetUp() {
			sess = OpenSession();
			m = new MockPersistantClass();
			m.Range = new DateRange(d1, d5);
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
			sess.Disconnect();
			sess.Close();
		}

		private void TestRangeCRUD(DateRange testRange) {
			m.Range = testRange;
			CommitAndReloadMock();
			Assert.AreEqual(testRange, m.Range);
		}

		private void TestDateRange(DateRange range, bool shouldContain) {
			IList result = sess.CreateCriteria(typeof (MockPersistantClass))
				.Add(range.WithinExpression("Range"))
				.List();
			Assert.AreEqual(shouldContain, result.Contains(sess.Get<MockPersistantClass>(m.Id)));
		}

		[Test]
		public void CriteriaEndTest() {
			IList result = sess.CreateCriteria(typeof (MockPersistantClass))
				.Add(Expression.Eq("Range.End", new DateRange(d1, d5).End)).List();
			Assert.IsTrue(result.Count >= 1);
			Assert.IsTrue(result.Contains(sess.Get<MockPersistantClass>(m.Id)));
			foreach (MockPersistantClass item in result) Assert.AreEqual(d1, item.Range.Start);
		}

		[Test]
		public void CriteriaRangeTest() {
			IList result = sess.CreateCriteria(typeof (MockPersistantClass))
				.Add(Expression.Eq("Range", new DateRange(d1, d5)))
				.List();
			Assert.IsTrue(result.Count >= 1);
			Assert.IsTrue(result.Contains(sess.Get<MockPersistantClass>(m.Id)));
			foreach (MockPersistantClass item in result) Assert.AreEqual(new DateRange(d1, d5), item.Range);
		}

		[Test]
		public void CRUDTest() {
			//The setup and teardown create and delete the mockclass m
			Assert.AreEqual(new DateRange(d1, d5), m.Range);
			TestRangeCRUD(new DateRange(d2, null));
			TestRangeCRUD(new DateRange(null, null));
			TestRangeCRUD(null);
		}

		[Test]
		public void QueryStartTest() {
			IList result = sess.CreateQuery("from MockPersistantClass ms where ms.Range.Start = :d")
				.SetDateTime("d", d1.Value).List();

			Assert.IsTrue(result.Count >= 1);

			Assert.IsTrue(result.Contains(sess.Get<MockPersistantClass>(m.Id)));
			foreach (MockPersistantClass item in result) {
				Assert.AreEqual(d1, item.Range.Start);
				Console.WriteLine(item.Range.End);
			}
		}

		[Test]
		public void WithinExpressionTest() {
			TestDateRange(new DateRange(d1, d5), true);
			TestDateRange(new DateRange(d1, d6), true);
			TestDateRange(new DateRange(null, null), true);
			TestDateRange(new DateRange(d1, null), true);
			TestDateRange(new DateRange(null, d5), true);
			TestDateRange(new DateRange(d2, null), false);
			TestDateRange(new DateRange(null, d4), false);
			TestDateRange(new DateRange(d1, d4), false);
			TestDateRange(new DateRange(d2, d5), false);
		}
	}
}
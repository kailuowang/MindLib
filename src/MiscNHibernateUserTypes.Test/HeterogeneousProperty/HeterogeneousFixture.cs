using System.Collections;
using NHibernate;
using NUnit.Framework;

using TestCase = NHibernate.Test.TestCase;

namespace MindHarbor.MiscNHibernateUserTypes.Test.HeterogeneousProperty {
	[TestFixture]
	public class HeterogeneousFixture : TestCase {
		[Test]
		public void DictionaryTest() {
	        HeterogeneousPropertyDict dict = new HeterogeneousPropertyDict();
			string key1 = "afloat";
			string key2 = "aint";
			dict.Add(key1, 12.4f);
			dict.Add(key2, 12);
			Assert.AreEqual(12.4f, dict[key1]);
			Assert.AreEqual(12, dict[key2]);
			Assert.AreEqual(typeof(float), dict.GetValueType(key1));
			Assert.AreEqual(typeof(int), dict.GetValueType(key2));
			dict[key2] =  12.5f;
			Assert.AreEqual(12.5f, dict[key2]);
			Assert.AreEqual(typeof(float), dict.GetValueType(key2));
		}


		[Test]
		public void PersistenceTest(){
			Foo f = new Foo();
			string key1 = "afloat";
			string key2 = "aint";
			f.Props.Add(key1, 12.4f);
			f.Props.Add(key2, 12);
			ISession sess = OpenSession();
			sess.Persist(f);
			sess.Flush();
			sess.Close();

			sess = OpenSession();
			f = sess.Get<Foo>( f.Id);
			Assert.AreEqual(12.4f, f.Props[key1]);
			Assert.AreEqual(12, f.Props[key2]);
			Assert.AreEqual(typeof(float),f.Props.GetValueType(key1));
			Assert.AreEqual(typeof(int), f.Props.GetValueType(key2));

			sess.Delete(f);
			sess.Flush();
			sess.Close();

		}

		protected override IList Mappings
		{
			get { return new string[] { "HeterogeneousProperty.Foo.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return GetType().Assembly.GetName().Name; }
		}
	}
}
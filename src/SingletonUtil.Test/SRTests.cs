using NUnit.Framework;

namespace MindHarbor.SingletonUtil.Test {
	[TestFixture]
	public class SRTests {
		[Test]
		public void InstanceLoaderTest() {
			Assert.IsNotNull(SingletonInstanceLoader.LoadByConstrutor(typeof (StrategyA)));
			Assert.AreSame(StrategyB.Instance, SingletonInstanceLoader.LoadByInstanceProperty(typeof (StrategyB)));
		}

		[Test]
		public void RepositoryTest() {
			Assert.AreEqual(typeof (StrategyA), SingletonRepository.Instance.Find(typeof (StrategyA)).GetType());
			Assert.AreSame(StrategyB.Instance, SingletonRepository.Instance.Find<StrategyB>());
			Assert.AreEqual(2, SingletonRepository.Instance.FindAll<IStrategy>().Count);

			Assert.AreEqual(2, SingletonRepository.Instance.FindAll<IStrategy>().Count);
			Assert.IsNotNull(SingletonRepository.Instance.Find<StrategyC>());
			Assert.AreEqual(3, SingletonRepository.Instance.FindAll<IStrategy>().Count,
			                "Unknown class failed to be registered after saving");

			Assert.AreSame(StrategyD.Instance, SingletonRepository.Instance.Find<StrategyD>());
		}
	}
}
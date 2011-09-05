namespace MindHarbor.SingletonUtil.Test {
	public class StrategyA : IStrategy {}

	public class StrategyB : IStrategy {
		private static readonly StrategyB instance = new StrategyB();

		private StrategyB() {}

		public static StrategyB Instance {
			get { return instance; }
		}
	}

	public class StrategyC : IStrategy {}

	public class StrategyD : IStrategy {
		private static readonly StrategyD instance = new StrategyD();

		private StrategyD() {}

		public static StrategyD Instance {
			get { return instance; }
		}
	}

	public interface IStrategy {}
}
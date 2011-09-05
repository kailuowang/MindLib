namespace MindHarbor.GenClassLib.Data {
	public static class Tuples {
		///<summary>
		///</summary>
		///<param name="t1"></param>
		///<param name="t2"></param>
		///<typeparam name="T1"></typeparam>
		///<typeparam name="T2"></typeparam>
		///<returns></returns>
		public static TwoTuple<T1, T2> Tuple<T1, T2>(T1 t1, T2 t2) {
			return new TwoTuple<T1, T2>(t1, t2);
		}

		public static ThreeTuple<T1, T2, T3> Tuple<T1, T2, T3>(T1 t1, T2 t2, T3 t3) {
			return new ThreeTuple<T1, T2, T3>(t1, t2, t3);
		}
	}

	public class TwoTuple<T1, T2> {
		private T1 item1;
		private T2 item2;

		public TwoTuple(T1 item1, T2 item2) {
			this.item1 = item1;
			this.item2 = item2;
		}

		public T2 Item2 {
			get { return item2; }
			set { item2 = value; }
		}

		public T1 Item1 {
			get { return item1; }
			set { item1 = value; }
		}
	}

	public class ThreeTuple<T1, T2, T3> : TwoTuple<T1, T2> {
		private T3 item3;

		public ThreeTuple(T1 item1, T2 item2, T3 item3) : base(item1, item2) {
			this.item3 = item3;
		}

		public T3 Item3 {
			get { return item3; }
			set { item3 = value; }
		}
	}
}
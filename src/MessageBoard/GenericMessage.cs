namespace MindHarbor.MessageBoard {
	/// <summary>
	/// A generic message with a data object included
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericMessage<T> : IMessage {
		private T data;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public GenericMessage(T data) {
			Data = data;
		}

		///<summary>
		/// The data object associated with this message
		///</summary>
		public T Data {
			get { return data; }
			private set { data = value; }
		}
	}
}
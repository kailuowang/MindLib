namespace MindHarbor.GenClassLib.MessageBoard {
	/// <summary>
	/// A filter that does not filter out any messages
	/// </summary>
	public class NullMsgFilter : IMessageFilter {
		private static readonly NullMsgFilter instance = new NullMsgFilter();

		private NullMsgFilter() {}

		///<summary>
		///</summary>
		public static NullMsgFilter Instance {
			get { return instance; }
		}

		#region IMessageFilter Members

		/// <summary>
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>always true </returns>
		public bool Accept(IMessage msg) {
			return true;
		}

		#endregion
	}
}
namespace MindHarbor.GenClassLib.MessageBoard {
	public class BlockAllFilter : IMessageFilter {
		#region IMessageFilter Members

		public bool Accept(IMessage msg) {
			return false;
		}

		#endregion
	}
}
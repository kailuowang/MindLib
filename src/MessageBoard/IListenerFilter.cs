namespace MindHarbor.MessageBoard {
	public interface IListenerFilter {
		bool Accept(IMessageListener listener, IMessage msg);
	}
}
namespace MindHarbor.GenClassLib.MessageBoard {
	///<summary>
	/// A message broadcaster that works as an intermedia that can broadcast messages to all subscribers
	///</summary>
	public interface IMessageBroadcaster {
		/// <summary>
		/// Subscribe a listener to the broadcaster
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		bool Subscribe(IMessageListener subscriber);

		///<summary>
		///</summary>
		///<param name="subscriber"></param>
		///<param name="filter">the filter associated with this subscriber</param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		bool Subscribe(IMessageListener subscriber, IMessageFilter filter);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>the number of listeners that get the message</returns>
		int Broadcast(IMessage msg);

		/// <summary>
		/// Unsubscribe a listener from the subscribers list
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns> true if unscribe successfully, otherwise, false</returns>
		bool Unsubscribe(IMessageListener subscriber);
	}
}
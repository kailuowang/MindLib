/*
 * Created by: 
 * Created: Sunday, December 17, 2006
 */

namespace MindHarbor.MessageBoard {
	/// <summary>
	/// Class that listen to messages
	/// </summary>
	public interface IMessageListener {
		///<summary>
		///</summary>
		///<param name="msg"></param>
		void Listen(IMessage msg);
	}

	/// <summary>
	/// MessageListener that can turn itself off
	/// </summary>
	public interface IMessageListenerWithSwich : IMessageListener {
		///<summary>
		/// if this listenner is switched off
		///</summary>
		bool Off { get; }
	}
}
/*
 * Created by: 
 * Created: Sunday, December 17, 2006
 */

namespace MindHarbor.MessageBoard {
	///<summary>
	/// A Filter that filter out certain messages
	///</summary>
	public interface IMessageFilter {
		/// <summary>
		/// Indicate if the filter can pass through the message
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>true if the message can be passed through;
		/// false if the message is filtered out
		/// </returns>
		bool Accept(IMessage msg);
	}
}
namespace MindHarbor.MessageBoard {
	///<summary>
	/// Message that includes Source information
	///</summary>
	public interface IMessageWithSource : IMessage {
		///<summary>
		/// Gets the object from which the message is originated
		///</summary>
		object Source { get; }
	}
}
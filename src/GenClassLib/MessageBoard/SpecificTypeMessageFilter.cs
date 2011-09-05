namespace MindHarbor.GenClassLib.MessageBoard {
	///<summary>
	/// A filte that only accept message of the specific <typeparamref name="T"/>  
	///</summary>
	///<typeparam name="T">the type of the message accecpted</typeparam>
	public class SpecificTypeMessageFilter<T> : IMessageFilter {
		#region IMessageFilter Members

		/// <summary>
		/// Indicate if the filter can pass through the message
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>true if the message can be passed through;
		/// false if the message is filtered out
		/// </returns>
		public bool Accept(IMessage msg) {
			return typeof (T).IsInstanceOfType(msg);
		}

		#endregion
	}
}
using System;

namespace MindHarbor.GenClassLib.MessageBoard {
	public interface IMessageWithSourceBroadcaster {
		/// <summary>
		/// Subscribe a listener to the broadcaster
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		/// <param name="source">if null, then it'll be not source specific</param>
		bool Subscribe(IMessageListener subscriber, object source);

		///<summary>
		///</summary>
		///<param name="subscriber"></param>
		///<param name="filter">the filter associated with this subscriber</param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		///<param name="source"></param>
		bool Subscribe(IMessageListener subscriber, IMessageFilter filter, object source);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>the number of listeners that get the message</returns>
		/// <remarks>
		/// if msg has a null source, it'll be listened by all subscriber that subscribed with null source
		/// </remarks>
		int Broadcast(IMessageWithSource msg);

		/// <summary>
		/// Unsubscribe a listener from the subscribers list
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns> true if unscribe successfully, otherwise, false</returns>
		bool Unsubscribe(IMessageListener subscriber, object source);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="subscriber"></param>
		/// <param name="listeningTypes"></param>
		/// <param name="listeningSource"></param>
		/// <returns></returns>
		bool Subscribe(IMessageListener subscriber, object listeningSource, params Type[] listeningTypes);
	}
}
/*
 * Created by: 
 * Created: Sunday, December 17, 2006
 */

using System.Collections.Generic;
using log4net;

namespace MindHarbor.MessageBoard {
	/// <summary>
	/// The broadcaster that simple broad cast the message to all listeners
	/// </summary>
	public class MessageBroadcasterImpl : IMessageBroadcaster {
		private readonly IDictionary<IMessageListener, IMessageFilter> generalSubscribers =
			new Dictionary<IMessageListener, IMessageFilter>();

		#region IMessageBroadcaster Members

		/// <summary>
		/// Subscribe a listener to the broadcaster
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		public bool Subscribe(IMessageListener subscriber) {
            if (subscriber is IMessageListenerByType)
                return Subscribe((IMessageListenerByType) subscriber);
			return Subscribe(subscriber, null);
		}	
        
        /// <summary>
		/// Subscribe a listener to the broadcaster
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		public bool Subscribe(IMessageListenerByType subscriber) {
			return Subscribe(subscriber, new MessageFilterByType(subscriber.MessageType) );
		}

		///<summary>
		///</summary>
		///<param name="subscriber"></param>
		///<param name="filter">the filter associated with this subscriber</param>
		/// <returns>return if the <paramref name="subscriber"/>is already subscribed</returns>
		/// <remarks>if there is already a filer, subscribing will generate a DisJunction filter for them. <see cref="MessageFilters.DisJunctionFilter"/>
		/// </remarks>
		public bool Subscribe(IMessageListener subscriber, IMessageFilter filter) {
			if (filter == null)
				filter = NullMsgFilter.Instance;
			if (generalSubscribers.ContainsKey(subscriber)) {
				IMessageFilter existingFilter = generalSubscribers[subscriber];
				if (existingFilter != null)
					generalSubscribers[subscriber] = MessageFilters.DisJunctionFilter(
						new IMessageFilter[] {filter, existingFilter});
				else
					generalSubscribers[subscriber] = filter;
				return true;
			}
			else {
				generalSubscribers.Add(subscriber, filter);
				return false;
			}
		}

		/// <summary>
		/// Unsubscribe a listener from the generalSubscribers list
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns> true if unscribe successfully, otherwise, false</returns>
		public bool Unsubscribe(IMessageListener subscriber) {
			return generalSubscribers.Remove(subscriber);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>the number of listeners that get the message</returns>
		public int Broadcast(IMessage msg) {
			return Broadcast(msg, null);
		}

		#endregion

		public int Broadcast(IMessage msg, IListenerFilter lf) {
			int retVal = 0;
			KeyValuePair<IMessageListener, IMessageFilter>[] copied
				= new KeyValuePair<IMessageListener, IMessageFilter>[generalSubscribers.Count];
			generalSubscribers.CopyTo(copied, 0);
			foreach (KeyValuePair<IMessageListener, IMessageFilter> p in copied) {
				if (p.Key is IMessageListenerWithSwich && ((IMessageListenerWithSwich) p.Key).Off)
					continue;
				if (p.Value.Accept(msg)) {
					if (lf != null && !lf.Accept(p.Key, msg))
						continue;
					LogManager.GetLogger(typeof (MessageBroadcasterImpl)).Debug("message of type " + msg.GetType() + " was sent to " +
					                                                            p.Key.GetType());
					p.Key.Listen(msg);
					retVal++;
				}
			}
			return retVal;
		}
	}
}
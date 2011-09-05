using System;
using System.Collections.Generic;

namespace MindHarbor.GenClassLib.MessageBoard {
	///<summary>
	/// A simple implementation of IMessageWithSourceBroadcaster
	///</summary>
	/// <remarks>
	/// it's using a repository of normal MessageBroadcasterImpl
	/// </remarks>
	public class MessageWithSourceBroadcasterImpl : IMessageWithSourceBroadcaster {
		private readonly IDictionary<object, MessageBroadcasterImpl> broadcasterRepo
			= new Dictionary<object, MessageBroadcasterImpl>();

		private readonly MessageBroadcasterImpl nullSourceBroadcaster = new MessageBroadcasterImpl();

		#region IMessageWithSourceBroadcaster Members

		public bool Subscribe(IMessageListener subscriber, object source) {
			return GetSourceSpecificBroadcaster(source).Subscribe(subscriber);
		}

		public bool Subscribe(IMessageListener subscriber, IMessageFilter filter, object source) {
			return GetSourceSpecificBroadcaster(source).Subscribe(subscriber, filter);
		}

		public bool Subscribe(IMessageListener subscriber, object listeningSource, params Type[] listeningTypes) {
			return
				GetSourceSpecificBroadcaster(listeningSource).Subscribe(subscriber, new SpecificTypesMessageFilter(listeningTypes));
		}

		/// <summary>
		/// Will always also broadcast message to subscribers that subscribe to null sources
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public int Broadcast(IMessageWithSource msg) {
			DuplicationPreventionListenerFilter f = new DuplicationPreventionListenerFilter(msg);
			int retVal = 0;
			if (msg.Source != null)
				retVal = nullSourceBroadcaster.Broadcast(msg, f);
			return retVal + GetSourceSpecificBroadcaster(msg.Source).Broadcast(msg, f);
		}

		public bool Unsubscribe(IMessageListener subscriber, object source) {
			return GetSourceSpecificBroadcaster(source).Unsubscribe(subscriber);
		}

		#endregion

		private MessageBroadcasterImpl GetSourceSpecificBroadcaster(object source) {
			if (source == null)
				return nullSourceBroadcaster;
			if (!broadcasterRepo.ContainsKey(source))
				broadcasterRepo.Add(source, new MessageBroadcasterImpl());
			return broadcasterRepo[source];
		}
	}
}
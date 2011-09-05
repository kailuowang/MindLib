using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace MindHarbor.MessageBoard {
	/// <summary>
	/// A message filter that filter out messages according to their types
	/// </summary>
	public class SpecificTypesMessageFilter : IMessageFilter {
		/// <summary>
		/// The collection of the message types accecpeted
		/// </summary>
		private ICollection<Type> acceptedTypes;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="acceptedTypes">null acceptedTypes has the same effect as empty acceptedTypes, that is, no message will be accepted </param>
		public SpecificTypesMessageFilter(IEnumerable<Type> acceptedTypes) {
			this.acceptedTypes = new HashedSet<Type>();
			if (acceptedTypes != null)
				foreach (Type type in acceptedTypes)
					this.acceptedTypes.Add(type);
		}

		#region IMessageFilter Members

		/// <summary>
		/// Indicate if the filter can pass through the message
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>true if the message can be passed through;
		/// false if the message is filtered out
		/// </returns>
		public virtual bool Accept(IMessage msg) {
			return AcceptType(msg);
		}

		#endregion

		/// <summary>
		/// Judge if the type of the message is accecpted
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		protected bool AcceptType(IMessage msg) {
			foreach (Type type in acceptedTypes)
				if (type.IsInstanceOfType(msg))
					return true;
			return false;
		}
	}
}
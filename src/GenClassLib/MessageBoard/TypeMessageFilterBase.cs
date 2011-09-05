using System;
using System.Collections.Generic;

namespace MindHarbor.GenClassLib.MessageBoard {
	/// <summary>
	/// A base class for utilizing specificTypesMessageFilter
	/// </summary>
	public abstract class TypeMessageFilterBase : SpecificTypesMessageFilter {
		///<summary>
		///</summary>
		///<param name="acceptedTypes"></param>
		public TypeMessageFilterBase(IEnumerable<Type> acceptedTypes) : base(acceptedTypes) {}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override sealed bool Accept(IMessage msg) {
			if (!AcceptType(msg))
				return false;
			return AcceptByOtherCriteria(msg);
		}

		/// <summary>
		/// Override to add filter logic other than if Message Type is Accepted Type 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>true if accepted by other criteria, otherwise false  </returns>
		protected abstract bool AcceptByOtherCriteria(IMessage msg);
	}
}
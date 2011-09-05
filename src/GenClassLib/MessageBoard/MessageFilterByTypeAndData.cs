using System;
using System.Collections.Generic;

namespace MindHarbor.GenClassLib.MessageBoard {
	public class MessageFilterByTypeAndData<DataT> : TypeMessageFilterBase {
		private DataT data;

		public MessageFilterByTypeAndData(IEnumerable<Type> acceptedTypes, DataT d)
			: base(acceptedTypes) {
			data = d;
		}

		public DataT Data {
			get { return data; }
			set { data = value; }
		}

		protected override bool AcceptByOtherCriteria(IMessage msg) {
			if (Data == null) return true;
			return Data.Equals(((GenericMessage<DataT>) msg).Data);
		}
	}
}
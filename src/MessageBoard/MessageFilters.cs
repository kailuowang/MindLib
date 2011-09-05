using System;
using System.Collections.Generic;

namespace MindHarbor.MessageBoard {
	/// <summary>
	/// Static factory of message filters
	/// </summary>
	public class MessageFilters {
		public static IMessageFilter BlockAllFilter() {
			return new BlockAllFilter();
		}

		public static IMessageFilter PassAllFilter() {
			return NullMsgFilter.Instance;
		}

		public static IMessageFilter ConJunctionFilter(IEnumerable<IMessageFilter> filters) {
			return new JunctionFilter(filters, true);
		}

		public static IMessageFilter DisJunctionFilter(IEnumerable<IMessageFilter> filters) {
			return new JunctionFilter(filters, false);
		}

		public static IMessageFilter FilterByTypeAndData<DataT>(IEnumerable<Type> acceptedMessageTypes, DataT data) {
			return new MessageFilterByTypeAndData<DataT>(acceptedMessageTypes, data);
		}

		public static IMessageFilter FilterByType(IEnumerable<Type> acceptedMessageTypes) {
			return new SpecificTypesMessageFilter(acceptedMessageTypes);
		}
	}
}
using System;
using System.Collections.Generic;

namespace MindHarbor.MessageBoard {
	internal class JunctionFilter : IMessageFilter {
		private readonly bool requiresAllPassed;
		private IEnumerable<IMessageFilter> filters;

		public JunctionFilter(IEnumerable<IMessageFilter> junctFilters, bool requiresAllPassed) {
			bool allNull = true;
			foreach (IMessageFilter filter in junctFilters)
				if (filter != null) {
					allNull = false;
					break;
				}
			if (allNull) throw new ArgumentException("there must be aleast on non-null filter in the junctionFilters");
			filters = junctFilters;
			this.requiresAllPassed = requiresAllPassed;
		}

		public IEnumerable<IMessageFilter> Filters {
			get { return filters; }
			private set { filters = value; }
		}

		#region IMessageFilter Members

		public bool Accept(IMessage msg) {
			foreach (IMessageFilter filter in filters)
				if (filter != null && filter.Accept(msg) != requiresAllPassed)
					return !requiresAllPassed;
			return requiresAllPassed;
		}

		#endregion
	}
}
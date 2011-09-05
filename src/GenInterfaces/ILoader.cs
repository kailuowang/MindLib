using System;

namespace MindHarbor.GenInterfaces {
	public interface ILoader {
		object Load(Type t, object id);
	}
}
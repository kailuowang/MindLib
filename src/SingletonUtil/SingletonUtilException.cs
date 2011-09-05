/*
 * Created by: 
 * Created: Sunday, August 20, 2006
 */
using System;

namespace MindHarbor.SingletonUtil {
	public class SingletonUtilException : Exception {
		public SingletonUtilException(string msg) : base(msg) {}
		public SingletonUtilException(string msg, Exception innerException) : base(msg, innerException) {}
	}
}
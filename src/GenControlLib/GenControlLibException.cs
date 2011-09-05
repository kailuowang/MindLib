using System;

namespace MindHarbor.GenControlLib {
	public class GenControlLibException : Exception {
		public GenControlLibException() : base() {}
		public GenControlLibException(string msg) : base(msg) {}
		public GenControlLibException(string message, Exception innerException) : base(message, innerException) {}
	}
}
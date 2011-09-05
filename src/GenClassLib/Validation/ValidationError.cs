using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.GenClassLib.Validation {
	/// <summary>
	/// Summary description for ValidationError.
	/// </summary>
	[Serializable]
	public class ValidationError {
		private string _errorMessage;
		private int _errorNumber = -1;

		public ValidationError() {}

		public ValidationError(string errorMessage) {
			ErrorMessage = errorMessage;
		}

		public ValidationError(string errorMessage, int errorNumber) {
			ErrorMessage = errorMessage;
			ErrorNumber = errorNumber;
		}

		#region Properties

		public string ErrorMessage {
			get { return _errorMessage; }
			set { _errorMessage = value; }
		}

		public int ErrorNumber {
			get { return _errorNumber; }
			set { _errorNumber = value; }
		}

		#endregion

		public static string ToString(IEnumerable<ValidationError> ves) {
			StringBuilder sb = new StringBuilder();
			foreach (ValidationError error in ves)
				sb.AppendLine(error.ErrorMessage);
			return sb.ToString();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.GenClassLib.Validation {
	/// <summary>
	/// Summary description for mmpireDomainException.
	/// </summary>
	public class ValidationException : Exception {
		private readonly string msgString;
		private ICollection<ValidationError> validationErrors = new List<ValidationError>();

		public ValidationException() {}

		public ValidationException(string msg) : base(msg) {}

		public ValidationException(string msg, ICollection<ValidationError> errors) : base(msg) {
			ValidationErrors = errors;
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(msg);
			foreach (ValidationError error in errors) {
				Data.Add(error.ErrorNumber, error.ErrorMessage);
				sb.AppendLine(error.ErrorMessage);
			}
			msgString = sb.ToString();
		}

		public ValidationException(ICollection<ValidationError> errors)
			: this("More than one validaton error occurred.", errors) {
			ValidationErrors = errors;
		}

		public ValidationException(string msg, ValidationError error) : base(msg) {
			ValidationErrors.Add(error);
		}

		public ValidationException(ValidationError error) : base(error.ErrorMessage) {
			ValidationErrors.Add(error);
		}

		public ICollection<ValidationError> ValidationErrors {
			get { return validationErrors; }
			private set { validationErrors = value; }
		}

		public override string Message {
			get { return msgString; }
		}
	}
}
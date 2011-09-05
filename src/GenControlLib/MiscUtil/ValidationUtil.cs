using System.Collections.Generic;
using MindHarbor.GenClassLib.Validation;

namespace MindHarbor.GenControlLib.MiscUtil {
	public class ValidationUtil {
		public static string BuildErrorMessage(string info, ICollection<ValidationError> ves) {
			string errMsg = info + ": <br /><ul>";
			foreach (ValidationError ve in ves)
				errMsg += "<li>" + ve.ErrorMessage + "</li>";

			errMsg += "</ul>";
			return errMsg;
		}
	}
}
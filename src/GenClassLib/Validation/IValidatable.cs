using System.Collections.Generic;

namespace MindHarbor.GenClassLib.Validation {
	/// <summary>
	/// Summary description for IValidatable.
	/// </summary>
	public interface IValidatable {
		/// <summary>
		/// 
		/// </summary>
		/// <returns>an collection of <see cref="ValidationError"/>
		/// if the validation passes, an empty collection will be returned. 
		/// </returns>
		ICollection<ValidationError> Validate();
	}
}
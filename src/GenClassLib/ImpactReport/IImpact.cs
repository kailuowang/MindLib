using Iesi.Collections.Generic;

namespace MindHarbor.GenClassLib.ImpactReport {
	/// <summary>
	/// reprents an impact that will be caused by a deletion
	/// </summary>
	public interface IImpact {
		/// <summary>
		/// Gets the <see cref="string"/> message
		/// </summary>
		string Message { get; }

		/// <summary>
		/// Sub Impact Items
		/// </summary>
		ISet<IImpact> SubItems { get; }
	}
}
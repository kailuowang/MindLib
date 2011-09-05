namespace MindHarbor.GenClassLib.ImpactReport {
	/// <summary>
	/// An interface for entities that has deletion impact and can generate furthur ImpactReport
	/// </summary>
	public interface IHasDeletionImpact {
		/// <summary>
		/// Generate the <see cref="EntityDeletionImpact"/> of the deletion of this entity
		/// </summary>
		EntityDeletionImpact ReportImpact();
	}
}
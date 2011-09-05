namespace MindHarbor.GenClassLib.ImpactReport {
	/// <summary>
	///  impact that delete other entity
	/// </summary>
	public class EntityDeletionImpact : ImpactBase {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public EntityDeletionImpact(string msg) : base(msg) {}

		/// <summary>
		/// Generate EntityDeletionImpact with a simple deletion impact message built upon <paramref name="entityName"/> and <paramref name="entityType"/>
		/// </summary>
		/// <param name="entityType"></param>
		/// <param name="entityName"></param>
		/// <returns></returns>
		/// <remarks>a simple helper creation method</remarks>
		public static EntityDeletionImpact CreateWithSimpleMessage(string entityType, string entityName) {
			return new EntityDeletionImpact(entityType + "- \"" + entityName + "\" will be deleted.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="associationName"></param>
		/// <returns></returns>
		public AssociationImpact NewAssociationSubItem(string associationName) {
			AssociationImpact retVal = new AssociationImpact(associationName);
			subItems.Add(retVal);
			return retVal;
		}
	}
}
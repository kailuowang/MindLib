namespace MindHarbor.GenClassLib.ImpactReport {
	/// <summary>
	/// Impact to a association of the to-be-deleted
	/// </summary>
	public class AssociationImpact : ImpactBase {
		internal AssociationImpact(string msg) : base(msg) {}

		/// <summary>
		/// Create a Impact for an entity in this assocation that is going to be updated
		/// </summary>  
		/// <param name="msg"></param>
		/// <returns></returns>
		public IImpact NewEntityUpdateSubItem(string msg) {
			EntityUpdateImpact retVal = new EntityUpdateImpact(msg);
			subItems.Add(retVal);
			return retVal;
		}

		/// <summary>
		/// Create a Impact for an entity in this assocation that is going to be deleted
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public IImpact NewEntityDeletionSubItem(string msg) {
			ImpactBase retVal = new EntityDeletionImpact(msg);
			subItems.Add(retVal);
			return retVal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public IImpact NewEntityDeletionSubItem(IHasDeletionImpact entity) {
			ImpactBase retVal = entity.ReportImpact();
			subItems.Add(retVal);
			return retVal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public IImpact NewEntityDeletionSubItem(object entity) {
			return NewEntityDeletionSubItem(entity.ToString() + " will be deleted.");
		}
	}
}
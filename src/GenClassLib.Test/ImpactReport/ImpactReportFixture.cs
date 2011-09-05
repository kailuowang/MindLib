using System.Collections.Generic;
using MindHarbor.GenClassLib.ImpactReport;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.ImpactReport {
	[TestFixture]
	public class ImpactReportFixture {
		[Test]
		public void GenerateReportTest() {
			MockImpacter1 mi1 = new MockImpacter1();
			EntityDeletionImpact edi = mi1.ReportImpact();
			Assert.AreEqual(1, edi.SubItems.Count);
			foreach (IImpact impact in edi.SubItems) {
				Assert.IsInstanceOfType(typeof (AssociationImpact), impact);
				Assert.AreEqual(2, impact.SubItems.Count);
				foreach (IImpact ssub in impact.SubItems) {
					Assert.IsInstanceOfType(typeof (EntityDeletionImpact), ssub);
					Assert.AreEqual(1, ssub.SubItems.Count);
					foreach (IImpact sssub in ssub.SubItems) {
						Assert.IsInstanceOfType(typeof (AssociationImpact), sssub);
						Assert.AreEqual(1, sssub.SubItems.Count);
						foreach (IImpact ssssub in sssub.SubItems) {
							Assert.IsInstanceOfType(typeof (EntityUpdateImpact), ssssub);
							Assert.AreEqual(0, ssssub.SubItems.Count);
						}
					}
				}
			}
		}
	}

	public class MockImpacter1 : IHasDeletionImpact {
		private ICollection<MockImpacter2> mockAssociation = new List<MockImpacter2>();

		public MockImpacter1() {
			mockAssociation.Add(new MockImpacter2());
			mockAssociation.Add(new MockImpacter2());
		}

		#region IHasDeletionImpact Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EntityDeletionImpact ReportImpact() {
			EntityDeletionImpact retVal = new EntityDeletionImpact("MockImpact1");
			AssociationImpact ai = retVal.NewAssociationSubItem("Association to MockImpacter2 from MockImpacter1");
			foreach (MockImpacter2 mi2 in mockAssociation) ai.NewEntityDeletionSubItem(mi2);
			return retVal;
		}

		#endregion
	}

	public class MockImpacter2 : IHasDeletionImpact {
		#region IHasDeletionImpact Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EntityDeletionImpact ReportImpact() {
			EntityDeletionImpact retVal = new EntityDeletionImpact("MockImpact2");
			retVal.NewAssociationSubItem("association in MockImpact2")
				.NewEntityUpdateSubItem("some entity is updated");
			return retVal;
		}

		#endregion
	}
}
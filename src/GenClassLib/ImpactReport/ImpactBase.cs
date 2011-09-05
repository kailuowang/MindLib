using Iesi.Collections.Generic;

namespace MindHarbor.GenClassLib.ImpactReport {
	/// <summary>
	/// reprents an impact caused by the deletion
	/// </summary>
	public abstract class ImpactBase : IImpact {
		private string message;

		/// <summary>
		/// 
		/// </summary>
		protected ISet<IImpact> subItems = new HashedSet<IImpact>();

		protected ImpactBase(string message) {
			Message = message;
		}

		#region IImpact Members

		/// <summary>
		/// Gets the <see cref="string"/> message
		/// </summary>
		public string Message {
			get { return message; }
			private set { message = value; }
		}

		/// <summary>
		/// Sub Impacts
		/// </summary>
		public ISet<IImpact> SubItems {
			get { return new ImmutableSet<IImpact>(subItems); }
		}

		#endregion
	}
}
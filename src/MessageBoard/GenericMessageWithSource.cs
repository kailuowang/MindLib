namespace MindHarbor.MessageBoard {
	public class GenericMessageWithSource<SourcT> : IMessageWithSource {
		private SourcT source;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public GenericMessageWithSource(SourcT src) {
			Source = src;
		}

		///<summary>
		/// The source object associated with this message
		///</summary>
		public SourcT Source {
			get { return source; }
			private set { source = value; }
		}

		#region IMessageWithSource Members

		object IMessageWithSource.Source {
			get { return Source; }
		}

		#endregion
	}
}
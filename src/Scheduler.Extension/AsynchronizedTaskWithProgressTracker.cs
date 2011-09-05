using System;
using MindHarbor.GenClassLib.ProgressTracking;
using NHibernate.Burrow;

namespace MindHarbor.Scheduler.Extension {
	public abstract class AsynchronizedTaskWithProgressTracker : AsynchronizedProcessTask {
		protected ProgressTracker pt = new ProgressTracker();
		protected AsynchronizedTaskWithProgressTracker(string name) : base(name) {}

		public override float PercentageProgress {
			get { return pt.Progress*100; }
		}

		protected override void Perform() {
			pt = new ProgressTracker();
			BurrowFramework bf = new BurrowFramework();
			bf.InitWorkSpace();
		    try {
		        pt.ProgressChanged += new EventHandler(pt_ProgressChanged);
		        DoPerform();
		    }catch(Exception) {
				try {
					if (bf.CurrentConversation != null)
						bf.CurrentConversation.GiveUp();
				}catch (Exception) {/*prevent this exception hiding the real one*/}
				throw;
		    }
		    finally {
				try {
					bf.CloseWorkSpace();
				}
				catch (Exception) {/*prevent this exception hiding the real one*/}
		    }
		}

		private void pt_ProgressChanged(object sender, EventArgs e) {
			if (e is ProgressChangedArgs)
			    AddLog(((ProgressChangedArgs) e).Message + " " + pt.Progress.ToString("p"));// + " Est. Remaining " + pt.Remainning);
		}

		protected abstract void DoPerform();
	}
}
using System;
using MindHarbor.GenClassLib.ProgressTracking;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.ProgressTests {
	[TestFixture]
	public class ProgressTrackingFixture {
		private float lastProgress;

		private void pt_ProgressChanged(object sender, EventArgs e) {
			float currentProgress = ((ProgressTracker) sender).Progress;
			float error = Math.Abs((float) 1/24 - (currentProgress - lastProgress));
			Assert.Less(error, 0.00001);
			Console.WriteLine(currentProgress);
			lastProgress = currentProgress;
		}

		[Test]
		public void Test() {
			ProgressTracker pt = new ProgressTracker();
			TestProcess tp = new TestProcess();
			pt.ProgressChanged += new EventHandler(pt_ProgressChanged);
			tp.Process(pt, 4);
		}
	}

	public class TestProcess {
		public void Process(ProgressTracker progressTracker, int maximumNumOfSubs) {
			if (maximumNumOfSubs > 0)
				for (int i = 0; i < maximumNumOfSubs; i++)
					Process(progressTracker.CreateSubTracker(maximumNumOfSubs), maximumNumOfSubs - 1);
		}
	}
}
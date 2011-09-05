using System;

namespace MindHarbor.GenClassLib.ProgressTracking {
	///<summary>
	/// A class that helps tracking progress of an operation that overspan multiple classes
	///</summary>
	public class ProgressTracker {
		private int currentSubProcIdx = -1;
		private int numOfSubProcedure = 0;
		private float progress = 0;
		private string progressStatus;
		private ProgressTracker subTracker;
		private readonly DateTime start = DateTime.Now;
		private DateTime lastCheck;
		private long estimateBasedOnLastPeriod;
		private float lastProgress;
		public event EventHandler ProgressChanged;
		public TimeSpan TimeSpan
		{
			get
			{
				return DateTime.Now - start;
			}
		}

		public DateTime Start
		{
			get { return start; }
		}

		public TimeSpan Remainning
		{
			get
			{
				float p = Progress;

				if (lastCheck < Start)
				{
					lastCheck = Start;
					estimateBasedOnLastPeriod = 0;
				}
				TimeSpan ts = TimeSpan;
				long estimateBasedOnTotal = (long)(ts.Ticks / p);
				if (estimateBasedOnLastPeriod == 0)
					estimateBasedOnLastPeriod = estimateBasedOnTotal;
				if (lastProgress != p && (DateTime.Now - lastCheck).TotalSeconds > 200)
				{
					float progressImprove = p - lastProgress;
					estimateBasedOnLastPeriod = (long)((DateTime.Now - lastCheck).Ticks / progressImprove);
					lastProgress = p;
					lastCheck = DateTime.Now;
				}

				long lastTotalEstimate = (estimateBasedOnLastPeriod + estimateBasedOnTotal) / 2;
				return new TimeSpan(lastTotalEstimate - ts.Ticks);
			}
		}
		///<summary>
		/// string description of the current progress
		///</summary>
		public string ProgressStatus {
			get { return progressStatus; }
			set {
				progressStatus = value;
				if (ProgressChanged != null)
					ProgressChanged(this, new ProgressChangedArgs(value));
			}
		}

		/// <summary>
		/// Gets the progress of this tracker 
		/// </summary>
		/// <remarks>
		/// from 0 to 1;
		/// </remarks>
		public float Progress {
			get { return progress + (1 - progress)*SubProceduresProgress(); }
			set {
				if (value > 1 || value < 0)
					throw new ArgumentException("progress must be between 0 and 1");
				bool changed = progress != value;
				progress = value;
				if (changed)
					FireProgressChangedEvt();
			}
		}

		
		private float SubProceduresProgress() {
			if (numOfSubProcedure <= 0)
				return 0;
			float finished = (float) (currentSubProcIdx)/numOfSubProcedure;
			if (finished < 0) finished = 0;
			float subProgress = (subTracker != null ? subTracker.Progress : 0)/numOfSubProcedure;
			float retVal = finished + subProgress;
			if (retVal > 1)
				retVal = 1;
			return retVal;
		}

		private void FireProgressChangedEvt() {
			if (ProgressChanged != null)
				ProgressChanged(this, new EventArgs());
		}

		public static ProgressTracker CreateSubTracker(ProgressTracker currentTracker, int numberOfSubProc) {
			if (currentTracker != null)
				return currentTracker.CreateSubTracker(numberOfSubProc);
			else
				return null;
		}

		public ProgressTracker CreateSubTracker(int numberOfSubProc) {
			return CreateSubTracker(numberOfSubProc, currentSubProcIdx + 1);
		}

        /// <summary>
        /// Create a subtracker that takes up multiple steps
        /// </summary>
        /// <param name="totalNumberOfSteps">how many steps in total this process will have</param>
        /// <param name="numberOfStepsInTheSubTracker">how many steps this creating sub tracker will take</param>
        /// <returns></returns>
        /// <remarks>
        /// <example>
        /// CreateSubTrackerOfSteps(10, 4) means that you are currently in a sub process that takes up the 4 steps out of the 10 steps
        /// </example>
        /// </remarks>
        public ProgressTracker CreateSubTrackerOfSteps(int totalNumberOfSteps, int numberOfStepsInTheSubTracker)
        {
            return CreateSubTracker(totalNumberOfSteps, currentSubProcIdx + numberOfStepsInTheSubTracker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalNumberOfSteps"></param>
        /// <param name="thisSubProcFinishingStep">the finishing number of Step once this current sub process finishes</param>
        /// <returns></returns>
		public ProgressTracker CreateSubTracker(int totalNumberOfSteps, int thisSubProcFinishingStep) {
			numOfSubProcedure = totalNumberOfSteps;
			if (numOfSubProcedure <= 0)
				throw new Exception("Cannot enter sub procedure if tracker created with 0 number of sub procedure");
			subTracker = new ProgressTracker();
			subTracker.ProgressChanged += new EventHandler(subTracker_ProgressChanged);
			currentSubProcIdx = thisSubProcFinishingStep;
			if (currentSubProcIdx > 0)
				FireProgressChangedEvt();
			return subTracker;
		}

		private void subTracker_ProgressChanged(object sender, EventArgs e) {
			if (ProgressChanged != null)
				ProgressChanged(this, e);
		}
	}
}
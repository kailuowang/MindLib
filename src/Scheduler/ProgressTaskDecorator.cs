using System;
using System.Collections.Generic;

namespace MindHarbor.Scheduler {
	/// <summary>
	/// A decorator that adds more functions to the ITaskWithProgressInfo
	/// </summary>
	public class ProgressTaskDecorator : ITaskWithProgressInfo {
		private ITaskWithProgressInfo innerTask;
		private float lastProgress = 0;
		DateTime lastCheck = DateTime.Now ;
		private long estimateBasedOnLastPeriod;
		#region ITaskWithProgressInfo

		public float PercentageProgress {
			get { return innerTask.PercentageProgress; }
		}

		public TaskLogEntry[] Logs {
			get { return innerTask.Logs; }
		}

		public string LatestLogMessage {
			get { return innerTask.LatestLogMessage; }
		}

		public string Name {
			get { return innerTask.Name; }
		}

		public TaskStatus Status {
			get { return innerTask.Status; }
		}

		public DateTime? LastInvokedTime {
			get { return innerTask.LastInvokedTime; }
		}

		public DateTime NextInvokeTime {
			get { return innerTask.NextInvokeTime; }
		}

		public DateTime StartTime {
			get { return innerTask.StartTime; }
		}

		public int LogSize {
			get { return innerTask.LogSize; }
			set { innerTask.LogSize = value; }
		}

		public Schedule Schedule {
			get { return innerTask.Schedule; }
		}

		public ScheduleType ScheduleType {
			get { return innerTask.ScheduleType; }
		}

		public bool IsOutDated {
			get { return innerTask.IsOutDated; }
		}

		public TaskLogEntry[] ErrLogs {
			get { return innerTask.ErrLogs; }
		}

		public IList<string> ErrorAlertEmails {
			get { return innerTask.ErrorAlertEmails; }
			set { innerTask.ErrorAlertEmails = value; }
		}

		public string ErrorAlertFrom {
			get { return innerTask.ErrorAlertFrom; }
			set { innerTask.ErrorAlertFrom = value; }
		}

		public event TaskLogEventHandler LogAdded {
			add { innerTask.LogAdded += value; }
			remove { innerTask.LogAdded -= value; }
		}

		public event EventHandler PostPerformed {
			add { innerTask.PostPerformed += value; }
			remove { innerTask.PostPerformed -= value; }
		}

		public event EventHandler ExceptionOccured {
			add { innerTask.ExceptionOccured += value; }
			remove { innerTask.ExceptionOccured -= value; }
		}

		#endregion

		public ProgressTaskDecorator(ITaskWithProgressInfo innerTask) {
			this.innerTask = innerTask;
			
		}

		public ITaskWithProgressInfo InnerTask {
			get { return innerTask; }
			set { innerTask = value; }
		}

		public TimeSpan? TimeSpan {
			get {
				if (LastInvokedTime != null)
					return DateTime.Now - LastInvokedTime.Value;
				return null;
			}
		}

		public DateTime? StartAt {
			get { return LastInvokedTime.Value; }
		}

		public TimeSpan? Remainning {
			get {
				float progress = PercentageProgress;
				if (Status != TaskStatus.Running || progress == 0) return null;
			
				if(lastCheck < StartAt.Value) {
					lastCheck = StartAt.Value;
					estimateBasedOnLastPeriod = 0;
				}
				TimeSpan ts = TimeSpan.Value;
				long estimateBasedOnTotal = (long) (ts.Ticks*100f/progress);
				if(estimateBasedOnLastPeriod == 0)
					estimateBasedOnLastPeriod = estimateBasedOnTotal;
				if (lastProgress != progress && (DateTime.Now - lastCheck).TotalSeconds > 200) {
					float progressImprove = progress - lastProgress;
					estimateBasedOnLastPeriod = (long)((DateTime.Now - lastCheck).Ticks * 100f / progressImprove);
					lastProgress = progress;
					lastCheck = DateTime.Now;
				}
				
				long lastTotalEstimate = (estimateBasedOnLastPeriod + estimateBasedOnTotal)/2;
				return new TimeSpan(lastTotalEstimate - ts.Ticks);
			}
		}

		public bool Interrutable {
			get { return innerTask is IInterruptable; }
		}

		public void Interrupt() {
			if (!Interrutable)
				throw new NotSupportedException("NonInterupptable cannot be interrupted.");
			else
				((IInterruptable) innerTask).Interrupt();
		}
	}
}
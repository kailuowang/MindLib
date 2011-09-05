using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using MindHarbor.Scheduler;

public partial class Controls_AsynchronizedProcess : UserControl {
	public string Title {
		get {
			if (ViewState["Title"] == null)
				ViewState["Title"] = string.Empty;
			return (string) ViewState["Title"];
		}
		set { ViewState["Title"] = value; }
	}

	private ProgressTaskDecorator CurrentTask {
		get {
			if (ViewState["CurrentTaskName"] != null)
				return (ProgressTaskDecorator) TaskManager.GetTaskByName((string) ViewState["CurrentTaskName"]);
			return null;
		}
		set {
			if (value == null)
				ViewState["CurrentTaskName"] = null;
			else
				ViewState["CurrentTaskName"] = value.Name;
		}
	}

	public bool AutoClose {
		get {
			if (ViewState["AutoClose"] == null)
				ViewState["AutoClose"] = false;
			return (bool) ViewState["AutoClose"];
		}
		set { ViewState["AutoClose"] = value; }
	}

	private bool IsRunning {
		get { return CurrentTask != null && CurrentTask.Status != TaskStatus.Finished; }
	}

	public event EventHandler ErrorOccurred;
	public event EventHandler Finished;
	public event EventHandler Cancelled;

	protected void Page_Load(object sender, EventArgs e) {}

	public void Process(ITaskWithProgressInfo task) {
		Process(new ProgressTaskDecorator(task));
	}

	public void Process(ProgressTaskDecorator task) {

		ITask t = FindByName(task.Name);
		if(t == null){
			TaskManager.AddTask(task, true);
			CurrentTask = task;
			OnTaskLoad();
		}else {
			CurrentTask = (ProgressTaskDecorator) t;
			MsgTaskExists.Confirm("!",
				"A task with the same name \"" + task.Name 
					+ "\" is still running. This task cannot be added. Do you want to load that task instead?");
		}
	}

	private ITask FindByName(string name) {
		foreach (ITask t in TaskManager.Tasks)
			if (t.Name == name)
				return t;
		return null;
	}

	private void OnTaskLoad() {
		Show();
		Timer1.Enabled = true;
		lTitle.Text = Title;
		lTitle.ForeColor = Color.Black;
		Refresh();
	}

	private void Refresh() {
		if (!divPopupProcessor.Visible) return;

		bool runningStatusChanged = phComplete.Visible == IsRunning;
		phProgressBar.Visible = IsRunning;
		phCancel.Visible = IsRunning && CurrentTask.Interrutable;
		phComplete.Visible = !IsRunning;

		if (runningStatusChanged)
			upTopBar.Update();
	}

	protected void Timer1_Tick(object sender, EventArgs e) {
		UpdateProgress();
		if (!IsRunning)
			Timer1.Enabled = false;
	}

	private void UpdateProgress() {
		if (CurrentTask == null) {
			lTitle.Text += "ERROR! Task was removed by error.";
			lTitle.ForeColor = Color.Red;
			if (ErrorOccurred != null)
				ErrorOccurred(this, new EventArgs());
		}
		else {
			PercentageBar1.Bind(CurrentTask.PercentageProgress);
			BuildLogMessage();
			BuildTimeInfo();
			if (CurrentTask.Status == TaskStatus.Finished) {
				TaskManager.RemoveTask(CurrentTask);
				if (AutoClose)
					btnFinish_Click(this, null);
			}

			upProgress.Update();
			if (cbAutoRefreshLogs.Checked)
				upLogs.Update();
		}
	}

	private void BuildTimeInfo() {
		lStart.Text = CurrentTask.StartAt.Value.ToString("MMM. dd, hh:mm");
		lTimeSpan.Text = FormatTimeSpanString(CurrentTask.TimeSpan);
		lRemainning.Text = FormatTimeSpanString(CurrentTask.Remainning);
		phRemaining.Visible = CurrentTask.Status == TaskStatus.Running;
	}

	private static string FormatTimeSpanString(TimeSpan? ts) {
		if (ts == null)
			return "Unknown";
		string timeSpanString = ts.ToString();
		int idx = timeSpanString.LastIndexOf(".");
		if (idx <= 0) return timeSpanString;
		return timeSpanString.Substring(0, idx);
	}

	private void BuildLogMessage() {
		StringBuilder sb = new StringBuilder();
		sb.Append("<ul>");
		foreach (TaskLogEntry log in CurrentTask.Logs)
			sb.Append(string.Format(" <li>{0} - {1}</li>",
			                        log.CreationTime.ToLongTimeString(),
			                        log.Msg));
		sb.Append("</ul>");
		sb.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("  ", " &nbsp;");
		tbLogMessage.Text = sb.ToString();
	}

	protected override void OnPreRender(EventArgs e) {
		Refresh();
	}

	protected void btnCancel_Click(object sender, EventArgs e) {
		CurrentTask.Interrupt();
		TaskManager.RemoveTask(CurrentTask);
		Hide();
		if (Cancelled != null)
			Cancelled(this, null);
	}

	private void Show() {
		upMain.Update();
		upProgress.Update();
		divInnerProcessor.Visible = true;
		mpeProcessor.Show();
	}

	private void Hide() {
		CurrentTask = null;
		mpeProcessor.Hide();
		upMain.Update();
		divInnerProcessor.Visible = false;
	}

	protected void btnFinish_Click(object sender, EventArgs e) {
		Hide();
		if (Finished != null)
			Finished(this, null);
	}

 

	protected void MsgTaskExistsConfirmed(object sender, EventArgs e) {
		OnTaskLoad();
	}
}
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MindHarbor.Scheduler;

public partial class Controls_TaskManagerUC : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
        Page.Title = Scheduler.Name;
        lInterceptor.Text = Scheduler.InterceptorTypeName;
        lCurrentTime.Text = DateTime.Now.ToString();
    }

    protected override void OnPreRender(EventArgs e)
    {
        lbShutDown.Visible = !TaskManager.IsShutDown;
        lbErrMsg.Text = TaskManager.IsShutDown ? "Task Manager is shut down" : string.Empty;
        lbReloadTasks.Visible = !TaskManager.IsShutDown;
        base.OnPreRender(e);
    }

    private void BindGrid()
    {
        gvTasks.DataSource = TaskManager.Tasks;
        gvTasks.DataBind();
    }

    protected void RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ITask t = TaskManager.GetTaskByName(e.CommandArgument.ToString());
        if (e.CommandName == "Stop")
            TaskManager.StopTask(t);
        else if (e.CommandName == "Start")
            TaskManager.StartTask(t);
        else if (e.CommandName == "ErrorLog")
            TaskLogViewer1.Bind(t.ErrLogs);
        else if (e.CommandName == "Log")
            TaskLogViewer1.Bind(t.Logs);
        BindGrid();
    }

    protected void RowBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton lbStart = e.Row.FindControl("lbStart") as LinkButton;
        if (lbStart == null) return;
        ITask task = GetTask(e.Row);
        bool running = task.Status != TaskStatus.Finished;

        lbStart.Visible = !running;
        lbStart.CommandArgument = task.Name;
        LinkButton lbStop = e.Row.FindControl("lbStop") as LinkButton;
        lbStop.Visible = running;
        lbStop.CommandArgument = task.Name;

        LinkButton lblog = e.Row.FindControl("lbLog") as LinkButton;
        LinkButton lbErrlog = e.Row.FindControl("lbErrLog") as LinkButton;
        lblog.CommandArgument = task.Name;
        lbErrlog.CommandArgument = task.Name;
    }

    private ITask GetTask(GridViewRow row)
    {
        string name = gvTasks.DataKeys[row.RowIndex]["Name"].ToString();
        return TaskManager.GetTaskByName(name);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (DateTime.Now.Second%5 == 0)
            BindGrid();
    }

    protected void lbShutDown_Click(object sender, EventArgs e)
    {
        lbShutDown.Visible = false;
        TaskManager.ShutDown();
        BindGrid();
    }

    protected void lbReloadTasks_Click(object sender, EventArgs e)
    {
        TaskManager.ReloadTasks();
    }
}
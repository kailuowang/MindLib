using System;
using System.Web.UI;
using MindHarbor.Scheduler;

public partial class Scheduler_TaskLogViewer : UserControl
{
    public int Left
    {
        get
        {
            if (ViewState["Left"] == null)
                ViewState["Left"] = 50;
            return (int) ViewState["Left"];
        }
        set { ViewState["Left"] = value; }
    }

    public int Top
    {
        get
        {
            if (ViewState["Top"] == null)
                ViewState["Top"] = 300;
            return (int) ViewState["Top"];
        }
        set { ViewState["Top"] = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnInit(e);
        pnlMain.Style.Add("left", Left + "px");
        pnlMain.Style.Add("Top", Top + "px");
    }

    public void Show()
    {
        upMain.Update();
        phMain.Visible = true;
    }

    public void Hide()
    {
        upMain.Update();
        phMain.Visible = false;
    }

    protected void lbHide_Click(object sender, EventArgs e)
    {
        Hide();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    public void Bind(TaskLogEntry[] logs)
    {
        rptLogs.DataSource = logs;
        rptLogs.DataBind();
        lNoLogs.Visible = rptLogs.Items.Count == 0;
        Show();
    }
}
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using MindHarbor.GenClassLib.Validation;
using NHibernate.Burrow.WebUtil.Attributes;
using PMA.Domain.Event;

public partial  class GeneralControl_Msg : ControlBase, IMessageBox {
    public event EventHandler Confirmed;
    public event EventHandler Cancelled;
	public event EventHandler<EventArg<string>> ConfirmedCommand;
     
    #region public properties

    [StatefulField] protected string  callerId;
    [StatefulField] protected string  commandName;

    public string  CallerId {
        get { return callerId; }
        private set { callerId = value; }
    }
   
    public bool ShowCancelButton {
        get {
            if (ViewState["ShowCancelButton"] == null)
                ViewState["ShowCancelButton"] = true;
            return (bool) ViewState["ShowCancelButton"];
        }
        set { ViewState["ShowCancelButton"] = value; }
    }

    public string OkButtonText {
        get {
            if (ViewState["OkButtonText"] == null)
                ViewState["OkButtonText"] = "  Ok  ";
            return (string) ViewState["OkButtonText"];
        }
        set { ViewState["OkButtonText"] = value; }
    }

    public string CancelButtonText {
        get {
            if (ViewState["CancelButtonText"] == null)
                ViewState["CancelButtonText"] = "Cancel";
            return (string) ViewState["CancelButtonText"];
        }
        set { ViewState["CancelButtonText"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {}

    public string Title {
        set { ltTitle.Text = value; }
    }

    public string Message {
        set { ltMsgInfo.Text = value; }
    }

    

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e) {
       
        Hide();
        if (Cancelled != null) Cancelled(this, null);
		
	}

    protected void btnOk_Click(object sender, EventArgs e) {
    	string command = commandName;
		Hide();
		if(string.IsNullOrEmpty(command)) {
			if (Confirmed != null) Confirmed(this, null);
		}
		else {
			if(ConfirmedCommand != null )ConfirmedCommand (this, new EventArg<string>(command));
		}
	}

    protected override void OnPreRender(EventArgs e) {
        btnCancel.Visible = ShowCancelButton;
        btnCancel.Text = CancelButtonText;
       
        btnOk.Text = OkButtonText;
        base.OnPreRender(e);
    }

    public void Confirm(string title, string info) {
        Confirm(title, info, null);
    }

    public void Confirm(string title, string info, Control caller) {
        CallerId = caller != null ? caller.ID : null;
        Show(title, info, true);
    }

	public void ConfirmCommand(string title, string info, string command)
	{
		commandName = command;
		Show(title, info, true);
	}

    public void Alert(string title, string info) {
        Show(title, info, false);
    }

    public void ErrorMsg(string title, string info, ICollection<ValidationError> ves)
    {
        string errMsg = BuildErrorMessage(info, ves);
        Alert(title, errMsg);
    }

    private void Show(string title, string info, bool showCancelButton) {
        Title = title;
        Message = info;
        ShowCancelButton = showCancelButton;
        Show();
    }
    
    
    private void Show() {
        upMain.Update();
        upInner.Update();
        divInner.Visible = true;
        
        ModalPopupExtender1.Show();
    }
    
    private void Hide() {
    	commandName = null;
        ModalPopupExtender1.Hide();
        upMain.Update();
        divInner.Visible = false;
    }
}
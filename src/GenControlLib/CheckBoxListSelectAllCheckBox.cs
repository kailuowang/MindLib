using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace MindHarbor.GenControlLib {
	/// <summary>
	///		Summary description for CheckBoxListSelectAll.
	/// </summary>
	public class CheckBoxListSelectAllCheckBox : CheckBox {
		private CheckBoxList checkBoxList;

		[Bindable(true),
		 Category("Behavior"),
		 DefaultValue("")]
		public string CheckBoxListName {
			get { return (string) ViewState["CheckBoxListName"]; }
			set { ViewState["CheckBoxListName"] = value; }
		}

		/// <summary>
		/// this property is NOT stored in the ViewState
		/// </summary>
		public CheckBoxList CheckBoxList {
			get { return checkBoxList; }
			set { checkBoxList = value; }
		}

		private void Page_Load(object sender, EventArgs e) {
			AutoPostBack = false;
			string script =
				@"<script language='javascript'>    
							  function Checkall(tabnm,chknm){
									var strname=tabnm;
									var strchknm=chknm;
									// Go through all items of a check list control
									var table = document.getElementById (strname);
									var cells = table.getElementsByTagName('td');
									var ctlr;
									for (var i = 0; i < cells.length; i++){
									ctrl = cells[i].firstChild;
									if (ctrl.type == 'checkbox')
										if(document.getElementById(strchknm).checked==true){
											ctrl.checked=true;                  
										}
											else{ctrl.checked=false;}
										}
									}
								</script>

		   ";

			Page.ClientScript.RegisterClientScriptBlock(typeof (string), "SelectAll", script);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);
			if (CheckBoxList == null)
				CheckBoxList = (CheckBoxList) Page.FindControl(CheckBoxListName);
			if (CheckBoxList == null)
				throw new Exception("Failed to fine the CheckBox control");
			Attributes.Add("onclick", "Checkall('" + CheckBoxList.ClientID + "','" + ClientID + "')");
		}

		#region Web Form Designer generated code

		protected override void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion
	}
}
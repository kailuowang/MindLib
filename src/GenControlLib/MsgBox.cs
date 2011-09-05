using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MindHarbor.GenClassLib.Data;

namespace MindHarbor.GenControlLib {
	/// <summary>
	/// Summary description for WebCustomControl1.
	/// </summary>
	[DefaultProperty("Text"),
	 ToolboxData("<{0}:msgBox runat=server></{0}:msgBox>")]
	public class MsgBox : WebControl {
		//private string msg;
		private string content;

		public string HiddenFieldName {
			get {
				if (ViewState["hiddenFieldName"] == null) {
					RandomStringGenerator rsg = new RandomStringGenerator();
					rsg.IsUsingNumeric = false;
					rsg.IsUsingSpecialChars = false;

					ViewState["hiddenFieldName"] = rsg.Generate(5);
				}
				return (string) ViewState["hiddenFieldName"];
			}
			set { ViewState["hiddenFieldName"] = value; }
		}

		public event EventHandler Confirmed;
		public event EventHandler Canceled;

		[Bindable(true),
		 Category("Appearance"),
		 DefaultValue("")]
		public void alert(string msg) {
			string sMsg = msg.Replace("\n", "\\n");
			sMsg = msg.Replace("\"", "'");

			StringBuilder sb = new StringBuilder();
			sb.Append(@"<INPUT type=hidden value='0' name='" + HiddenFieldName + "'>");

			sb.Append(@"<script language='javascript'>");

			sb.Append("alert( \" " + sMsg + " \" );");

			sb.Append("document.forms[0]." + HiddenFieldName + ".value='1';" + "document.forms[0].submit(); ");

			sb.Append(@"</script>");

			content += sb.ToString();
		}

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
		}

		//good for the page with only one form
		public void confirm(string msg) {
			string sMsg = msg.Replace("\n", "\\n");
			sMsg = msg.Replace("\"", "'");

			StringBuilder sb = new StringBuilder();

			sb.Append(@"<INPUT type=hidden value='0' name='" + HiddenFieldName + "'>");

			sb.Append(@"<script language='javascript'>");

			sb.Append(" if(confirm( \" " + sMsg + " \" ))");
			sb.Append(@" { ");
			sb.Append("document.forms[0]." + HiddenFieldName + ".value='1';" + "document.forms[0].submit(); }");
			sb.Append(@" else { ");
			sb.Append("document.forms[0]." + HiddenFieldName + ".value='-1'; }");

			sb.Append(@"</script>");

			content = sb.ToString();
		}

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output) {
			output.Write(content);
		}

		private void Page_Load(object sender, EventArgs e) {
			if (Page.IsPostBack && Page.Request.Form[HiddenFieldName] != "")
				if (Page.Request.Form[HiddenFieldName] == "1") //if user clicks "OK" to confirm 
				{
					Page.Request.Form[HiddenFieldName].Replace("1", "0");
					//Reset the hidden field back to original value "0"
					if (Confirmed != null) Confirmed(this, new EventArgs());
				}
				else if (Page.Request.Form[HiddenFieldName] == "-1") {
					Page.Request.Form[HiddenFieldName].Replace("-1", "0");
					if (Canceled != null) Canceled(this, new EventArgs());
				}
		}

		#region Disabled

		//good for the page with multiple forms; NOT VERY NECESSARY
		/*public void confirm(string msg,string HiddenFieldName,string formname)
		{
			string  sMsg = msg.Replace( "\n", "\\n" );
			sMsg =  msg.Replace( "\"", "'" );
 
			StringBuilder sb = new StringBuilder();
			
			sb.Append( @"<INPUT type=hidden value='0' name='" + HiddenFieldName + "'>");

			sb.Append( @"<script language='javascript'>" );
			
			sb.Append( @" if(confirm( """ + sMsg + @""" ))" );
			sb.Append( @" { ");
			sb.Append( formname +"." + HiddenFieldName + ".value='1';" + formname + ".submit(); }" );
			sb.Append( @" else { ");
			sb.Append(formname +"." + HiddenFieldName + ".value='0'; }" );

			sb.Append( @"</script>" );

			content=sb.ToString();
		}*/

		#endregion
	}
}
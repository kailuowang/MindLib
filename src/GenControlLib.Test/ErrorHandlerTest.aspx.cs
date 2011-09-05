using System;
using System.Web.UI;

public partial class ErrorHandlerTest : Page {
	protected void Page_Load(object sender, EventArgs e) {
		//if (!IsPostBack)
		//    throw new Exception("Test Exception");
	}

	protected void Button1_Click(object sender, EventArgs e) {
		throw new NullReferenceException();
	}

	protected void Button2_Click(object sender, EventArgs e) {
		Call(null);
	}

	private void Call(string a) {
		throw new ArgumentNullException(a);
	}
}
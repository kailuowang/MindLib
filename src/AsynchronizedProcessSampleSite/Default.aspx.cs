using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void Button1_Click(object sender, EventArgs e){
        AsynchronizedProcess1.Process(new MockTask("A mock task"));
    }
    protected void Button2_Click(object sender, EventArgs e){
        AsynchronizedProcess1.Process(new MockTaskWithException( ));
    }
}
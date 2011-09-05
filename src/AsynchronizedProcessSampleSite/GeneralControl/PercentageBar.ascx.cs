using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class PercentageBar : UserControl
{
    public string percentage;
    public string leftPercentage;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    public void Bind(int per) {
        if (per < 0 || per > 100)
            throw new ArgumentOutOfRangeException("Percentage must be between 0 and 100");
      
      
        percentage = per.ToString();
        leftPercentage = (100 - per).ToString();
        imgSpacer.ToolTip = percentage;
    }
}

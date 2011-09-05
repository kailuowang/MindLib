using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using MindHarbor.GenInterfaces;

public partial class ControlTestPage : Page {
	protected void Page_Load(object sender, EventArgs e) {
		ddlstates.SetSelectedRegion("GA");
	    ICollection<CreditCardType> temp = new List<CreditCardType>();
        temp.Add(CreditCardType.AmericanExpress);
	    ddlCCT.SetHideEnumData(temp);
        ddlCCT.DataBind();
	}
}
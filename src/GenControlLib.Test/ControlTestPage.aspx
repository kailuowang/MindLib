<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ControlTestPage.aspx.cs" Inherits="ControlTestPage" %>

   <%@ Register Assembly="MindHarbor.GenControlLib" Namespace="MindHarbor.GenControlLib"   TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

            <cc1:ddlmonths id="ddlmonths" runat="server" cssclass="txtBox" ShowValueOrName=Values PlzSelectText="--"></cc1:ddlmonths>
              <cc1:ddlStates id="ddlstates" runat="server" cssclass="txtBox"></cc1:ddlStates>
          <cc1:DdlCreditCardType ID="ddlCCT" runat="server"></cc1:DdlCreditCardType>
    </div>
    </form>
</body>
</html>

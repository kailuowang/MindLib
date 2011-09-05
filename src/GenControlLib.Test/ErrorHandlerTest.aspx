<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorHandlerTest.aspx.cs" Inherits="ErrorHandlerTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<asp:button ID="Button1" runat="server" Text="Test Ignore Exceptions" OnClick="Button1_Click" /><br />
        <asp:button ID="Button2" runat="server" Text="Test Non-Ignore Exceptions" OnClick="Button2_Click" /></div>
    </form>
</body>
</html>

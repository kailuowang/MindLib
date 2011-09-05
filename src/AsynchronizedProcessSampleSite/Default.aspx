<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="Controls/AsynchronizedProcess.ascx" TagName="AsynchronizedProcess"
    TagPrefix="uc1" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> </title>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
              <asp:Button ID="Button1" runat="server" Text="Start" onclick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="Test exception" onclick="Button2_Click" />
        <uc1:AsynchronizedProcess id="AsynchronizedProcess1" runat="server"  Title="Mock Task for testing purpose" >
        </uc1:AsynchronizedProcess>
       </ContentTemplate>
</asp:UpdatePanel>

    </form>
</body>
</html>

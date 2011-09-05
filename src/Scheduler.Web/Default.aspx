<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="Controls/TaskManagerUC.ascx" TagName="TaskManagerUC" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MindHarbor Scheduler</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:TaskManagerUC ID="TaskManagerUC1" runat="server" />
    
    </div>
    </form>
</body>
</html>

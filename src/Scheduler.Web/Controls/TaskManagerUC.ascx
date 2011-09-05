<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskManagerUC.ascx.cs"
    Inherits="Controls_TaskManagerUC" %>
<%@ Register Src="TaskLogViewer.ascx" TagName="TaskLogViewer" TagPrefix="uc1" %>
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b>Interceptor:</b>
        <asp:Literal ID="lInterceptor" runat="server"></asp:Literal><br />
        <br />
        <asp:GridView ID="gvTasks" runat="server" DataKeyNames="Name" OnRowDataBound="RowBound"
            OnRowCommand="RowCommand" AutoGenerateColumns="false" CellPadding="4">
            <Columns><%--
             using this instead of BoundField because of a gridview bug --%>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Schedule Type">
                    <ItemTemplate>
                        <%# Eval("ScheduleType")%>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# Eval("Status")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last Invoke Time">
                    <ItemTemplate>
                        <%# Eval("LastInvokedTime")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Next Invoke Time">
                    <ItemTemplate>
                        <%# Eval("NextInvokeTime")%>
                    </ItemTemplate>
                </asp:TemplateField>
              <%--  <asp:BoundField DataField="ScheduleType" HeaderText="Schedule Type" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="LastInvokedTime" HeaderText="Last Invoked Time" />
                <asp:BoundField DataField="NextInvokeTime" HeaderText="Next Invoke Time" />--%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbStart" runat="server" CommandName="Start">Start</asp:LinkButton>
                        <asp:LinkButton ID="lbStop" runat="server" CommandName="Stop">Stop</asp:LinkButton>
                        <asp:LinkButton ID="lbErrLog" runat="server" CommandName="ErrorLog">View Error Log</asp:LinkButton>
                        <asp:LinkButton ID="lbLog" runat="server" CommandName="Log">View All Log</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle BackColor="#efefef" />
            <HeaderStyle BackColor="#EEEFFF" />
        </asp:GridView>
        <br />
        <br />
        <asp:LinkButton ID="lbReloadTasks" runat="server" OnClick="lbReloadTasks_Click">Reload Tasks</asp:LinkButton>
        <asp:LinkButton ID="lbShutDown" runat="server" OnClick="lbShutDown_Click" OnClientClick="return confirm('are you sure you want to shutdown the scheduler')">Shut Down</asp:LinkButton>
            <asp:Label ID="lCurrentTime" runat="server" Text="" Font-Bold="true"></asp:Label>
        <br />
        <asp:Label ID="lbErrMsg" runat="server" Text="" ForeColor="red"></asp:Label>
        <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" Enabled="true">
        </asp:Timer>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:TaskLogViewer ID="TaskLogViewer1" runat="server" />

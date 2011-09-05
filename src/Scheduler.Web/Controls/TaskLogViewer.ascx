<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskLogViewer.ascx.cs"
    Inherits="Scheduler_TaskLogViewer" %>
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <asp:PlaceHolder runat="server" ID="phMain" Visible="false">
            <asp:Panel runat="server" ID="pnlMain" Style="position: absolute; background: white;
                width: 800px">
                <table class="popupTable" width="100%" cellpadding="5">
                    <tbody>
                        <tr id="trLogViewerHeader">
                            <td class="formheader" align="left">
                                <h3 class="formtitle" style="width: 100%; display: inline">
                                    Logs
                                </h3>
                                &nbsp; &nbsp; <span style="font-size: x-small">click this banner to drag this panel
                                </span>
                            </td>
                            <td class="formheader" align="right" width="1%">
                                <asp:LinkButton ID="LinkButton1" OnClick="lbHide_Click" runat="server">X</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="width: 100%; height: 100%; overflow: auto">
                                    <asp:Repeater ID="rptLogs" runat="server">
                                        <ItemTemplate>
                                            <b>
                                                <%# Eval("CreationTime")%>
                                            </b>
                                            <br />
                                            <%# Eval("Msg") %>
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Literal ID="lNoLogs" runat="server">There are no logs.</asp:Literal>
                                </div>
                            </td>
                        </tr>
                </table>
            </asp:Panel>
            <asp:DragPanelExtender ID="DragPanelExtender1" runat="server" TargetControlID="pnlMain"
                DragHandleID="trLogViewerHeader" />
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>

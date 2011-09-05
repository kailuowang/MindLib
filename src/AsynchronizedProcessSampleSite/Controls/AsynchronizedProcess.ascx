<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AsynchronizedProcess.ascx.cs"
    Inherits="Controls_AsynchronizedProcess" %>
<%@ Register Src="Msg.ascx" TagName="Msg" TagPrefix="uc2" %>
<%@ Register Src="PercentageBar.ascx" TagName="PercentageBar" TagPrefix="uc1" %>
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <asp:LinkButton ID="lbHidden" runat="server"></asp:LinkButton>
        <div id="divPopupProcessor" runat="server">
            <div id="divInnerProcessor" runat="server" class="modalPopup" style="width: 900px;"
                visible="false">
                <asp:UpdatePanel ID="upTopBar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%" cellspacing="0" cellpadding="5">
                            <tr style="background-color: #f0f0f0">
                                <td align="left" width="1%" nowrap>
                                    <b>
                                        <asp:Label ID="lTitle" runat="server"></asp:Label></b>
                                </td>
                                <asp:PlaceHolder ID="phCancel" runat="server" Visible="true">
                                    <td align="right">
                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                            CausesValidation="false" />
                                        <asp:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btnCancel" ConfirmText="Are you sure you want to cancel to process? None of the results will be saved." />
                                    </td>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="phComplete" runat="server" Visible="false">
                                    <td align="left">
                                        <b>
                                            <asp:Literal ID="lFinishMessage" runat="server">...Done!</asp:Literal></b>
                                    </td>
                                    <td align="right">
                                        <asp:LinkButton ID="btnFinish" runat="server" Text="Close" OnClick="btnFinish_Click"
                                            CausesValidation="false" />
                                    </td>
                                </asp:PlaceHolder>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table width="100%" cellspacing="0" cellpadding="5" style="border: solid 1px silver;">
                    <tr>
                        <td>
                            <b>Task Logs</b></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="upLogs" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div style="width: 100%; height: 500px; overflow: auto;">
                                        <asp:Literal ID="tbLogMessage" runat="server"></asp:Literal>
                                    </div>
                                    <asp:CheckBox ID="cbAutoRefreshLogs" Text="Auto Refresh" runat="server" AutoPostBack="true"
                                        Checked="true" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="upProgress" runat="server" UpdateMode="conditional">
                    <ContentTemplate>
                        <table cellpadding="5" cellspacing="0" width="100%">
                            <asp:PlaceHolder ID="phProgressBar" runat="server">
                                <tr>
                                    <td align="left">
                                        <uc1:PercentageBar ID="PercentageBar1" runat="server"></uc1:PercentageBar>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <td align="left">
                                    <b>Started at: </b>
                                    <asp:Literal ID="lStart" runat="server"></asp:Literal><br />
                                    <b>Time span: </b>
                                    <asp:Literal ID="lTimeSpan" runat="server"></asp:Literal>
                                    &nbsp;&nbsp;
                                    <asp:PlaceHolder ID="phRemaining" runat="server"><b>Estimated remainning: </b>
                                        <asp:Literal ID="lRemainning" runat="server"></asp:Literal>
                                    </asp:PlaceHolder>
                                </td>
                            </tr>
                        </table>
                        <asp:Timer ID="Timer1" runat="server" Interval="500" OnTick="Timer1_Tick" Enabled="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:ModalPopupExtender runat="server" ID="mpeProcessor" BackgroundCssClass="modalBackground"
            PopupControlID="divPopupProcessor" DropShadow="false" TargetControlID="lbHidden" />
    </ContentTemplate>
</asp:UpdatePanel>
<uc2:Msg ID="MsgTaskExists" OnConfirmed="MsgTaskExistsConfirmed"  runat="server" />

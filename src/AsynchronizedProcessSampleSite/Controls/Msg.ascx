<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Msg.ascx.cs" Inherits="GeneralControl_Msg" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <asp:LinkButton ID="lbHidden" runat="server"></asp:LinkButton>
        <div id="divPopup" runat="server"  >
        <div id="divInner" runat="server" class="modalPopup" visible="false" >
            <asp:UpdatePanel ID="upInner" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                    <table width="100%" cellspacing="0">
                        <tr style="background-color: #f0f0f0">
                            <td align="left">
                                &nbsp;<b><asp:Literal ID="ltTitle" runat="server"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Literal ID="ltMsgInfo" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnOk" runat="server" Text="  Ok  " OnClick="btnOk_Click" Visible="true"
                                    CausesValidation="false" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                    CausesValidation="false" Visible="true" />
                            </td>
                        </tr>
                    </table>
                   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div> 
        </div>
        <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1"  
        BackgroundCssClass="modalBackground" PopupControlID="divPopup" DropShadow="false" TargetControlID="lbHidden"/>
    </ContentTemplate>
</asp:UpdatePanel>

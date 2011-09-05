<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PercentageBar.ascx.cs"
    Inherits="PercentageBar" %>

<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td width="100%">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="<%=percentage %>%">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class="progressbar">
                                    <a title="<%=percentage %>">
                                        <asp:Image ID="imgSpacer" ImageUrl="~/images/spacer.gif" Width="100%" Height="10"
                                            runat="server" BorderWidth="0" />
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="<%=leftPercentage %>%">&nbsp;</td>
                </tr>
            </table>
        </td>
          <td width="1%">&nbsp;&nbsp;<%=percentage %>%</td>
    </tr>
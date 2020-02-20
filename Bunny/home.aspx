<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Bunny.home" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bunny MP4 Video Upload</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div>
            Upload MP4 Video:
            <asp:FileUpload ID="upload" runat="server" />
            <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click" />
        </div>
         <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:ListView  ID="VideoDisplayControl" runat="server">
                        <LayoutTemplate>
                            <div id="itemPlaceholder"  runat="server">
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div>
                               <video src='<%# Eval("Url") %>' controls="" preload="none"></video>
                               <asp:Literal ID="label" Text='<%# Eval("Title") %>' runat="server"/>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:Button ID="Button1" runat="server" Text="Refresh" OnClick="refresh_Button_Click" />
        </div>
    </form>
</body>
</html>



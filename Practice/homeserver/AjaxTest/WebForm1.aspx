<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="AjaxTest.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Panel ID="Panel1" runat="server" Height="153px" Width="679px">
            <asp:Table ID="Table1" runat="server" Height="64px" Width="120px">
            </asp:Table>
            <asp:RadioButton ID="RadioButton1" runat="server" />
        </asp:Panel>
    
    </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View2" runat="server">
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
            </asp:View>
            <asp:View ID="View1" runat="server">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" Width="108px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:RadioButtonList>
            </asp:View>
        </asp:MultiView>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AjaxTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hello World</title>
    <script runat="server" type="text/c#">
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblHelloWorld.Text = "Panel refreshed at: " + DateTime.Now.ToLongTimeString();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="MainScriptManager" runat="server" />
        <asp:UpdatePanel ID="pnlHelloWorld" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer>
                <asp:Label runat="server" ID="lblHelloWorld" Text="Click the button" />
                <br />
                <br />
                <asp:Panel ID="Panel1" runat="server">
                    <asp:Button runat="server" ID="btnHelloWorld" Text="Update label!" OnClick="btnHelloWorld_Click" />
                    <asp:Button runat="server" ID="btnStart" Text="Start" OnClick="btnStart_Click" />
                    <asp:Button runat="server" ID="btnRegister" Text="Register" OnClick="btnRegister_Click" />
                    <asp:Button runat="server" ID="btnDeregister" Text="Deregister" OnClick="btnDeregister_Click" />
                    <asp:Button runat="server" ID="btnStop" Text="Stop" OnClick="btnStop_Click" />
                    <asp:ListBox runat="server" ID="lbxHelloWorld" Width="640" Height="480" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

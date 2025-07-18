<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="asg.Login" %>
<asp:Content ID="LoginTitle" ContentPlaceHolderID="PageTitle" runat="server">
    Login
</asp:Content>

<asp:Content ID="LoginCss" ContentPlaceHolderID="CssLink" runat="server">
    <link rel="stylesheet" type="text/css" href="Login.css" />
    <script>
    function togglePasswordVisibility() {
        var passwordField = document.getElementById('<%= txtPw.ClientID %>');
        var toggleButton = document.getElementById('<%= btnTogglePassword.ClientID %>');
        
        if (passwordField.type === "password") {
            passwordField.type = "text";
            toggleButton.value = "Hide"; // Update button text to "Hide"
        } else {
            passwordField.type = "password";
            toggleButton.value = "Show"; // Update button text to "Show"
        }
    }
    </script>

</asp:Content>

<asp:Content ID="LoginContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="homeContent">
    <div class="loginDiv" style="height: 600px;">
        <div class="left">            
            <h1>Welcome to Doughy Delight!</h1>
            <asp:Label ID="lblEmail" runat="server" Text="Email *"></asp:Label>
            <br />
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:Label ID="lblEmailExist" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                ErrorMessage="You must enter an Email." ControlToValidate="txtEmail"
                Display="Dynamic" ForeColor="Red" ValidationGroup="Login" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                ErrorMessage="Invalid email format." Display="Dynamic" ForeColor="Red" ValidationGroup="Login" SetFocusOnError="True" />
            <br /><br />
        
            <asp:Label ID="lblPw" runat="server" Text="Password *"></asp:Label>
            <br />
            <asp:TextBox ID="txtPw" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnTogglePassword" runat="server" Text="Show" OnClientClick="togglePasswordVisibility(); return false;" 
                CssClass="loginBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
            <br />
            <asp:RequiredFieldValidator ID="rfvPw" runat="server"
                ErrorMessage="You must enter a Password." ControlToValidate="txtPw"
                Display="Dynamic" ForeColor="Red" ValidationGroup="Login" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <br /><br />
                   
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="loginBtn" OnClick="btnLogin_Click" ValidationGroup="Login"/>
            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" CssClass="loginBtn" />
            <br />

            <div class="forgetBtn">
                <asp:HyperLink ID="hplForgetPw" runat="server" NavigateUrl="~/PasswordRecovery.aspx">Forget your password?</asp:HyperLink>                
            </div>
        </div>
            
        <div class="right">
            <img src="Images/login.png" alt="loginImg" class="loginImg"/>
        </div>
    </div>
</div>
</asp:Content>

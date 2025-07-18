<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="asg.PasswordRecovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Password Recovery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CssLink" runat="server">
    <link rel="stylesheet" type="text/css" href="Login.css" />
    <script>
        function togglePasswordVisibility1() {
            var passwordField = document.getElementById('<%= txtNewPw.ClientID %>');
            var toggleButton = document.getElementById('<%= btnTogglePassword1.ClientID %>');

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleButton.value = "Hide"; // Update button text to "Hide"
            } else {
                passwordField.type = "password";
                toggleButton.value = "Show"; // Update button text to "Show"
            }
        }

        function togglePasswordVisibility2() {
            var passwordField = document.getElementById('<%= txtConfirmPw.ClientID %>');
            var toggleButton = document.getElementById('<%= btnTogglePassword2.ClientID %>');

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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<div class="homeContent">
    <div class="loginDiv">
        <div class="left">
            <h1>Password Recovery</h1>
            <asp:Button ID="btnBack" runat="server" Text="Back" class="loginBtn" OnClick="btnBack_Click" style="position: absolute; left: 10px; top:30px; width:90px;" />
            <asp:Label ID="lblEmail" runat="server" Text="Email *"></asp:Label>
            <br />
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox> 
            <br />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                ErrorMessage="You must enter an Email." ControlToValidate="txtEmail" 
                Display="Dynamic" ForeColor="Red" ValidationGroup="validateSec"></asp:RequiredFieldValidator> 
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                ErrorMessage="Invalid email format." Display="Dynamic" ForeColor="Red" ValidationGroup="validateSec"/>
            <br /><br />    

            <asp:Label ID="lblSec" runat="server" Text="Security Question *"></asp:Label>
            <br />
            <asp:DropDownList ID="ddlSecQue" runat="server">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem Value="City">What city were you born in?</asp:ListItem>
                <asp:ListItem Value="School">What was the name of your first school?</asp:ListItem>
                <asp:ListItem Value="Nickname">What was your childhood nickname?</asp:ListItem>
                <asp:ListItem Value="Movie">What is your favorite movie from childhood?</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:RequiredFieldValidator ID="rfvSecQue" runat="server" 
                ErrorMessage="You must select a Security Question." ControlToValidate="ddlSecQue" 
                Display="Dynamic" ForeColor="Red" ValidationGroup="validateSec"></asp:RequiredFieldValidator>
            <br /><br />

            <asp:Label ID="lblSecAns" runat="server" Text="Security Answer *"></asp:Label>
            <br />
            <asp:TextBox ID="txtSecAns" runat="server"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvSecAns" runat="server" 
                ErrorMessage="You must enter a Security Answer." ControlToValidate="txtSecAns" 
                Display="Dynamic" ForeColor="Red" ValidationGroup="validateSec"></asp:RequiredFieldValidator>
            <br /><br />
            
            <asp:Button ID="btnPwRec" runat="server" Text="Submit" CssClass="loginBtn" OnClick="btnPwRec_Click" ValidationGroup="validateSec" />
            <br /><br />

            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>            
            <br /><br />

            <asp:Panel ID="pnlChangePw" runat="server" Visible="False">
                <div class="changePwDiv">
                    <h3>Please Update a New Password</h3>
                    <table class="changePwTable">
                        <tr>
                            <td>
                                <asp:Label ID="lblNewPw" runat="server" Text="New Passowrd *"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtNewPw" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:Button ID="btnTogglePassword1" runat="server" Text="Show" 
                                    OnClientClick="togglePasswordVisibility1(); return false;" 
                                    CssClass="pwRecBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvNewPw" runat="server"
                                    ErrorMessage="You must enter a New Password." ControlToValidate="txtNewPw"
                                    Display="Dynamic" ForeColor="Red" ValidationGroup="ChangePw" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPw" runat="server" ControlToValidate="txtNewPw"
                                    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                                    ErrorMessage="Password must be at least 8 characters long and contain both letters and numbers."
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="ChangePw" />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblConfirmPw" runat="server" Text="Confirm Passowrd *"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtConfirmPw" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:Button ID="btnTogglePassword2" runat="server" Text="Show" 
                                    OnClientClick="togglePasswordVisibility2(); return false;" 
                                    CssClass="pwRecBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvConfirmPw" runat="server"
                                    ErrorMessage="You must enter Confirm Password." ControlToValidate="txtConfirmPw"
                                    Display="Dynamic" ForeColor="Red" ValidationGroup="ChangePw" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvConfirmPw" runat="server" ControlToValidate="txtConfirmPw"
                                    ControlToCompare="txtNewPw" ErrorMessage="Passwords do not match."
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="True" Operator="Equal" ValidationGroup="ChangePw" />                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblChangePwMsg" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <br />

                    <asp:Button ID="btnNewPw" runat="server" Text="Submit" CssClass="pwRecBtn" OnClick="btnNewPw_Click" ValidationGroup="ChangePw"/>
                </div>
            </asp:Panel>
        </div>

        <div class="right">
            <img src="Images/login.png" alt="loginImg" class="loginImg"/>
        </div>
    </div>
</div>
</asp:Content>

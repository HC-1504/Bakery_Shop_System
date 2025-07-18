<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="asg.Register" %>
<asp:Content ID="RegisterTitle" ContentPlaceHolderID="PageTitle" runat="server">
    Register
</asp:Content>

<asp:Content ID="RegisterCss" ContentPlaceHolderID="CssLink" runat="server">
    <link rel="stylesheet" type="text/css" href="Login.css" />
    <script>
        function togglePasswordVisibility1() {
            var passwordField = document.getElementById('<%= txtPw.ClientID %>');
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

<asp:Content ID="RegisterContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="homeContent">
    <div class="loginDiv" style="height:auto;">
        <div class="left">
            <h1>Welcome to Doughy Delight!</h1>           
            <asp:Label ID="lblName" runat="server" Text="Name *"></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvName" runat="server"
                ErrorMessage="You must enter an Name." ControlToValidate="txtName"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <br /><br />

            <asp:Label ID="lblEmail" runat="server" Text="Email *"></asp:Label>
            <br />
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblEmailExist" runat="server" Text="" ForeColor="Red"></asp:Label>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                ErrorMessage="You must enter an Email." ControlToValidate="txtEmail"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>           
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                ErrorMessage="Invalid email format." Display="Dynamic" ForeColor="Red" 
                SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RegularExpressionValidator>
            <br /><br />    

            <asp:Label ID="lblContact" runat="server" Text="Contact No (10-15 digits without '-') *"></asp:Label>
            <br />
            <asp:TextBox ID="txtContact" runat="server"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvContact" runat="server"
                ErrorMessage="You must enter a Contact No." ControlToValidate="txtContact"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revContact" runat="server" ControlToValidate="txtContact"
                ValidationExpression="^(01[0-9])\d{7,12}$"
                ErrorMessage="Invalid phone number format." Display="Dynamic" ForeColor="Red" 
                SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RegularExpressionValidator>
            <br /><br />

            <asp:Label ID="lblDOB" runat="server" Text="Date of Birth (MM/dd/yyyy) *"></asp:Label>
            <br />
            <asp:TextBox ID="txtDOB" runat="server" AutoPostBack="True" OnTextChanged="txtDOB_TextChanged"></asp:TextBox>  
            <br />
            <asp:Calendar ID="cldDOB" runat="server" OnSelectionChanged="cldDOB_SelectionChanged" ShowNextPrevMonth="True" ShowYearSelector="true">
                <SelectedDayStyle BackColor="#FFCC99"></SelectedDayStyle>
                <TitleStyle BackColor="#FFCC99"></TitleStyle>
            </asp:Calendar>
            <asp:RequiredFieldValidator ID="rfvDOB" runat="server"
                ErrorMessage="You must select a DOB." ControlToValidate="txtDOB"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" 
                ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revDateOfBirth" runat="server"
                ControlToValidate="txtDOB" ValidationExpression="^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$"
                ErrorMessage="Invalid Date format (MM/dd/yyyy)." Display="Dynamic" 
                ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"/>
            <asp:CompareValidator
                ID="cvDOB"
                runat="server"
                ErrorMessage="DOB cannot be more than today's date."
                ControlToValidate="txtDOB"
                Operator="LessThanEqual"
                Type="Date"
                ForeColor="Red" SetFocusOnError="True"
                ValidationGroup="RegistrationGroup"></asp:CompareValidator>
            <br /><br />

            <asp:Label ID="lblGender" runat="server" Text="Gender *"></asp:Label>
            <br />
            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">                
                <asp:ListItem Value="Male">Male</asp:ListItem>
                <asp:ListItem Value="Female">Female</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvGender" runat="server"
                ErrorMessage="You must select a gender." ControlToValidate="rblGender"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <br /><br />

            <asp:Label ID="lblProfilePic" runat="server" Text="Profile Pic"></asp:Label>
            <br />            
            <asp:FileUpload ID="fuProfilePic" runat="server"/>
            <br />
            <asp:Label ID="lblUploadStatus" runat="server" Text="A default Profile Pic will be used if no picture/ invalid picture is uploaded."></asp:Label>
            <br />
            <br /><br />

            <asp:Label ID="lblPw" runat="server" Text="Password *"></asp:Label>
            <br />
            <asp:TextBox ID="txtPw" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnTogglePassword1" runat="server" Text="Show" OnClientClick="togglePasswordVisibility1(); return false;" 
                CssClass="loginBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
            <br />
            <asp:RequiredFieldValidator ID="rfvPw" runat="server"
                ErrorMessage="You must enter a Password." ControlToValidate="txtPw"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revPw" runat="server" ControlToValidate="txtPw"
                ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                ErrorMessage="Password must be at least 8 characters long and contain both letters and numbers."
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"/>
            <br /><br />

            <asp:Label ID="lblConfirmPw" runat="server" Text="Confirm Password *"></asp:Label>
            <br />
            <asp:TextBox ID="txtConfirmPw" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnTogglePassword2" runat="server" Text="Show" OnClientClick="togglePasswordVisibility2(); return false;" 
                CssClass="loginBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
            <br />
            <asp:RequiredFieldValidator ID="rfvConfirmPw" runat="server"
                ErrorMessage="You must enter a Confirm Password." ControlToValidate="txtConfirmPw"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvConfirmPw" runat="server" ControlToValidate="txtConfirmPw"
                ControlToCompare="txtPw" ErrorMessage="Passwords do not match."
                ForeColor="Red" Display="Dynamic" SetFocusOnError="True" Operator="Equal" ValidationGroup="RegistrationGroup"/>
            <br /><br />

            <asp:Label ID="lblSec" runat="server" Text="Security Question (Used for Password Recovery)"></asp:Label>
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
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <br /><br />

            <asp:Label ID="lblSecAns" runat="server" Text="Security Answer"></asp:Label>
            <br />
            <asp:TextBox ID="txtSecAns" runat="server"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvSecAns" runat="server"
                ErrorMessage="You must enter a Security Answer." ControlToValidate="txtSecAns"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="RegistrationGroup"></asp:RequiredFieldValidator>
            <br /><br />

            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="loginBtn" OnClick="btnRegister_Click" ValidationGroup="RegistrationGroup"/>
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="loginBtn" OnClick="btnClear_Click" />
            <br />
        </div>

        <div class="right">
            <img src="Images/login.png" alt="loginImg" class="loginImg" />
        </div>
    </div>
</div>
</asp:Content>

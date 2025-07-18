<%@ Page Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="AccountSetting.aspx.cs" Inherits="asg.userProfile" %>

<asp:Content ID="AccountSettingTitle" ContentPlaceHolderID="PageTitle" Runat="Server">
    Account Setting
</asp:Content>

<asp:Content ID="AccountSettingCss" ContentPlaceHolderID="CssLink" Runat="Server">
    <link rel="stylesheet" type="text/css" href="AccountSetting.css" />
    <script>
        function togglePasswordVisibility1() {
            var passwordField = document.getElementById('<%= txtCurrentPw.ClientID %>');
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
        var passwordField = document.getElementById('<%= txtNewPw.ClientID %>');
        var toggleButton = document.getElementById('<%= btnTogglePassword2.ClientID %>');

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleButton.value = "Hide"; // Update button text to "Hide"
            } else {
                passwordField.type = "password";
                toggleButton.value = "Show"; // Update button text to "Show"
            }
        }

        function togglePasswordVisibility3() {
            var passwordField = document.getElementById('<%= txtConfirmPw.ClientID %>');
            var toggleButton = document.getElementById('<%= btnTogglePassword3.ClientID %>');

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleButton.value = "Hide"; // Update button text to "Hide"
            } else {
                passwordField.type = "password";
                toggleButton.value = "Show"; // Update button text to "Show"
            }
        }

        function togglePasswordVisibilitySec1() {
            var passwordField = document.getElementById('<%= txtCurrentPwSec.ClientID %>');
            var toggleButton = document.getElementById('<%= btnToggleSec1.ClientID %>');

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

<asp:Content ID="AccountSettingContent" ContentPlaceHolderID="MainContent" Runat="Server">   
    <div class="homeContent">
        <h1>Account Setting</h1>       
        <br />

        <div class="accountDiv">
            <div class="left">
                <asp:SqlDataSource ID="AccSettingLeftDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [CustomerID], [Customername], [ProfilePic] FROM [Customer] WHERE ([CustomerID] = @CustomerID)">
                    <SelectParameters>
                        <asp:SessionParameter SessionField="CustomerID" Name="CustomerID" Type="String"></asp:SessionParameter>
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:Repeater ID="rptLeftDiv" runat="server" DataSourceID="AccSettingLeftDataSource">
                    <ItemTemplate>
                        <div style="height:150px; width:200px; overflow:hidden; display:flex; justify-content:center; align-items:center;">
                            <img src="<%#Eval("ProfilePic") %>" alt="profilePic" class="profilePic" style="max-width:100%; max-height:100%;"/>
                        </div>
                        <br />
                        <asp:FileUpload ID="fuProfilePic" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" CssClass="accountBtn" />
                        <br />
                        <div><b>Customer ID: </b><%#Eval("CustomerID") %></div>
                        <h2>Hello, <%#Eval("Customername") %>!</h2>
                    </ItemTemplate>
                </asp:Repeater>     
                <asp:Label ID="lblUploadStatus" runat="server"></asp:Label>
            </div>        

            <div class="right">
                <asp:SqlDataSource ID="AccSettingDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT [CustomerID], [Customername], [Email], [ContactNo], [DateOfBirth], [Gender] FROM [Customer] WHERE ([CustomerID] = @CustomerID)"
                    DeleteCommand="DELETE FROM [Customer] WHERE [CustomerID] = @CustomerID"
                    InsertCommand="INSERT INTO [Customer] ([CustomerID], [Customername], [Email], [ContactNo], [DateOfBirth], [Gender]) VALUES (@CustomerID, @Customername, @Email, @ContactNo, @DateOfBirth, @Gender)"
                    UpdateCommand="UPDATE [Customer] SET [Customername] = @Customername, [Email] = @Email, [ContactNo] = @ContactNo, [DateOfBirth] = @DateOfBirth, [Gender] = @Gender WHERE [CustomerID] = @CustomerID">
                    <DeleteParameters>
                        <asp:Parameter Name="CustomerID" Type="String"></asp:Parameter>
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="CustomerID" Type="String"></asp:Parameter>
                        <asp:Parameter Name="Customername" Type="String"></asp:Parameter>
                        <asp:Parameter Name="Email" Type="String"></asp:Parameter>
                        <asp:Parameter Name="ContactNo" Type="String"></asp:Parameter>
                        <asp:Parameter DbType="Date" Name="DateOfBirth"></asp:Parameter>
                        <asp:Parameter Name="Gender" Type="String"></asp:Parameter>
                    </InsertParameters>
                    <SelectParameters>
                        <asp:SessionParameter SessionField="CustomerID" Name="CustomerID" Type="String"></asp:SessionParameter>
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Customername" Type="String"></asp:Parameter>
                        <asp:Parameter Name="Email" Type="String"></asp:Parameter>
                        <asp:Parameter Name="ContactNo" Type="String"></asp:Parameter>
                        <asp:Parameter DbType="Date" Name="DateOfBirth"></asp:Parameter>
                        <asp:Parameter Name="Gender" Type="String"></asp:Parameter>
                        <asp:Parameter Name="CustomerID" Type="String"></asp:Parameter>
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:DetailsView ID="AccSettingDetailsView" runat="server" Height="100%" Width="100%" CssClass="AccSettingTable" AutoGenerateRows="False" DataKeyNames="CustomerID" DataSourceID="AccSettingDataSource">
                    <Fields>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Customername") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Customername") %>' />
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                                    ControlToValidate="txtName" ErrorMessage="You must enter a Name." 
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' />                            
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail" ErrorMessage="You must enter an Email." 
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                    ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                                    ErrorMessage="Invalid Email format." Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                                <asp:CustomValidator ID="cvEmailExists" runat="server" 
                                    ControlToValidate="txtEmail" 
                                    OnServerValidate="ValidateEmailUniqueness" 
                                    ErrorMessage="This email already exists." 
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True"></asp:CustomValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Contact No">
                            <ItemTemplate>
                                <asp:Label ID="lblContactNo" runat="server" Text='<%# Bind("ContactNo") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContactNo" runat="server" Text='<%# Bind("ContactNo") %>' />                            
                                <asp:RequiredFieldValidator ID="rfvContactNo" runat="server" 
                                    ControlToValidate="txtContactNo" ErrorMessage="You must enter a Contact No." 
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                                <asp:RegularExpressionValidator ID="revContactNo" runat="server" 
                                    ControlToValidate="txtContactNo" ValidationExpression="^(01[0-9])\d{7,12}$" 
                                    ErrorMessage="Invalid Contact Number format (10-15 digits without '-')." Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date of Birth">
                            <ItemTemplate>
                                <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Bind("DateOfBirth", "{0:MM/dd/yyyy}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDOB" runat="server" Text='<%# Bind("DateOfBirth", "{0:MM/dd/yyyy}") %>'></asp:TextBox>  
                                Format: MM/dd/yyyy
                                <asp:RequiredFieldValidator ID="rfvDateOfBirth" runat="server" 
                                    ControlToValidate="txtDOB" ErrorMessage="You must enter a Date of Birth." 
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                                <asp:RegularExpressionValidator ID="revDateOfBirth" runat="server" 
                                    ControlToValidate="txtDOB" ValidationExpression="^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$" 
                                    ErrorMessage="Invalid Date format (MM/dd/yyyy)." Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                                <asp:CustomValidator ID="cvDateOfBirth" runat="server"
                                    ControlToValidate="txtDOB" 
                                    OnServerValidate="ValidateDOB"
                                    ErrorMessage="DOB cannot be more than today's date."
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True"></asp:CustomValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gender">
                            <ItemTemplate>
                                <asp:Label ID="lblGender" runat="server" Text='<%# Bind("Gender") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" SelectedValue='<%# Bind("Gender") %>'>                
                                    <asp:ListItem Value="Male" Style="font-weight: normal;">Male</asp:ListItem>
                                    <asp:ListItem Value="Female">Female</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="rfvGender" runat="server" 
                                    ControlToValidate="rblGender" ErrorMessage="You must select a Gender." 
                                    Display="Dynamic" InitialValue="" ForeColor="Red" SetFocusOnError="True" />
                            </EditItemTemplate>
                        </asp:TemplateField>


                        <asp:CommandField ShowEditButton="True" ControlStyle-CssClass="accountBtn" ButtonType="Button"></asp:CommandField>
                    </fields>
                </asp:DetailsView>
                <br /><br />

                <asp:Button ID="btnChangePw" runat="server" Text="Change Password" CssClass="accountBtn" OnClick="btnChangePw_Click" style="padding-left: 10px;padding-right: 10px; width: auto;"/>
                <asp:Button ID="btnChangeSec" runat="server" Text="Change Security QnA" CssClass="accountBtn" OnClick="btnChangeSec_Click" style="padding-left: 10px;padding-right: 10px; width: auto;"/>
                <asp:Button ID="btnDeleteAccount" runat="server" Text="Delete Account" CssClass="accountBtn" OnClientClick="return confirm('Are you sure you want to delete your account?');" OnClick="btnDeleteAccount_Click" />
                <br />
                <asp:Label ID="lblDeleteAccMessage" runat="server" Text=""></asp:Label>
                <br /><br />

                <asp:Panel ID="pnlChangePw" runat="server" Visible="false">
                    <div class="changePwDiv">
                        <h3>Change Password</h3>
                        <table class="changePwTable">
                            <tr>
                                <td>
                                     <asp:Label ID="lblCurrentPw" runat="server" Text="Current Password *"></asp:Label>
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtCurrentPw" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:Button ID="btnTogglePassword1" runat="server" Text="Show" 
                                        OnClientClick="togglePasswordVisibility1(); return false;" 
                                        CssClass="accountBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
                                    <asp:RequiredFieldValidator ID="rfvCurrentPw" runat="server"
                                        ErrorMessage="You must enter Current Password." ControlToValidate="txtCurrentPw"
                                        Display="Dynamic" ForeColor="Red" ValidationGroup="ChangePw" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblNewPw" runat="server" Text="New Passowrd *"></asp:Label>
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtNewPw" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:Button ID="btnTogglePassword2" runat="server" Text="Show" 
                                        OnClientClick="togglePasswordVisibility2(); return false;" 
                                        CssClass="accountBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
                                    <asp:RequiredFieldValidator ID="rfvNewPw" runat="server"
                                        ErrorMessage="You must enter a New Password." ControlToValidate="txtNewPw"
                                        Display="Dynamic" ForeColor="Red" ValidationGroup="ChangePw" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPw" runat="server" ControlToValidate="txtNewPw"
                                        ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                                        ErrorMessage="Password must be at least 8 characters long and contain both letters and numbers."
                                        Display="Dynamic" ForeColor="Red" SetFocusOnError="True" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmPw" runat="server" Text="Confirm Passowrd *"></asp:Label>
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPw" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:Button ID="btnTogglePassword3" runat="server" Text="Show" 
                                        OnClientClick="togglePasswordVisibility3(); return false;" 
                                        CssClass="accountBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
                                    <asp:RequiredFieldValidator ID="rfvConfirmPw" runat="server" 
                                        ErrorMessage="You must enter Confirm Password." ControlToValidate="txtConfirmPw" 
                                        Display="Dynamic" ForeColor="Red" ValidationGroup="ChangePw"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvConfirmPw" runat="server" ControlToValidate="txtConfirmPw"
                                        ControlToCompare="txtNewPw" ErrorMessage="Passwords do not match."
                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="True" Operator="Equal" />                            
                                </td>
                            </tr>
                        </table>                       
                        <br />
                        <asp:Label ID="lblChangePwMsg" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                        <br />

                        <asp:Button ID="btnNewPw" runat="server" Text="Submit" CssClass="accountBtn" OnClick="btnNewPw_Click" ValidationGroup="ChangePw"/>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="accountBtn" OnClick="btnCancel_Click" />
                    </div>
                </asp:Panel>                                                        

                <asp:Panel ID="pnlChangeSec" runat="server" Visible="false">
                    <div class="changeSecDiv">
                        <h3>Change Security QnA</h3>
                        <table class="changeSecTable">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCurrentPwSec" runat="server" Text="Current Password *"></asp:Label>
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtCurrentPwSec" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:Button ID="btnToggleSec1" runat="server" Text="Show" 
                                        OnClientClick="togglePasswordVisibilitySec1(); return false;" 
                                        CssClass="accountBtn" style="width:auto; height:auto; padding: 5px 10px;"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="You must enter Current Password." ControlToValidate="txtCurrentPwSec"
                                        Display="Dynamic" ForeColor="Red" ValidationGroup="ChangeSec" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Security Question (Used for Password Recovery)"></asp:Label>
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlSecQue" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="City">What city were you born in?</asp:ListItem>
                                        <asp:ListItem Value="School">What was the name of your first school?</asp:ListItem>
                                        <asp:ListItem Value="Nickname">What was your childhood nickname?</asp:ListItem>
                                        <asp:ListItem Value="Movie">What is your favorite movie from childhood?</asp:ListItem>
                                    </asp:DropDownList>                                    
                                    <asp:RequiredFieldValidator ID="rfvSecQue" runat="server"
                                        ErrorMessage="You must select a Security Question." ControlToValidate="ddlSecQue"
                                        Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="ChangeSec"></asp:RequiredFieldValidator>                                    
                                </td>
                            </tr>

                            <tr><td><br /></td></tr>

                            <tr>                                
                                <td>
                                    <asp:Label ID="lblSecAns" runat="server" Text="Security Answer"></asp:Label>
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtSecAns" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSecAns" runat="server"
                                        ErrorMessage="You must enter a Security Answer." ControlToValidate="txtSecAns"
                                        Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="ChangeSec"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>                       
                        <br />
                        <asp:Label ID="lblErrorMsgSec" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                        <br />

                        <asp:Button ID="btnSubmitSec" runat="server" Text="Submit" CssClass="accountBtn" ValidationGroup="ChangeSec" OnClick="btnSubmitSec_Click" />
                        <asp:Button ID="btnCancelSec" runat="server" Text="Cancel" CssClass="accountBtn" OnClick="btnCancelSec_Click" />
                    </div>
                </asp:Panel>
            </div>
         </div>
    </div>
</asp:Content>
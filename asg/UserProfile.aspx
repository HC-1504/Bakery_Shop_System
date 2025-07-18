<%@ Page Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="asg.userProfile1" %>

<asp:Content ID="UserProfileTitle" ContentPlaceHolderID="PageTitle" Runat="Server">
    User Profile
</asp:Content>

<asp:Content ID="UserProfileCss" ContentPlaceHolderID="CssLink" Runat="Server">
    <link rel="stylesheet" type="text/css" href="UserProfile.css" />
</asp:Content>

<asp:Content ID="UserProfileContent" ContentPlaceHolderID="MainContent" Runat="Server">    

<div class="homeContent">
    <h1>User Profile</h1>
    <br />

    <asp:SqlDataSource ID="profileDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [CustomerID], [Customername], [ProfilePic], [Balance] FROM [Customer] WHERE ([CustomerID] = @CustomerID)">
        <SelectParameters>
            <asp:SessionParameter SessionField="CustomerID" Name="CustomerID" Type="String"></asp:SessionParameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:Repeater ID="rptProfile" runat="server" DataSourceID="profileDataSource">
        <ItemTemplate>
            <h2>Hello, <%#Eval("Customername") %>!</h2>

            <div class="profilePicDiv" style="height:150px; width:200px; overflow:hidden; display:flex; justify-content:center; align-items:center;">
                <img src=<%#Eval("ProfilePic") %> alt="profilePic" class="profilePic" style="max-width:100%; max-height:100%;"/>
            </div>
            <br />

            <div class="userIdNameDiv">
                <table>
                    <tr>
                        <td style="width: auto;">User ID</td>
                        <td style="width: auto;">:</td>
                        <td style="width: 100%;"><%#Eval("CustomerID") %></td>
                    </tr>

                    <tr>
                        <td>Username</td>
                        <td>:</td>
                        <td><%#Eval("Customername") %></td>
                    </tr>
                </table>
            </div>
            <br />
        </ItemTemplate>
    </asp:Repeater>
    

    <hr />

    <h2>Account Balance</h2>
    <br />
    <asp:Repeater ID="rptBalance" runat="server" DataSourceID="profileDataSource">
        <ItemTemplate>
            <span style="font-size:xx-large;">RM <%#Eval("Balance") %></span>
        </ItemTemplate>
    </asp:Repeater>
    <br /><br />
    <asp:Button ID="btnTopUp" runat="server" Text="+ Top Up" CssClass="profileBtn" style="width: auto; height:auto; padding: 10px 20px;" />
    <br />

    <hr />

    <p>You can edit your account details here!</p>   
    <div class="profileBtnDiv">
        <div>
            <asp:Button ID="btnAccountSetting" runat="server" Text="Account Setting" OnClick="btnAccountSetting_Click" CssClass="profileBtn" /> 
        </div>

        <div>
            <asp:Button ID="btnOrderTracking" runat="server" Text="Order Tracking" OnClick="btnOrderTracking_Click" CssClass="profileBtn" /> 
        </div>

        <div>
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="profileBtn" /> 
        </div>
    </div>
</div>
</asp:Content>

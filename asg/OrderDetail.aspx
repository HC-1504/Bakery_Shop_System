<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="asg.OrderDetail" %>

<asp:Content ID="OrderDetailTitle" ContentPlaceHolderID="PageTitle" runat="server">
    Order Details
</asp:Content>

<asp:Content ID="OrderDetailCss" ContentPlaceHolderID="CssLink" runat="server">
    <link rel="stylesheet" type="text/css" href="OrderDetail.css" />
</asp:Content>

<asp:Content ID="OrderDetailContent" ContentPlaceHolderID="MainContent" Runat="Server">

        <h1>Order Details</h1>

        <asp:Button ID="btnBack" runat="server" Text="Back" class="userBtn" OnClick="btnBack_Click" style="float:left"/>
    <div class="homeContent">
            <table class="orderInfoTable">
                <tr>
                    <th>Order ID:</th>
                    <td><asp:Label ID="lblOrderID" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Order Status:</th>
                    <td><asp:Label ID="lblOrderStatus" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Order Date:</th>
                    <td><asp:Label ID="lblOrderDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Payment ID:</th>
                    <td><asp:Label ID="lblPaymentID" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Total Price:</th>
                    <td><asp:Label ID="lblTotalPrice" runat="server"></asp:Label></td>
                </tr>
            </table>

            <h2>Products</h2>
            <asp:Repeater ID="rptOrderProducts" runat="server">
                <HeaderTemplate>
                    <table class="productInfoTable">
                        <tr>
                            <th>Product Image</th>
                            <th>Product Name</th>
                            <th>Quantity</th>
                            <th>Sub Price</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><img src='<%# Eval("Image") %>' alt="Product Image" width="100" height="100" /></td>
                        <td><%# Eval("Name") %></td>
                        <td><%# Eval("Quantity") %></td>
                        <td>RM<%# Eval("SubPrice", "{0:0.00}") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

        </div>


</asp:Content>

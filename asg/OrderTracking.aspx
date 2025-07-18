<%@ Page Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="OrderTracking.aspx.cs" Inherits="asg.OrderTracking" %>

<asp:Content ID="OrderTrackingTitle" ContentPlaceHolderID="PageTitle" Runat="Server">
    Order Tracking
</asp:Content>

<asp:Content ID="OrderTrackingCss" ContentPlaceHolderID="CssLink" Runat="Server">
    <link rel="stylesheet" type="text/css" href="OrderTracking.css" />
</asp:Content>

<asp:Content ID="OrderTrackingContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="homeContent">
        <h1>Order Tracking</h1>

        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
            <asp:ListItem Selected="True">All</asp:ListItem>
            <asp:ListItem>Processing</asp:ListItem>
            <asp:ListItem>Pick up</asp:ListItem>
            <asp:ListItem>Completed</asp:ListItem>
        </asp:DropDownList>
        <br />

        <asp:SqlDataSource ID="processingOrdersDataSource" runat="server"
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT O.OrderID, O.OrderDate, O.OrderStatus, P.PaymentID 
                           FROM [Order] O 
                           LEFT JOIN [Payment] P 
                           ON O.OrderID = P.OrderID 
                           WHERE O.CustomerID = @CustomerID AND O.OrderStatus = 'Processing'">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>


        <asp:Repeater ID="rptProcessingOrder" runat="server" DataSourceID="processingOrdersDataSource" OnItemCommand="rptProcessingOrder_ItemCommand">
            <HeaderTemplate>
                <div class="ProcessingOrderHeader">
                    <h3>Processing Order(s)</h3>
                </div>
                <br />
            </HeaderTemplate>

            <ItemTemplate>
                <div class="processingOrderItem"> 
                    <table> 
                        <tr>
                            <th>
                                <b>Order Status: <%#Eval("OrderStatus") %></b>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrderID" runat="server" Text="Order ID"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("OrderID") %>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblOrderDateTime" runat="server" Text="Order Date Time"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("OrderDate") %>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblPaymentID" runat="server" Text="Payment ID"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("PaymentID") %>
                            </td>
                        </tr>
                    </table>
                    <p>Dear customer, your ordered product(s) still in processing.</p>
                    <div style="text-align: right;">
                        <asp:Button ID="btnMore" runat="server" Text="More Details" CssClass="orderTrackingBtn" CommandArgument='<%# Eval("OrderID") %>' OnClick="btnMore_Click"/>
                    </div>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>

        <asp:SqlDataSource ID="pickUpOrdersDataSource" runat="server"
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT O.OrderID, O.OrderDate, O.OrderStatus, P.PaymentID 
                           FROM [Order] O 
                           LEFT JOIN [Payment] P 
                           ON O.OrderID = P.OrderID 
                           WHERE O.CustomerID = @CustomerID AND O.OrderStatus = 'Pick_Up'">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>        

        <asp:Repeater ID="rptPickUpOrder" runat="server" DataSourceID="pickUpOrdersDataSource" OnItemCommand="rptPickUpOrder_ItemCommand">
            <HeaderTemplate>
                <div class="PickUpOrderHeader">
                    <h3>Ready to Pick up Order(s)</h3>
                </div>
                <br />
            </HeaderTemplate>

            <ItemTemplate>
                <div class="pickUpOrderItem"> 
                    <table> 
                        <tr>
                            <th>
                                <b>Order Status: <%#Eval("OrderStatus") %></b>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrderID" runat="server" Text="Order ID"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("OrderID") %>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblOrderDateTime" runat="server" Text="Order Date Time"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("OrderDate") %>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblTotal" runat="server" Text="Payment ID"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("PaymentID") %>
                            </td>
                        </tr>
                    </table>
                    <p>Dear customer, please pick up your ordered product(s) at <br />
                        <b>No. 27, Jalan Indah 3,Taman Mutiara, 51200 Kuala Lumpur, Wilayah Persekutuan, Malaysia</b>!</p>
                    <div style="text-align: right;">
                        <asp:Button ID="btnMore" runat="server" Text="More Details" CssClass="orderTrackingBtn" CommandArgument='<%# Eval("OrderID") %>' OnClick="btnMore_Click"/>
                    </div>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>

        <asp:SqlDataSource ID="completedOrdersDataSource" runat="server"
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT O.OrderID, O.OrderDate, O.OrderStatus, P.PaymentID 
                           FROM [Order] O 
                           LEFT JOIN [Payment] P 
                           ON O.OrderID = P.OrderID 
                           WHERE O.CustomerID = @CustomerID AND O.OrderStatus = 'Completed'">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
      
        <asp:Repeater ID="rptCompleteOrder" runat="server" DataSourceID="completedOrdersDataSource" OnItemCommand="rptCompleteOrder_ItemCommand">
            <HeaderTemplate>
                <div class="CompleteOrderHeader">
                    <h3>Completed Order(s)</h3>
                </div>
                <br />
            </HeaderTemplate>

            <ItemTemplate>
                <div class="completeOrderItem"> 
                    <table> 
                        <tr>
                            <th>
                                <b>Order Status: <%#Eval("OrderStatus") %></b>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrderID" runat="server" Text="Order ID"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("OrderID") %>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblOrderDateTime" runat="server" Text="Order Date Time"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("OrderDate") %>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblPaymentID" runat="server" Text="Payment ID"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <%#Eval("PaymentID") %>
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: right;">
                        <asp:Button ID="btnMore" runat="server" Text="More Details" CssClass="orderTrackingBtn" CommandArgument='<%# Eval("OrderID") %>' OnClick="btnMore_Click"/>
                    </div>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>


        <br />
    </div>
</asp:Content>


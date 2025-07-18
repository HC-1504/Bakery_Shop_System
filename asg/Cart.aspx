<%@ Page Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="Asg.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="text-center title" style="margin-top: 1%;">Your Shopping Cart</h1>
    <br />

    <asp:GridView ID="GridViewCart" runat="server" AutoGenerateColumns="False" CssClass="cart-table" OnRowCommand="GridViewCart_RowCommand" DataKeyNames="ProductID">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnDeleteProduct" runat="server" CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>' CssClass="btn btn-outline-danger" BorderStyle="None" >
                        <i class="fas fa-times"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <img src='<%# Eval("Image") %>' alt="Product Image" style="width: 100px; height: 100px;" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Name" HeaderText="Product Name" />

            <asp:BoundField DataField="Price" HeaderText="Price (RM)" DataFormatString="RM {0:N2}" />

            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control" Width="50px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Subtotal (RM)">
                <ItemTemplate>
                    RM <%# (Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity"))).ToString("N2") %>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    <div id="cartContainer" class="cart-container" runat="server">
        <div class="container-price">
            <asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:RM 0.00" CssClass="font-weight-bold"></asp:Label><br />
            <asp:Label ID="lblTotal" runat="server" Text="Total:RM 0.00" CssClass="font-weight-bold"></asp:Label><br />
        </div>

        <div class="container-update">
            <asp:Button ID="btnUpdateCart" runat="server" Text="Update Cart" CssClass="userBtn" OnClick="btnUpdateCart_Click" />
        </div>

        <div class="container-check">
            <asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" CssClass="processBtn" OnClick="btnCheckout_Click" />
        </div>
    </div>

    <h2 class="cart-empty">
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btnGoToShopping" runat="server" Text="Go to Shopping" CssClass="shoppingBtn" OnClick="btnGoToShopping_Click" Visible="false" />
    </h2>

</asp:Content>


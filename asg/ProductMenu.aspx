<%@ Page Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="ProductMenu.aspx.cs" Inherits="Asg.ProductMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container">

        <h1 class="text-center title" style="margin-top: 1%;">Product Menu</h1>
        <br />

        <div class="d-flex justify-content-between align-items-center mb-3">
            <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" CssClass="form-control w-25 d-inline-block" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                <asp:ListItem Value="" Text="All Categories" />
                <asp:ListItem Value="Cake" Text="Cake" />
                <asp:ListItem Value="Bread" Text="Bread" />
                <asp:ListItem Value="Tart" Text="Tart" />
                <asp:ListItem Value="Biscuit" Text="Biscuit" />
            </asp:DropDownList>

            <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="True" CssClass="form-control w-25 d-inline-block" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                <asp:ListItem Value="" Text="Sort by: Featured" />
                <asp:ListItem Value="Price_Asc" Text="Price, Low - High" />
                <asp:ListItem Value="Price_Desc" Text="Price, High - Low" />
                <asp:ListItem Value="Alphabet_Asc" Text="Alphabet, A - Z" />
                <asp:ListItem Value="Alphabet_Desc" Text="Alphabet, Z - A" />
                <asp:ListItem Value="Best_Seller" Text="Best Seller" />
            </asp:DropDownList>
        </div>

    </div>

        <div class="table-container">
            <asp:DataList ID="DataListProducts" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="10" OnItemCommand="DataListProducts_ItemCommand">
                <ItemTemplate>
                    <div class="product-card" style="background-color:white;">
                        <img src='<%# Eval("Image") %>' alt="Product Image" class="card-img-top" />
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Name") %></h5>
                            <p class="card-text"><%# Eval("Description") %></p>
                            <p class="card-text"><strong>Category:</strong> <%# Eval("Category") %></p>
                            <p class="card-text"><strong>Price:</strong> RM <%# Eval("UnitPrice", "{0:N2}") %></p>
                            <asp:Button ID="btnViewMore" runat="server" Text="View More" CommandName="ViewMore" CommandArgument='<%# Eval("ProductID") %>' CssClass="userBtn" />
                            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>' CssClass="userBtn" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Product]"></asp:SqlDataSource>

    <div class="modal fade" id="ProductDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ProductDetailsModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ProductDetailsModalLabel">Product Details</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <h5><asp:Label ID="lblProductName" runat="server"></asp:Label></h5>
                    <p><asp:Image ID="imgProductImage" runat="server" CssClass="product-image"/></p>
                    <p><asp:Label ID="lblProductDescription" runat="server"></asp:Label></p>
                    <p><strong>Category: </strong> <asp:Label ID="lblProductCategory" runat="server"></asp:Label></p>
                    <p><strong>Price: </strong> RM <asp:Label ID="lblProductPrice" runat="server"></asp:Label></p>
                    <p><strong>Calories: </strong><asp:Label ID="lblProductCalories" runat="server" Text="Label"></asp:Label></p>
                    <p><strong>Description: </strong><asp:Label ID="lblProductLongDescription" runat="server" Text="Label"></asp:Label></p>
                    <p><strong>Ingredients: </strong><asp:Label ID="lblProductIngredients" runat="server" Text="Label"></asp:Label></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="userBtn" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="AddToCartModal" tabindex="-1" role="dialog" aria-labelledby="AddToCartModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="AddToCartModalLabel">Add to Cart</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <h5><asp:Label ID="lblCartProductName" runat="server"></asp:Label></h5>
                    <p><asp:Image ID="imgCartProductImage" runat="server" CssClass="product-image"/></p>
                    <p><strong>Price: </strong> RM <asp:Label ID="lblCartProductPrice" runat="server"></asp:Label></p>
                    <p><strong>Quantity: </strong> 
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmAddToCart" runat="server" CssClass="addCartBtn" Text="Confirm Add to Cart" OnClick="btnConfirmAddToCart_Click"/>
                    <button type="button" class="userBtn" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>


</asp:Content>


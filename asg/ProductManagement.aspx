<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ProductManagement.aspx.cs" Inherits="Asg.ProductManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br /><br />
    <div class="container">
        <br />
        <br />
        <h1 class="text-center title" style="margin-top: 1%;">Product Management</h1>
        <br />

        <div class="float-right mb-3">
            <asp:Button ID="btnExport" runat="server" style="margin-right: 1px; background:#16a085; color:white;" class="btn btn-sm btn-light" Text="Export to Excel" OnClick="btnExportToExcel_Click" />
            <asp:LinkButton runat="server" ID="btnAddProduct" class="btn btn-sm btn-warning" title="Add Product" CausesValidation="false" data-toggle="modal" data-target="#AddNewModal">
                <i class='fa fa-plus'></i> Add Product
            </asp:LinkButton>

            <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" class="btn btn-sm btn-light" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                <asp:ListItem Value="" Text="All Categories" />
                <asp:ListItem Value="Cake" Text="Cake" />
                <asp:ListItem Value="Bread" Text="Bread" />
                <asp:ListItem Value="Tart" Text="Tart" />
                <asp:ListItem Value="Biscuit" Text="Biscuit" />
            </asp:DropDownList>
        </div>
            
        <div>
            <label for="searchOrder">Search Product:</label>
            <asp:TextBox ID="txtSearchPro" runat="server" placeholder="Enter Product ID or Name" style="width:20%;" ></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" ToolTip="Search" />
        </div>

        <div class="table-container">
            <asp:DataList ID="DataListProducts" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="10" OnItemCommand="DataListProducts_ItemCommand">
                <ItemTemplate>
                    <div class="product-card" style="background-color:white; height: 410px; width: 350px;">
                        <img src='<%# Eval("Image") %>' alt="Product Image" class="card-img-top" />
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Name") %></h5>
                            <p class="card-text"><%# Eval("Description") %></p>
                            <p class="card-text"><strong>Category:</strong> <%# Eval("Category") %></p>
                            <p class="card-text"><strong>Price:</strong> RM <%# Eval("UnitPrice", "{0:N2}") %></p>
                              <asp:LinkButton runat="server" CssClass="btn btn-outline-secondary" BorderStyle="None" CommandName="Edit" Text="<i class='fas fa-edit'></i>" CommandArgument='<%#Eval("ProductID") %>' ToolTip="Edit" />
                              <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" BorderStyle="None" CommandName="Delete" Text="<i class='fas fa-times'></i>" CommandArgument='<%# Eval("ProductID") %>' ToolTip="Delete" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Product]"></asp:SqlDataSource>


        <!-- Edit Product Modal -->
        <div id="EditModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="EditProductModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Product Details</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                    </div>
                    <div class="modal-body">
                        <table class="table">
                            <tr>
                                <td>Product ID</td>
                                <td>
                                    <asp:Label ID="lblEditProductID" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="txtEditProductName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorProductName" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlEditCategory" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Select..." Value="" />
                                        <asp:ListItem Text="Cake" Value="Cake" />
                                        <asp:ListItem Text="Bread" Value="Bread" />
                                        <asp:ListItem Text="Tart" Value="Tart" />
                                        <asp:ListItem Text="Biscuit" Value="Biscuit" />
                                    </asp:DropDownList>
                                    <asp:Label ID="lblErrorCategory" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Price</td>
                                <td>
                                    <asp:TextBox ID="txtEditProductPrice" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorProductPrice" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Description</td>
                                <td>
                                    <asp:TextBox ID="txtEditProductDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorProductDesc" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Long Description</td>
                                <td>
                                    <asp:TextBox ID="txtEditLongDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorLongDesc" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Ingredients</td>
                                <td>
                                    <asp:TextBox ID="txtEditIngre" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorEditIngre" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Calories</td>
                                <td>
                                    <asp:TextBox ID="txtEditCalories" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorEditCalories" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Image</td>
                                <td>
                                    <asp:TextBox ID="txtEditImage" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorImage" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveEdit" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSaveEdit_Click" ToolTip="Save" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>



        <!-- Confirm Delete Modal -->
        <div id="DeleteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="DeleteProductModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Confirm Delete</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                    </div>
                    <div class="modal-body">
                        <p>Are you sure you want to delete this product?</p>
                        <asp:Label ID="lblDeleteProductID" runat="server" CssClass="d-none"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, Delete" CssClass="btn btn-danger" OnClick="btnConfirmDelete_Click" ToolTip="Confirm" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>



        <!-- Add New Product Modal -->
        <div id="AddNewModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="AddNewProductModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add New Product</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                    </div>
                    <div class="modal-body">
                        <table class="table">
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="txtAddProductName" runat="server" CssClass="form-control"></asp:TextBox>
                                      <asp:Label ID="lblErrorAddProductName" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Description</td>
                                <td>
                                    <asp:TextBox ID="txtAddProductDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                      <asp:Label ID="lblErrorAddProductDescription" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlAddCategory" runat="server" CssClass="form-control">
                                          <asp:ListItem Text="Select..." Value="" />
                                          <asp:ListItem Text="Cake" Value="Cake" />
                                          <asp:ListItem Text="Bread" Value="Bread" />
                                          <asp:ListItem Text="Tart" Value="Tart" />
                                          <asp:ListItem Text="Biscuit" Value="Biscuit" />
                                    </asp:DropDownList>
                                    <asp:Label ID="lblErrorAddCategory" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Price</td>
                                <td>
                                    <asp:TextBox ID="txtAddProductPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                      <asp:Label ID="lblErrorAddProductPrice" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Long Description</td>
                                <td>
                                    <asp:TextBox ID="txtAddLongDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorAddLongDesc" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Ingredients</td>
                                <td>
                                    <asp:TextBox ID="txtAddIngre" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorAddIngre" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Calories</td>
                                <td>
                                    <asp:TextBox ID="txtAddCalories" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErroAddCalories" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Image</td>
                                <td>
                                    <asp:TextBox ID="txtAddImage" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblErrorAddImage" runat="server" CssClass="text-danger"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnConfirmAddProduct" runat="server" Text="Add Product" CssClass="btn btn-success" OnClick="btnConfirmAddProduct_Click" ToolTip="Add" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

     <!-- Success Modal (add product) -->
     <div class="modal fade" id="successModalAdd" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                     <h5 class="modal-title" id="successModalLabelAdd">Product Added</h5>
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                     The product has been successfully added.
                </div>
             </div>
         </div>
     </div>


     <!-- Success Modal (delete product) -->
     <div class="modal fade" id="successModalDelete" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
         <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="successModalLabelDelete">Product Deleted</h5>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
             </div>
             <div class="modal-body">
             The product has been successfully deleted.
             </div>
         </div>
         </div>
     </div>

       <!-- Error Modal (delete product) -->
   <div class="modal fade" id="errorModalDelete" tabindex="-1" aria-labelledby="errorModalLabel" aria-hidden="true">
       <div class="modal-dialog">
       <div class="modal-content">
           <div class="modal-header">
               <h5 class="modal-title" id="errorModalLabelDelete" style="color:red;">Error</h5>
               <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
           </div>
           <div class="modal-body">
           This product cannot be deleted because it is associated with existing orders.
           </div>
       </div>
       </div>
   </div>

     <!-- Success Modal (delete product) -->
     <div class="modal fade" id="successModalUpdate" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
         <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header">
             <h5 class="modal-title" id="successModalLabelUpdate">Product Updated</h5>
             <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
             </div>
             <div class="modal-body">
             The product has been successfully updated.
             </div>
         </div>
         </div>
     </div>

</asp:Content>

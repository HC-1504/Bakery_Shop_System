<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="OrderManagement.aspx.cs" Inherits="Asg.OrderManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br /><br />
    <div class="container">
        <br />
        <br />
        <h1 class="text-center title" style="margin-top: 1%;">Order Management</h1>
        <br />

          <div class="float-right mb-3">
              <asp:Button ID="btnExport" runat="server" style="margin-right: 1px; background:#16a085; color:white;" class="btn btn-sm btn-light" Text="Export to Excel" OnClick="btnExportToExcel_Click" />
             
          </div>

         <div>
             <label for="searchStaff">Search Order:</label>
             <asp:TextBox ID="txtSearchOrder" runat="server" placeholder="Enter Order ID or Customer ID or Order Status" Style="width: 20%;"></asp:TextBox>
             <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchOrder_Click"/>
         </div>

        <div class="table-container">
            <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" CssClass="gridview table-light table-striped table-hover table-responsive" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="gvOrder_PageIndexChanging" OnSorting="gvOrder_Sorting">
                <Columns>
                    <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID"></asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="OrderID" SortExpression="OrderID"></asp:BoundField>
                    <asp:BoundField DataField="OrderStatus" HeaderText="OrderStatus" SortExpression="OrderStatus"></asp:BoundField>
                    <asp:BoundField DataField="ProductID" HeaderText="ProductID" SortExpression="ProductID"></asp:BoundField>
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"></asp:BoundField>
                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category"></asp:BoundField>
                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID" SortExpression="PaymentID"></asp:BoundField>
                    <asp:BoundField DataField="PaymentTotal" HeaderText="PaymentTotal" ReadOnly="True" SortExpression="PaymentTotal"></asp:BoundField>
                    <asp:BoundField DataField="PaymentDate" HeaderText="PaymentDate" SortExpression="PaymentDate"></asp:BoundField>
                    <asp:BoundField DataField="PaymentStatus" HeaderText="PaymentStatus" SortExpression="PaymentStatus"></asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CssClass="btn btn-outline-secondary" BorderStyle="None" Text="<i class='fas fa-edit'></i>" CommandArgument='<%#Eval("OrderID") %>' OnClick="btnEdit_OnClick" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>


     <!-- Edit Customer Modal -->
    <div id="EditModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="EditCustModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Order Details</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr>
                            <td>Order ID</td>
                            <td>
                                 <asp:Label ID="lblEditOrderID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer ID</td>
                            <td>
                                <asp:Label ID="lblEditCustID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Product ID</td>
                            <td>
                                 <asp:Label ID="lblEditProductID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Product Name</td>
                            <td>
                                 <asp:Label ID="lblEditProductname" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Quantity</td>
                            <td>
                                 <asp:label ID="lblEditQuantity" runat="server"></asp:label>
                            </td>
                        </tr>
                        <tr>
                            <td>Category</td>
                            <td>
                                <asp:Label ID="lblEditCategory" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment ID</td>
                            <td>
                                 <asp:Label ID="lblEditPaymentID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Total</td>
                            <td>
                                 <asp:Label ID="lblEditPaymentTotal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Date</td>
                            <td>
                                <asp:Label ID="lblEditPaymentDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Status</td>
                            <td>
                                 <asp:Label ID="lblEditPaymentStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Order Status</td>
                            <td>
                                <asp:DropDownList ID="ddlSaveEditOrderStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select..." Value=""></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                    <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                                    <asp:ListItem Text="Processing" Value="Processing"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblErrorOrderStatus" runat="server" CssClass="text-danger"></asp:Label>
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



     <!-- Success Modal -->
     <div class="modal fade" id="successModalUpdate" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
         <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header">
             <h5 class="modal-title" id="successModalLabelUpdate">Order Updated</h5>
             <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
             </div>
             <div class="modal-body">
                The order has been successfully updated.
             </div>
         </div>
         </div>
     </div>

</asp:Content>

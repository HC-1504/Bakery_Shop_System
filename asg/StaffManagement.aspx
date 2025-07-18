<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="StaffManagement.aspx.cs" Inherits="Asg.StaffManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br /><br />
    <div class="container">
        <br />
        <br />
        <h1 class="text-center title" style="margin-top: 1%;">Staff Management</h1>
        <br />
        
         <div class="float-right mb-3">
             <asp:Button ID="btnExport" runat="server" style="margin-right: 1px; background:#16a085; color:white;" class="btn btn-sm btn-light" Text="Export to Excel" OnClick="btnExportToExcel_Click" />
             <asp:LinkButton runat="server" ID="btnAddStaff" class="btn btn-sm btn-warning" title="Add Staff" CausesValidation="false" data-toggle="modal" data-target="#AddNewModal">
                 <i class='fa fa-plus'></i> Add Staff
             </asp:LinkButton>
         </div>

        <div>
            <label for="searchStaff">Search Staff:</label>
            <asp:TextBox ID="txtSearchStaff" runat="server" placeholder="Enter Staff ID or Name" Style="width: 20%;"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchStaff_Click"/>
        </div>

        <div class="table-container">
            <asp:GridView ID="gvStaff" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" CssClass="gridview table-light table-striped table-hover table-responsive" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="gvStaff_PageIndexChanging" OnSorting="gvStaff_Sorting">
                <Columns>
                    <asp:BoundField DataField="AdminID" HeaderText="AdminID" ReadOnly="True" SortExpression="AdminID" />
                    <asp:BoundField DataField="Adminname" HeaderText="Adminname" SortExpression="Adminname" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                    <asp:BoundField DataField="DateOfBirth" HeaderText="DateOfBirth" SortExpression="DateOfBirth" />
                    <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" SortExpression="ContactNo" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CssClass="btn btn-outline-secondary" BorderStyle="None" Text="<i class='fas fa-edit'></i>" CommandArgument='<%# Eval("AdminID") %>' OnClick="btnEdit_OnClick" ToolTip="Edit" />
                            <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" BorderStyle="None" Text="<i class='fas fa-times'></i>" CommandArgument='<%# Eval("AdminID") %>' OnClick="btnDelete_OnClick" ToolTip="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

     <!-- Edit Staff Modal -->
    <div id="EditModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="EditStaffModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Staff Details</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr>
                            <td>Staff ID</td>
                            <td>
                                <asp:Label ID="lblEditStaffID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Name</td>
                            <td>
                                <asp:TextBox ID="txtEditStaffName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorEditStaffName" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorEditEmail" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Contact Number</td>
                            <td>
                                <asp:TextBox ID="txtEditContactNo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorEditContact" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Date of Birth</td>
                            <td>
                                <asp:TextBox ID="txtEditDateOfBirth" runat="server" CssClass="form-control" TextMode="Date" ></asp:TextBox>
                                <asp:Label ID="lblErrorEditDate" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Gender</td>
                            <td>
                                <asp:DropDownList ID="ddlSaveEditGender" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select..." Value="" />
                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblErrorEditGender" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveEdit" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSaveEdit_Click" ToolTip="Save Changes" />
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
                    <p>Are you sure you want to delete this staff?</p>
                    <asp:Label ID="lblDeleteStaffID" runat="server" CssClass="d-none"></asp:Label>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, Delete" CssClass="btn btn-danger" OnClick="btnConfirmDelete_Click" ToolTip="Delete" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>



    <!-- Add New staff Modal -->
    <div id="AddNewModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="AddNewStaffModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Add New Staff</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <table class="table">
                         <tr>
                             <td>Name</td>
                             <td>
                                 <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:Label ID="lblErrorAddName" runat="server" CssClass="text-danger"></asp:Label>
                             </td>
                         </tr>
                         <tr>
                             <td>Email</td>
                             <td>
                                 <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:Label ID="lblErrorAddEmail" runat="server" CssClass="text-danger"></asp:Label>
                             </td>
                         </tr>
                         <tr>
                             <td>Contact Number</td>
                             <td>
                                 <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:Label ID="lblErrorAddContact" runat="server" CssClass="text-danger"></asp:Label>
                             </td>
                         </tr>
                         <tr>
                             <td>Date of Birth</td>
                             <td>
                                 <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" TextMode="Date" ></asp:TextBox>
                                 <asp:Label ID="lblErrorAddDate" runat="server" CssClass="text-danger"></asp:Label>
                             </td>
                         </tr>
                         <tr>
                             <td>Gender</td>
                             <td>
                                 <asp:DropDownList ID="ddlAddGender" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select..." Value="" />
                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                </asp:DropDownList>
                                 <asp:Label ID="lblErrorAddGender" runat="server" CssClass="text-danger"></asp:Label>
                             </td>
                         </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmAddStaff" runat="server" Text="Add Staff" CssClass="btn btn-success" OnClick="btnConfirmAddStaff_Click" ToolTip="Add Staff" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>




 <!-- Success Modal (add staff) -->
 <div class="modal fade" id="successModalAdd" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                 <h5 class="modal-title" id="successModalLabelAdd">Staff Added</h5>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
            </div>
            <div class="modal-body">
                 The staff has been successfully added.
            </div>
         </div>
     </div>
 </div>


 <!-- Success Modal (delete staff) -->
 <div class="modal fade" id="successModalDelete" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
     <div class="modal-dialog">
     <div class="modal-content">
         <div class="modal-header">
             <h5 class="modal-title" id="successModalLabelDelete">Staff Deleted</h5>
             <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
         </div>
         <div class="modal-body">
         The staff has been successfully deleted.
         </div>
     </div>
     </div>
 </div>

 <!-- Success Modal (delete staff) -->
 <div class="modal fade" id="successModalUpdate" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
     <div class="modal-dialog">
     <div class="modal-content">
         <div class="modal-header">
         <h5 class="modal-title" id="successModalLabelUpdate">Staff Updated</h5>
         <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
         </div>
         <div class="modal-body">
         The staff has been successfully updated.
         </div>
     </div>
     </div>
 </div>
</asp:Content>

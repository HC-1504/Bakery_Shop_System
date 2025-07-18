<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="Asg.AdminProfile" %>


<asp:Content ID="AccountSettingContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <br /><br />
    <h1 class="text-center title" style="margin-top: 1%;">Account Setting</h1>       

    <div class="accountDiv">
        <div class="left">
           <asp:SqlDataSource ID="AccSettingLeftDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [AdminID], [Adminname] FROM [Admin] WHERE ([AdminID] = @AdminID)">
                <SelectParameters>
                    <asp:SessionParameter SessionField="AdminID" Name="AdminID" Type="String"></asp:SessionParameter>
                </SelectParameters>
            </asp:SqlDataSource>


            <asp:Repeater ID="rptLeftDiv" runat="server" DataSourceID="AccSettingLeftDataSource">
                <ItemTemplate>
                    <br />
                    <div style="text-align:center;"><b>Admin ID: </b><%#Eval("AdminID") %></div>
                    <h2 style="text-align:center;">Hello, <%#Eval("Adminname") %>!</h2>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="right">
           <asp:SqlDataSource ID="AccSettingDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [AdminID], [Adminname], [Email], [ContactNo], [DateOfBirth], [Gender] FROM [Admin] WHERE ([AdminID] = @AdminID)">
                <SelectParameters>
                    <asp:SessionParameter SessionField="AdminID" Name="AdminID" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

            <div class="table-container">
                <asp:DetailsView ID="AccSettingDetailsView" runat="server" CssClass="AccSettingTable" AutoGenerateRows="False" DataKeyNames="AdminID" DataSourceID="AccSettingDataSource">
                    <Fields>
                        <asp:BoundField DataField="Adminname" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="ContactNo" HeaderText="Contact No" />
                        <asp:BoundField DataField="DateOfBirth" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Date of Birth" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CssClass="btn btn-outline-secondary" BorderStyle="None" Text="<i class='fas fa-edit'></i>" CommandArgument='<%# Eval("AdminID") %>' OnClick="btnEdit_OnClick" ToolTip="Edit" />
                                <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" BorderStyle="None" Text="<i class='fas fa-times'></i>" CommandArgument='<%# Eval("AdminID") %>' OnClick="btnDelete_OnClick" ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
            </div>

            <div class="modal-btn-container">
                <asp:Button ID="btnChangePass" runat="server" class="btn btn-secondary mt-3" OnClick="btnChange_OnClick" Text="Change Password" /> &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSecurityQu" runat="server" class="btn btn-secondary mt-3" OnClick="btnSecurityQu_OnClick" Text="Change Security Question" />
            </div>

        </div>
    </div>

    <!-- Edit Modal -->
    <div id="EditModal" class="modal fade" tabindex="-1" aria-labelledby="EditModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Profile Details</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr>
                            <td>Admin ID</td>
                            <td>
                                <asp:Label ID="lblEditAdminID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Name</td>
                            <td>
                                <asp:TextBox ID="txtEditAdminName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorAdminName" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <asp:TextBox ID="txtEditAdminEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorAdminEmail" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Contact No</td>
                            <td>
                                <asp:TextBox ID="txtEditAdminContact" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorAdminContact" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Date of Birth</td>
                            <td>
                                <asp:TextBox ID="txtEditAdminDOB" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="lblErrorAdminDOB" runat="server" CssClass="text-danger"></asp:Label>
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
                    <asp:Button ID="btnSaveEdit" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSaveEdit_Click" ToolTip="Save" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" ToolTip="Cancel">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Delete Modal -->
    <div id="DeleteModal" class="modal fade" tabindex="-1" aria-labelledby="DeleteModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Confirm Delete</h5>
                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Are you sure you want to delete your account?</p>
                    <asp:Label ID="lblDeleteAdminID" runat="server" CssClass="d-none"></asp:Label>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, Delete" CssClass="btn btn-danger" OnClick="btnConfirmDelete_Click" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Change Password Modal -->
    <div id="ChangePasswordModal" class="modal fade" tabindex="-1" aria-labelledby="ChangePasswordModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Change Password</h5>
                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr>
                            <td>Current Password</td>
                            <td>
                                <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:Label ID="lblErrorCurrentPassword" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>New Password</td>
                            <td>
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:Label ID="lblErrorNewPassword" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Confirm New Password</td>
                            <td>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:Label ID="lblErrorConfirmPassword" runat="server" CssClass="text-danger"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSubmitPassword" runat="server" Text="Change Password" CssClass="btn btn-primary" OnClick="btnSubmitPassword_Click" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>


    <!-- Success Modal -->
    <div class="modal fade" id="successModalDelete" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
        <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="successModalLabelDelete">Admin Deleted</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
            </div>
            <div class="modal-body">
            Your account has been successfully deleted.
            </div>
        </div>
        </div>
    </div>

    <!-- Success Modal -->
    <div class="modal fade" id="successModalUpdate" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
        <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title" id="successModalLabelUpdate">Admin Updated</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
            </div>
            <div class="modal-body">
            Your details has been successfully updated.
            </div>
        </div>
        </div>
    </div>

  <!-- Success Modal --> 
    <div class="modal fade" id="successModalChangePass" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="successModalLabelChangePass">Password Updated</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    Password has been updated successfully.
                </div>
            </div>
        </div>
    </div>

    <!-- Error Modal -->
    <div id="errorModalDelete" class="modal fade" tabindex="-1" aria-labelledby="errorModalDeleteLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title" id="errorModalDeleteLabel">Error</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
                </div>
                <div class="modal-body">
                    <p>An error occurred while trying to delete your account. Please try again later or contact support if the issue persists.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Change Password Modal -->
<div id="ChangeSecurityModal" class="modal fade" tabindex="-1" aria-labelledby="ChangeSecurityModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Change Security Question</h5>
               <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
            </div>
            <div class="modal-body">
                <table class="table">
                    <tr>
                        <td>Please Choose a Security Question</td>
                        <td>
                            <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form-control">
                               <asp:ListItem Text="Select..." Value="" />
                               <asp:ListItem Text="Movie" Value="Movie"></asp:ListItem>
                               <asp:ListItem Text="Nickname" Value="Nickname"></asp:ListItem>
                               <asp:ListItem Text="School" Value="School"></asp:ListItem>
                               <asp:ListItem Text="City" Value="City"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblSecurityQuestion" runat="server" CssClass="text-danger"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Please key in the Security Answer</td>
                        <td>
                            <asp:TextBox ID="txtSecurityAns" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="lblSecurityAns" runat="server" CssClass="text-danger"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSubmitSecurity" runat="server" Text="Change Security Question" CssClass="btn btn-primary" OnClick="btnSubmitSecurity_Click" />
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>
</div>

    <!-- Success Modal --> 
  <div class="modal fade" id="successModalChangeSec" tabindex="-1" aria-labelledby="successModalLabel" aria-hidden="true">
      <div class="modal-dialog">
          <div class="modal-content">
              <div class="modal-header">
                  <h5 class="modal-title" id="successModalLabelChangePass">Security Question Updated</h5>
                  <button type="button" class="close" data-dismiss="modal" aria-label="Close">&times;</button>
              </div>
              <div class="modal-body">
                  Security Question has been updated successfully.
              </div>
          </div>
      </div>
  </div>
</asp:Content>

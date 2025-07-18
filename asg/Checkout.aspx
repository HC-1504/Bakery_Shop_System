<%@ Page Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Asg.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        
        <h1 class="text-center title" style="margin-top: 1%;">Checkout</h1>
        <br />

        <div class="row">
            <!-- Left side: Collect user information -->
            <div class="col-md-6">
                <h3>Collection Information</h3>
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                <div class="form-group">
                    <label for="txtCollectionDate">Collection Date</label>
                    <asp:TextBox ID="txtCollectionDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtCollectionTime">Collection Time</label>
                    <asp:TextBox ID="txtCollectionTime" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtRemark">Remark</label>
                    <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>

                <div class="form-group">
                    <p>
                        Your personal data will be used to process your order, support your experience throughout this website, and for other purposes described in our privacy policy.
                    </p>
                </div>

                <div class="form-check">
                    <asp:CheckBox ID="chkAgreeTerms" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label" for="chkAgreeTerms">
                        I have read and agree to the website terms and conditions *
                    </label>
                </div>
                <asp:Label ID="lblTermsError" runat="server" CssClass="text-danger"></asp:Label>
                <br />
                <asp:Button ID="btnSubmitOrder" runat="server" Text="Place Order" CssClass="userBtn" OnClick="btnSubmitOrder_Click" />
            </div>

            <!-- Right side: Order summary -->
            <div class="col-md-6">
                <h3>Order Summary</h3>
                <asp:GridView ID="GridViewSummary" runat="server" AutoGenerateColumns="False" CssClass="summary-table">
                    <Columns>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <img src='<%# Eval("Image") %>' alt="Product Image" style="width: 100px; height: 100px;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Product Name" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal (RM)" DataFormatString="RM {0:N2}" />
                    </Columns>
                </asp:GridView>
                <div class="summary-price">
                <asp:Label ID="lblOrderTotal" runat="server" CssClass="font-weight-bold"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>



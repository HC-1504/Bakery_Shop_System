using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asg
{
    public partial class OrderTracking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"] || Session["CustomerID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                ddlCategory.SelectedValue = "All";
                rptProcessingOrder.Visible = true;
                rptPickUpOrder.Visible = true;
                rptCompleteOrder.Visible = true;
            }
        }


        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = ddlCategory.SelectedValue;

            switch (selectedCategory)
            {
                case "All":
                    rptProcessingOrder.Visible = true;
                    rptPickUpOrder.Visible = true;
                    rptCompleteOrder.Visible = true;
                    break;
                case "Processing":
                    rptProcessingOrder.Visible = true;
                    rptPickUpOrder.Visible = false;
                    rptCompleteOrder.Visible = false;
                    break;
                case "Pick up":
                    rptProcessingOrder.Visible = false;
                    rptPickUpOrder.Visible = true;
                    rptCompleteOrder.Visible = false;
                    break;
                case "Completed":
                    rptProcessingOrder.Visible = false;
                    rptPickUpOrder.Visible = false;
                    rptCompleteOrder.Visible = true;
                    break;

            }
        }

        protected void rptProcessingOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Response.Redirect("OrderDetail.aspx");
        }
        protected void rptPickUpOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Response.Redirect("OrderDetail.aspx");
        }
        protected void rptCompleteOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Response.Redirect("OrderDetail.aspx");
        }

        protected void btnMore_Click(object sender, EventArgs e)
        {
            // Retrieve the OrderID from the CommandArgument property of the clicked button
            Button btn = (Button)sender;
            string orderID = btn.CommandArgument;

            // Store the OrderID in a session variable
            Session["OrderID"] = orderID;

            // Redirect to the details page
            Response.Redirect("OrderDetail.aspx");
        }
    }
}
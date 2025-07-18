using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asg
{
    public partial class Cart : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"] || Session["CustomerID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                BindCartData();
            }

        }

        private void BindCartData()
        {
            DataTable cart = Session["Cart"] as DataTable;

            if (cart != null && cart.Rows.Count > 0)
            {
                foreach (DataRow row in cart.Rows)
                {
                    decimal price = Convert.ToDecimal(row["Price"]);
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    row["Subtotal"] = price * quantity;
                }

                GridViewCart.DataSource = cart;
                GridViewCart.DataBind();

                decimal subtotal = 0;
                foreach (DataRow row in cart.Rows)
                {
                    subtotal += Convert.ToDecimal(row["Subtotal"]);
                }

                Session["TotalPrice"] = subtotal;

                lblSubtotal.Text = $"Subtotal: RM {subtotal:N2}";
                lblTotal.Text = $"Total: RM {subtotal:N2}";

                // Show cart container and buttons
                cartContainer.Visible = true;
                btnGoToShopping.Visible = false;
            }
            else
            {                
                lblMessage.Text = "Your cart is empty!";

                // Hide cart container and buttons
                GridViewCart.Visible = false;
                cartContainer.Visible = false;
                btnGoToShopping.Visible = true;
            }
        }

        protected void GridViewCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                string productId = e.CommandArgument.ToString();

                DataTable cart = Session["Cart"] as DataTable;
                if (cart != null)
                {
                    DataRow rowToDelete = cart.AsEnumerable().FirstOrDefault(row => row["ProductID"].ToString() == productId);
                    if (rowToDelete != null)
                    {
                        cart.Rows.Remove(rowToDelete);
                    }

                    Session["Cart"] = cart;
                    BindCartData();
                }
            }
        }


        protected void GridViewCart_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCart.EditIndex = e.NewEditIndex;
            BindCartData();
        }

        protected void GridViewCart_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCart.EditIndex = -1;
            BindCartData();
        }

        protected void GridViewCart_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable cart = Session["Cart"] as DataTable;
            if (cart != null)
            {
                GridViewRow row = GridViewCart.Rows[e.RowIndex];
                int productID = Convert.ToInt32(GridViewCart.DataKeys[e.RowIndex].Value);
                TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;

                DataRow[] rows = cart.Select($"ProductID = '{productID}'");
                if (rows.Length > 0)
                {
                    rows[0]["Quantity"] = Convert.ToInt32(txtQuantity.Text);
                    rows[0]["Subtotal"] = Convert.ToDecimal(rows[0]["Price"]) * Convert.ToInt32(rows[0]["Quantity"]);
                }

                GridViewCart.EditIndex = -1;
                Session["Cart"] = cart;
                BindCartData();
            }
        }

        protected void btnSaveQuantity_Click(object sender, EventArgs e)
        {
            DataTable cart = Session["Cart"] as DataTable;
            if (cart != null)
            {
                foreach (GridViewRow row in GridViewCart.Rows)
                {
                    string productId = GridViewCart.DataKeys[row.RowIndex]["ProductID"].ToString();
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

                    DataRow cartRow = cart.AsEnumerable().FirstOrDefault(r => r["ProductID"].ToString() == productId);
                    if (cartRow != null)
                    {
                        cartRow["Quantity"] = Convert.ToInt32(txtQuantity.Text);
                    }
                }

                Session["Cart"] = cart;
                BindCartData();
            }
        }

        protected void btnUpdateCart_Click(object sender, EventArgs e)
        {
            DataTable cart = Session["Cart"] as DataTable;
            if (cart != null)
            {
                foreach (GridViewRow row in GridViewCart.Rows)
                {
                    string productId = GridViewCart.DataKeys[row.RowIndex]["ProductID"].ToString();
                    TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;

                    if (txtQuantity != null && int.TryParse(txtQuantity.Text, out int quantity) && quantity > 0)
                    {
                        DataRow cartRow = cart.AsEnumerable().FirstOrDefault(r => r["ProductID"].ToString() == productId);
                        if (cartRow != null)
                        {
                            cartRow["Quantity"] = quantity;

                            // Calculate and update Subtotal
                            decimal price = Convert.ToDecimal(cartRow["Price"]);
                            cartRow["Subtotal"] = price * quantity;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Invalid quantity for one or more products.";
                    }
                }

                Session["Cart"] = cart;
                BindCartData();
            }
            else
            {
                
                lblMessage.Text = "Your cart is empty!";

                // Hide cart container and buttons
                GridViewCart.Visible = false;
                cartContainer.Visible = false;
                btnGoToShopping.Visible = true;
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Checkout.aspx");
        }

        protected void btnGoToShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductMenu.aspx");
        }

    }
}


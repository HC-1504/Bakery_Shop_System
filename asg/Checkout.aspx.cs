using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Asg
{
    public partial class Checkout : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"])
            {
                BindOrderSummary();
            }
            else
            {
                // Redirect to login page if the user is not logged in
                Response.Redirect("Login.aspx");
            }
        }

        private void BindOrderSummary()
        {
            DataTable cart = Session["Cart"] as DataTable;

            if (cart != null && cart.Rows.Count > 0)
            {
                GridViewSummary.DataSource = cart;
                GridViewSummary.DataBind();

                // Calculate total
                decimal total = 0;
                if (Session["TotalPrice"] != null)
                {
                    total = Convert.ToDecimal(Session["TotalPrice"]);
                }

                lblOrderTotal.Text = $"Total: RM {total:N2}";
            }
            else
            {
                lblMessage.Text = "Your cart is empty!";
                GridViewSummary.DataSource = null;
                GridViewSummary.DataBind();
            }
        }

        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            string collectionDate = txtCollectionDate.Text;
            string collectionTime = txtCollectionTime.Text;
            string remark = txtRemark.Text;
            lblTermsError.Text = "";
            lblMessage.Text = "";

            if (!chkAgreeTerms.Checked)
            {
                lblTermsError.Text = "You must agree to the terms and conditions before placing your order.";
                return;
            }

            if (string.IsNullOrEmpty(collectionDate) || string.IsNullOrEmpty(collectionTime))
            {
                lblMessage.Text = "Please fill in the required fields (Collection Date and Time).";
                return;
            }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    string getLastID = "SELECT TOP 1 OrderID FROM [Order] ORDER BY OrderID DESC";
                    string newOrderID;
                    conn.Open();

                    using (SqlCommand getCmd = new SqlCommand(getLastID, conn))
                    {
                        object lastIdObj = getCmd.ExecuteScalar();
                        if (lastIdObj != null)
                        {
                            string lastId = lastIdObj.ToString();
                            // Extract the numeric part, increment it, and format it
                            int numPart = int.Parse(lastId.Substring(1));
                            newOrderID = $"O{(numPart + 1):D5}";
                        }
                        else
                        {
                           
                            newOrderID = "O00001";
                        }
                    }


                    string customerId = Session["CustomerID"].ToString();

                    DateTime pickupDate = DateTime.Parse($"{collectionDate} {collectionTime}");


                    string query = "INSERT INTO [Order] (OrderID, CustomerID, OrderDate, OrderStatus, PickupDate, Remark) " +
                                   "VALUES (@OrderID, @CustomerID, @OrderDate, @OrderStatus, @PickupDate, @Remark)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", newOrderID);
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);
                        cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OrderStatus", "Pending");
                        cmd.Parameters.AddWithValue("@PickupDate", pickupDate);
                        if (string.IsNullOrEmpty(remark))
                        {
                            cmd.Parameters.AddWithValue("@Remark", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Remark", remark);
                        }


                        cmd.ExecuteNonQuery();
                    }

                    string query2 = "INSERT INTO OrderProduct (OrderID, ProductID, Quantity, SubPrice) " +
                                    "VALUES (@OrderID, @ProductID, @Quantity, @SubPrice)";
                DataTable cart = Session["Cart"] as DataTable;

                foreach (DataRow row in cart.Rows) {
                    using (SqlCommand cmd = new SqlCommand(query2, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", newOrderID);
                        cmd.Parameters.AddWithValue("@ProductID", row["ProductID"]);
                        cmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                        cmd.Parameters.AddWithValue("@SubPrice", row["Subtotal"]);


                        cmd.ExecuteNonQuery();
                    }
                }

            }

                Session["Cart"] = null;

                Response.Redirect("Payment.aspx");
        
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Cart.aspx");
        }

    }
}





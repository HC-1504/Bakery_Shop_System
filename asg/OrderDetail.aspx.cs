using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;

namespace asg
{
    public partial class OrderDetail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"] || Session["CustomerID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            // Ensure the OrderID exists
            if (!IsPostBack)
            {
                if (Session["OrderID"] != null)
                {
                    string orderId = Session["OrderID"].ToString();
                    LoadOrderDetails(orderId);
                    LoadOrderProducts(orderId);
                }
                else
                {
                    Response.Redirect("OrderTracking.aspx");
                }
            }
        }

        private void LoadOrderDetails(string orderId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT O.OrderID, O.OrderDate, O.OrderStatus, P.PaymentID, P.TotalPrice
            FROM [Order] O
            LEFT JOIN [Payment] P ON O.OrderID = P.OrderID
            WHERE O.OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblOrderID.Text = reader["OrderID"].ToString();
                    lblOrderStatus.Text = reader["OrderStatus"].ToString();
                    lblOrderDate.Text = Convert.ToDateTime(reader["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    lblPaymentID.Text = reader["PaymentID"].ToString();
                    lblTotalPrice.Text = "RM" + Convert.ToDecimal(reader["TotalPrice"]).ToString("0.00");
                }
                reader.Close();
            }
        }

        private void LoadOrderProducts(string orderId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT OP.Quantity, OP.SubPrice, P.Name, P.Image
            FROM [OrderProduct] OP
            INNER JOIN [Product] P ON OP.ProductID = P.ProductID
            WHERE OP.OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                rptOrderProducts.DataSource = dt;
                rptOrderProducts.DataBind();
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderTracking.aspx");
        }
    }
}

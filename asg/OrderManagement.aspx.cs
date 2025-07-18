using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Asg
{
    public partial class OrderManagement : System.Web.UI.Page
    {
        // Define the connection string
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] == null || (bool)Session["IsLoggedIn"] == false)
            {
                // Redirect to login page if the user is not logged in
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadOrderData();
                }            
            }            
        }

        private void LoadOrderData(string sortExpression = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT c.CustomerID, o.OrderID, o.OrderStatus AS OrderStatus, p.PaymentID AS PaymentID, CAST(p.TotalPrice AS DECIMAL(10, 2)) AS PaymentTotal,p.PaymentDate, p.PaymentStatus AS PaymentStatus, op.ProductID, op.Quantity AS Quantity, pr.Category FROM [dbo].[Customer] c JOIN [dbo].[Order] o ON c.CustomerID = o.CustomerID JOIN [dbo].[Payment] p ON o.OrderID = p.OrderID JOIN [dbo].[OrderProduct] op ON o.OrderID = op.OrderID JOIN [dbo].[Product] pr ON op.ProductID = pr.ProductID";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(sortExpression))
                    {
                        DataView dv = dt.DefaultView;
                        dv.Sort = $"{sortExpression} {SortDirection}";
                        gvOrder.DataSource = dv;
                    }
                    else
                    {
                        gvOrder.DataSource = dt;
                    }
                }
                else
                {
                    gvOrder.DataSource = null;
                }

                gvOrder.DataBind();
            }
        }

        protected void gvOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrder.PageIndex = e.NewPageIndex;
            LoadOrderData(); // Reload data for the new page
        }

        protected void gvOrder_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Toggle sort direction
            SortDirection = SortDirection == "ASC" ? "DESC" : "ASC";
            LoadOrderData(e.SortExpression); // Reload data with sorting
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        protected void btnSearchOrder_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearchOrder.Text.Trim();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                                SELECT 
                                    c.CustomerID, 
                                    o.OrderID, 
                                    o.OrderStatus AS OrderStatus, 
                                    p.PaymentID AS PaymentID, 
                                    CAST(p.TotalPrice AS DECIMAL(10, 2)) AS PaymentTotal, 
                                    p.PaymentDate, 
                                    p.PaymentStatus AS PaymentStatus, 
                                    op.ProductID, 
                                    op.Quantity AS Quantity, 
                                    pr.Category 
                                FROM 
                                    [dbo].[Customer] c 
                                JOIN 
                                    [dbo].[Order] o ON c.CustomerID = o.CustomerID 
                                JOIN 
                                    [dbo].[Payment] p ON o.OrderID = p.OrderID 
                                JOIN 
                                    [dbo].[OrderProduct] op ON o.OrderID = op.OrderID 
                                JOIN 
                                    [dbo].[Product] pr ON op.ProductID = pr.ProductID 
                                WHERE 
                                    o.OrderID LIKE @Search 
                                    OR c.CustomerID LIKE @Search 
                                    OR o.OrderStatus LIKE @Search";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + searchValue + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvOrder.DataSource = dt;
                gvOrder.DataBind();
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT c.CustomerID, o.OrderID, o.OrderStatus AS OrderStatus, p.PaymentID AS PaymentID, CAST(p.TotalPrice AS DECIMAL(10, 2)) AS PaymentTotal, p.PaymentDate, p.PaymentStatus AS PaymentStatus, op.ProductID, op.Quantity AS Quantity, pr.Category FROM[dbo].[Customer] c JOIN[dbo].[Order] o ON c.CustomerID = o.CustomerID JOIN[dbo].[Payment] p ON o.OrderID = p.OrderID JOIN[dbo].[OrderProduct] op ON o.OrderID = op.OrderID JOIN[dbo].[Product] pr ON op.ProductID = pr.ProductID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=OrderData.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        GridView gridView = new GridView();
                        gridView.DataSource = dt;
                        gridView.DataBind();
                        gridView.RenderControl(hw);

                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }

     
        protected void btnEdit_OnClick(object sender, EventArgs e)
        {
            string custID = (sender as LinkButton)?.CommandArgument;
            if (!string.IsNullOrEmpty(custID))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#EditModal').modal('show');", true);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT c.CustomerID, o.OrderID, o.OrderStatus AS OrderStatus, p.PaymentID AS PaymentID, CAST(p.TotalPrice AS DECIMAL(10, 2)) AS PaymentTotal,p.PaymentDate, p.PaymentStatus AS PaymentStatus, op.ProductID, op.Quantity AS Quantity, pr.Category, pr.Name AS ProductName FROM [dbo].[Customer] c JOIN [dbo].[Order] o ON c.CustomerID = o.CustomerID JOIN [dbo].[Payment] p ON o.OrderID = p.OrderID JOIN [dbo].[OrderProduct] op ON o.OrderID = op.OrderID JOIN [dbo].[Product] pr ON op.ProductID = pr.ProductID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CustomerID", custID);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblEditOrderID.Text = reader["OrderID"].ToString();
                        lblEditCustID.Text = reader["CustomerID"].ToString();
                        lblEditProductID.Text = reader["ProductID"].ToString();
                        lblEditProductname.Text = reader["ProductName"].ToString();
                        lblEditQuantity.Text = reader["Quantity"].ToString();
                        lblEditCategory.Text = reader["Category"].ToString();
                        lblEditPaymentID.Text = reader["PaymentID"].ToString();
                        lblEditPaymentTotal.Text = reader["PaymentTotal"].ToString();
                        lblEditPaymentDate.Text = reader["PaymentDate"].ToString();
                        lblEditPaymentStatus.Text = reader["PaymentStatus"].ToString();
                        ddlSaveEditOrderStatus.SelectedValue = reader["OrderStatus"].ToString();
                    }
                    con.Close();
                }
            }
        }


        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblErrorOrderStatus.Text = "";


            // Validation checks
            if (string.IsNullOrWhiteSpace(ddlSaveEditOrderStatus.SelectedValue))
            {
                lblErrorOrderStatus.Text = "Order status is required.";
                isValid = false;
            }

            if (!isValid)
            {
                // Use ScriptManager to re-trigger the modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "$('#EditModal').modal('show');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE [Order] SET OrderStatus = @OrderStatus WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OrderID", lblEditOrderID.Text);
                cmd.Parameters.AddWithValue("@OrderStatus", ddlSaveEditOrderStatus.SelectedValue);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "$('#successModalUpdate').modal('show');", true);
                LoadOrderData();
            }
        }


    }
}
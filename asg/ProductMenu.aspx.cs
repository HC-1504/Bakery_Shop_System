
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asg
{
    public partial class ProductMenu : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"] || Session["CustomerID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                BindProductData();
            }
        }

        private void BindProductData(string category = "", string sortOption = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Product";

                // Add WHERE clause if a category is selected
                if (!string.IsNullOrEmpty(category))
                {
                    query += " WHERE Category = @Category";
                }

                // Add ORDER BY clause based on the selected sort option
                switch (sortOption)
                {
                    case "Price_Asc":
                        query += " ORDER BY UnitPrice ASC";
                        break;
                    case "Price_Desc":
                        query += " ORDER BY UnitPrice DESC";
                        break;
                    case "Alphabet_Asc":
                        query += " ORDER BY Name ASC";
                        break;
                    case "Alphabet_Desc":
                        query += " ORDER BY Name DESC";
                        break;
                    case "Best_Seller":
                        query = @"
                    SELECT p.ProductID, p.Name, p.Description, p.Category, p.UnitPrice, p.Image, ISNULL(SUM(op.Quantity), 0) AS TotalSold
                    FROM Product p
                    LEFT JOIN OrderProduct op ON p.ProductID = op.ProductID";

                        // Include filtering in the subquery if category is selected
                        if (!string.IsNullOrEmpty(category))
                        {
                            query += " WHERE p.Category = @Category";
                        }

                        query += " GROUP BY p.ProductID, p.Name, p.Description, p.Category, p.UnitPrice, p.Image ORDER BY TotalSold DESC";
                        break;
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(category))
                    {
                        cmd.Parameters.AddWithValue("@Category", category);
                    }

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataListProducts.DataSource = reader;
                    DataListProducts.DataBind();
                }
            }
        }


        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = ddlCategory.SelectedValue;
            string selectedSort = ddlSort.SelectedValue;


            BindProductData(selectedCategory, selectedSort);
        }

        protected void DataListProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ViewMore")
            {
                string productId = e.CommandArgument.ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Product WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {

                            lblProductName.Text = reader["Name"].ToString();
                            imgProductImage.ImageUrl = reader["Image"].ToString();
                            lblProductDescription.Text = reader["Description"].ToString();
                            lblProductCategory.Text = reader["Category"].ToString();
                            lblProductPrice.Text = Convert.ToDecimal(reader["UnitPrice"]).ToString("F2");
                            lblProductCalories.Text = reader["Calories"].ToString();
                            lblProductLongDescription.Text = reader["LongDescription"].ToString();
                            lblProductIngredients.Text = reader["Ingredient"].ToString();

                            // Show the modal using JavaScript
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ProductDetailsModal').modal('show');", true);
                        }
                    }
                }
            }

            else if (e.CommandName == "AddToCart")
            {
                string productId = e.CommandArgument.ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Product WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populate the modal with product details
                            lblCartProductName.Text = reader["Name"].ToString();
                            imgCartProductImage.ImageUrl = reader["Image"].ToString();
                            lblCartProductPrice.Text = Convert.ToDecimal(reader["UnitPrice"]).ToString("F2");
                            txtQuantity.Text = "1"; // Default quantity

                            // Store the product ID in a hidden field for later use
                            ViewState["SelectedProductID"] = productId;

                            // Show the modal using JavaScript
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#AddToCartModal').modal('show');", true);
                        }
                    }
                }
            }


        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSort = ddlSort.SelectedValue;
            string selectedCategory = ddlCategory.SelectedValue;

            BindProductData(selectedCategory, selectedSort);
        }

        protected void btnConfirmAddToCart_Click(object sender, EventArgs e)
        {
            string productId = ViewState["SelectedProductID"]?.ToString();
            int quantity = int.TryParse(txtQuantity.Text, out int q) ? q : 1; // Default to 1 if parsing fails

            DataTable cart = Session["Cart"] as DataTable ?? CreateCartDataTable();

            DataRow existingRow = cart.AsEnumerable().FirstOrDefault(row => row["ProductID"].ToString() == productId);

            if (existingRow != null)
            {
                existingRow["Quantity"] = Convert.ToInt32(existingRow["Quantity"]) + quantity;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Product WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            DataRow newRow = cart.NewRow();
                            newRow["ProductID"] = reader["ProductID"];
                            newRow["Name"] = reader["Name"];
                            newRow["Price"] = reader["UnitPrice"];
                            newRow["Quantity"] = quantity;
                            newRow["Image"] = reader["Image"];
                            cart.Rows.Add(newRow);
                        }
                    }
                }
            }

            Session["Cart"] = cart;
        }


        private DataTable CreateCartDataTable()
        {
            DataTable cart = new DataTable();
            cart.Columns.Add("ProductID", typeof(string));
            cart.Columns.Add("Name", typeof(string));
            cart.Columns.Add("Price", typeof(decimal));
            cart.Columns.Add("Quantity", typeof(int));
            cart.Columns.Add("Image", typeof(string));
            cart.Columns.Add("Subtotal", typeof(decimal));
            return cart;
        }

    }
}
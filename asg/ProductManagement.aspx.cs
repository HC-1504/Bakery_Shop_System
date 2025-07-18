using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asg
{
    public partial class ProductManagement : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

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
                    BindProductData();
                }
            }            
        }

        private void BindProductData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Product";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = selectedCategory == ""
                    ? "SELECT * FROM Product"
                    : "SELECT * FROM Product WHERE Category = @Category";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (selectedCategory != "")
                    {
                        cmd.Parameters.AddWithValue("@Category", selectedCategory);
                    }

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataListProducts.DataSource = reader;
                    DataListProducts.DataBind();
                }
            }
        }

        protected void DataListProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string productId = e.CommandArgument.ToString();

            if (e.CommandName == "Edit")
            {
                PopulateEditModal(productId);
            }
            else if (e.CommandName == "Delete")
            {
                lblDeleteProductID.Text = productId;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowDeleteModal", "$('#DeleteModal').modal('show');", true);
            }
        }

        private void PopulateEditModal(string productId)
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
                        lblEditProductID.Text = reader["ProductID"].ToString();
                        txtEditProductName.Text = reader["Name"].ToString();
                        ddlEditCategory.SelectedValue = reader["Category"].ToString();
                        txtEditProductPrice.Text = reader["UnitPrice"].ToString();
                        txtEditProductDescription.Text = reader["Description"].ToString();
                        txtEditLongDesc.Text = reader["LongDescription"].ToString();
                        txtEditIngre.Text = reader["Ingredient"].ToString();
                        txtEditCalories.Text = reader["Calories"].ToString();
                        txtEditImage.Text = reader["Image"].ToString();
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "$('#EditModal').modal('show');", true);
                    }
                }
            }
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblErrorProductName.Text = "";
            lblErrorCategory.Text = "";
            lblErrorProductPrice.Text = "";
            lblErrorProductDesc.Text = "";
            lblErrorLongDesc.Text = "";
            lblErrorEditIngre.Text = "";
            lblErrorEditCalories.Text = "";
            lblErrorImage.Text = "";


            // Validation checks
            if (string.IsNullOrWhiteSpace(txtEditProductName.Text))
            {
                lblErrorProductName.Text = "Product name is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEditProductDescription.Text))
            {
                lblErrorProductDesc.Text = "Product's description is required.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(ddlEditCategory.SelectedValue))
            {
                lblErrorCategory.Text = "Please select a valid category.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEditProductPrice.Text) || !decimal.TryParse(txtEditProductPrice.Text, out decimal price) || price <= 0)
            {
                lblErrorProductPrice.Text = "Please enter a valid price greater than 0.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEditLongDesc.Text))
            {
                lblErrorLongDesc.Text = "Product's long description is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEditIngre.Text))
            {
                lblErrorEditIngre.Text = "Product's ingredients is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEditCalories.Text))
            {
                lblErrorEditCalories.Text = "Product's calories is required.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtEditImage.Text))
            {
                lblErrorImage.Text = "Image's path location is required.";
                isValid = false;
            }

            // If validation fails, show the modal again
            if (!isValid)
            {
                // Use ScriptManager to re-trigger the modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "$('#EditModal').modal('show');", true);
                return;
            }

            // Proceed with saving to the database if valid
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Product SET Name = @Name, Description = @Description, Category = @Category, UnitPrice = @UnitPrice, Image = @Image, LongDescription = @LongDescription, Ingredient = @Ingredient, Calories = @Calories WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", lblEditProductID.Text);
                    cmd.Parameters.AddWithValue("@Name", txtEditProductName.Text);
                    cmd.Parameters.AddWithValue("@Category", ddlEditCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@UnitPrice", txtEditProductPrice.Text);
                    cmd.Parameters.AddWithValue("@Image", txtEditImage.Text);
                    cmd.Parameters.AddWithValue("@Description", txtEditProductDescription.Text);
                    cmd.Parameters.AddWithValue("@LongDescription", txtEditLongDesc.Text);
                    cmd.Parameters.AddWithValue("@Ingredient", txtEditIngre.Text);
                    cmd.Parameters.AddWithValue("@Calories", txtEditCalories.Text);


                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh product data and show success modal
            BindProductData();
            string script = "<script type=\"text/javascript\">$(document).ready(function() { $('#successModalUpdate').modal('show'); });</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowSuccessModalUpdate", script);
        }


        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check for related records in OrderProduct table
                string checkQuery = "SELECT COUNT(*) FROM OrderProduct WHERE ProductID = @ProductID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ProductID", lblDeleteProductID.Text);
                    conn.Open();
                    int relatedRecords = (int)checkCmd.ExecuteScalar();

                    if (relatedRecords > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorModalDelete", "$('#errorModalDelete').modal('show');", true);
                        return;
                    }
                }

                // If no related records, proceed to delete
                string query = "DELETE FROM Product WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", lblDeleteProductID.Text);
                    cmd.ExecuteNonQuery();
                }
            }

            BindProductData();

            // Trigger modal to show success
            string script = "<script type=\"text/javascript\">$(document).ready(function() { $('#successModalDelete').modal('show'); });</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowSuccessModalDelete", script);
        }


        protected void btnConfirmAddProduct_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblErrorAddProductName.Text = "";
            lblErrorAddProductDescription.Text = "";
            lblErrorAddCategory.Text = "";
            lblErrorAddProductPrice.Text = "";
            lblErrorAddImage.Text = "";
            lblErrorAddLongDesc.Text = "";
            lblErrorAddIngre.Text = "";
            lblErroAddCalories.Text = "";

            // Validation checks
            if (string.IsNullOrWhiteSpace(txtAddProductName.Text))
            {
                lblErrorAddProductName.Text = "Product name is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAddProductDescription.Text))
            {
                lblErrorAddProductDescription.Text = "Product's description is required.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(ddlAddCategory.SelectedValue))
            {
                lblErrorAddCategory.Text = "Please select a valid category.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAddProductPrice.Text) || !decimal.TryParse(txtAddProductPrice.Text, out decimal price) || price <= 0)
            {
                lblErrorAddProductPrice.Text = "Please enter a valid price greater than 0.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtAddImage.Text))
            {
                lblErrorAddImage.Text = "Image's path location is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAddLongDesc.Text))
            {
                lblErrorAddLongDesc.Text = "Product's long description is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAddIngre.Text))
            {
                lblErrorAddIngre.Text = "Product's ingredients is required.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtAddCalories.Text))
            {
                lblErroAddCalories.Text = "Product's calories is required.";
                isValid = false;
            }

            if (!isValid)
            {
                // Use ScriptManager to re-trigger the modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAddNewModal", "$('#AddNewModal').modal('show');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //get last productid
                string getLastID = "SELECT TOP 1 ProductId FROM Product ORDER BY ProductID DESC";
                string newProductID;

                conn.Open();

                using (SqlCommand getCmd = new SqlCommand(getLastID, conn))
                {
                    object lastIdObj = getCmd.ExecuteScalar();
                    if (lastIdObj != null)
                    {
                        string lastId = lastIdObj.ToString();
                        int numPart = int.Parse(lastId.Substring(2)); //get the number part of productid
                        newProductID = $"PR{(numPart + 1):D5}"; //add num part with the "PR"
                    }
                    else
                    {
                        //if no productid exist
                        newProductID = "PR00001";
                    }
                }

                string query = "INSERT INTO Product (ProductId, AdminID, Name, Description, Category, UnitPrice, Image, LongDescription, Ingredient, Calories) VALUES (@ProductId, @AdminID, @Name, @Description, @Category, @UnitPrice, @Image, @LongDescription, @Ingredient, @Calories)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", newProductID);
                    cmd.Parameters.AddWithValue("@AdminID", Session["AdminID"].ToString());
                    cmd.Parameters.AddWithValue("@Name", txtAddProductName.Text);
                    cmd.Parameters.AddWithValue("@Description", txtAddProductDescription.Text);
                    cmd.Parameters.AddWithValue("@Category", ddlAddCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@UnitPrice", txtAddProductPrice.Text);
                    cmd.Parameters.AddWithValue("@Image", txtAddImage.Text);
                    cmd.Parameters.AddWithValue("@LongDescription", txtAddLongDesc.Text);
                    cmd.Parameters.AddWithValue("@Ingredient", txtAddIngre.Text);
                    cmd.Parameters.AddWithValue("@Calories", txtAddCalories.Text);

                    cmd.ExecuteNonQuery();
                }
            }
            BindProductData();

            //trigger modal to show success
            string script = "<script type=\"text/javascript\">$(document).ready(function() { $('#successModalAdd').modal('show'); });</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowSuccessModalAdd", script);
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Product";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Prepare the response
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ProductData.xls");
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


        private void LoadData(string filter = "")
        {
            string query = "SELECT ProductID, Name, Description, Category, UnitPrice, Image FROM Product";
            if (!string.IsNullOrEmpty(filter))
            {
                query += " WHERE ProductID LIKE @Filter OR Name LIKE @Filter";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataListProducts.DataSource = dt;
                DataListProducts.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchPro.Text.Trim();
            LoadData(searchTerm);
        }
    }
}

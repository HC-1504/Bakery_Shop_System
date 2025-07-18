using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Asg
{
    public partial class UserManagement : System.Web.UI.Page
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
                    LoadCustData();
                }
            }            
        }

        private void LoadCustData(string sortExpression = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CustomerID, Customername, Email, Gender, DateOfBirth, ContactNo FROM Customer";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(sortExpression))
                    {
                        DataView dv = dt.DefaultView;
                        dv.Sort = $"{sortExpression} {SortDirection}";
                        gvCust.DataSource = dv;
                    }
                    else
                    {
                        gvCust.DataSource = dt;
                    }
                }
                else
                {
                    gvCust.DataSource = null;
                }

                gvCust.DataBind();
            }
        }

        protected void gvCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCust.PageIndex = e.NewPageIndex;
            LoadCustData(); // Reload data for the new page
        }

        protected void gvCust_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Toggle sort direction
            SortDirection = SortDirection == "ASC" ? "DESC" : "ASC";
            LoadCustData(e.SortExpression); // Reload data with sorting
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        protected void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearchCust.Text.Trim();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Customer WHERE CustomerID LIKE @Search OR Customername LIKE @Search";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + searchValue + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvCust.DataSource = dt;
                gvCust.DataBind();
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CustomerID, Customername, Email, Gender, DateOfBirth, ContactNo FROM Customer";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=CustomerData.xls");
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

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            string custID = (sender as LinkButton)?.CommandArgument;
            if (!string.IsNullOrEmpty(custID))
            {
                lblDeleteCustID.Text = custID;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#DeleteModal').modal('show');", true);
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
                    string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CustomerID", custID);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblEditCustID.Text = reader["CustomerID"].ToString();
                        txtEditCustName.Text = reader["Customername"].ToString();
                        txtEditEmail.Text = reader["Email"].ToString();
                        txtEditContactNo.Text = reader["ContactNo"].ToString();
                        txtEditDateOfBirth.Text = reader["DateOfBirth"] != DBNull.Value
                            ? Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd")
                            : string.Empty;
                        ddlSaveEditGender.SelectedValue = reader["Gender"].ToString();
                    }
                    con.Close();
                }
            }
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblErrorEditName.Text = "";
            lblErrorEditEmail.Text = "";
            lblErrorEditContact.Text = "";
            lblErrorEditDate.Text = "";
            lblErrorEditGender.Text = "";

            // Validation checks

            // 1. Staff Name Validation
            if (string.IsNullOrWhiteSpace(txtEditCustName.Text))
            {
                lblErrorEditName.Text = "Customer name is required.";
                isValid = false;
            }

            // 2. Email Validation
            if (string.IsNullOrWhiteSpace(txtEditEmail.Text))
            {
                lblErrorEditEmail.Text = "Customer's email is required.";
                isValid = false;
            }
            else
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basic email regex pattern
                if (!Regex.IsMatch(txtEditEmail.Text.Trim(), emailPattern))
                {
                    lblErrorEditEmail.Text = "Invalid email format. Example: example@gmail.com";
                    isValid = false;
                }
            }

            // 3. Contact Number Validation
            if (string.IsNullOrWhiteSpace(txtEditContactNo.Text))
            {
                lblErrorEditContact.Text = "Customer's contact number is required.";
                isValid = false;
            }
            else
            {
                string contactPattern = @"^\d{10,15}$"; // Only digits, 10-15 length
                if (!Regex.IsMatch(txtEditContactNo.Text.Trim(), contactPattern))
                {
                    lblErrorEditContact.Text = "Invalid contact number. It should contain 10 to 15 digits.";
                    isValid = false;
                }
            }

            // 4. Date of Birth Validation
            if (string.IsNullOrWhiteSpace(txtEditDateOfBirth.Text))
            {
                lblErrorEditDate.Text = "Customer's date of birth is required.";
                isValid = false;
            }
            else
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParseExact(txtEditDateOfBirth.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                {
                    lblErrorEditDate.Text = "Invalid date format. Use YYYY-MM-DD.";
                    isValid = false;
                }
            }

            // 5. Gender Validation
            if (string.IsNullOrEmpty(ddlSaveEditGender.SelectedValue))
            {
                lblErrorEditGender.Text = "Please select a valid gender.";
                isValid = false;
            }

            // If validation fails, show the modal again
            if (!isValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "$('#EditModal').modal('show');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Customer SET Customername = @Customername, Email = @Email, ContactNo = @ContactNo, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerID", lblEditCustID.Text);
                cmd.Parameters.AddWithValue("@Customername", txtEditCustName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEditEmail.Text);
                cmd.Parameters.AddWithValue("@ContactNo", txtEditContactNo.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", txtEditDateOfBirth.Text);
                cmd.Parameters.AddWithValue("@Gender", ddlSaveEditGender.SelectedValue);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "$('#successModalUpdate').modal('show');", true);
                LoadCustData();
            }
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Delete dependent records in the Payment table first
                string deletePaymentsQuery = "DELETE FROM Payment WHERE OrderID IN (SELECT OrderID FROM [Order] WHERE CustomerID = @CustomerID)";
                SqlCommand deletePaymentsCmd = new SqlCommand(deletePaymentsQuery, con);
                deletePaymentsCmd.Parameters.AddWithValue("@CustomerID", lblDeleteCustID.Text);

                // Then delete dependent records in the OrderProduct table
                string deleteOrderProductsQuery = "DELETE FROM OrderProduct WHERE OrderID IN (SELECT OrderID FROM [Order] WHERE CustomerID = @CustomerID)";
                SqlCommand deleteOrderProductsCmd = new SqlCommand(deleteOrderProductsQuery, con);
                deleteOrderProductsCmd.Parameters.AddWithValue("@CustomerID", lblDeleteCustID.Text);

                // Then delete records in the Order table
                string deleteOrdersQuery = "DELETE FROM [Order] WHERE CustomerID = @CustomerID";
                SqlCommand deleteOrdersCmd = new SqlCommand(deleteOrdersQuery, con);
                deleteOrdersCmd.Parameters.AddWithValue("@CustomerID", lblDeleteCustID.Text);

                // Finally, delete the customer record
                string deleteCustomerQuery = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
                SqlCommand deleteCustomerCmd = new SqlCommand(deleteCustomerQuery, con);
                deleteCustomerCmd.Parameters.AddWithValue("@CustomerID", lblDeleteCustID.Text);

                con.Open();

                // Execute the deletion commands in order
                deletePaymentsCmd.ExecuteNonQuery();
                deleteOrderProductsCmd.ExecuteNonQuery();
                deleteOrdersCmd.ExecuteNonQuery();
                deleteCustomerCmd.ExecuteNonQuery();

                con.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccess", "$('#successModalDelete').modal('show');", true);
                LoadCustData();
            }
        }


        protected void btnConfirmAddCust_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblErrorAddName.Text = "";
            lblErrorAddEmail.Text = "";
            lblErrorAddContact.Text = "";
            lblErrorAddDate.Text = "";
            lblErrorAddGender.Text = "";

            // Validation checks
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblErrorAddName.Text = "Customer name is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorAddEmail.Text = "Customer's email is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtContactNo.Text))
            {
                lblErrorAddContact.Text = "Customer's contact number is required.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
            {
                lblErrorAddDate.Text = "Customer's date of birth is required.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(ddlAddGender.SelectedValue))
            {
                lblErrorAddGender.Text = "Please select a valid gender.";
                isValid = false;
            }

            if (!isValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAddModal", "$('#AddNewModal').modal('show');", true);
                return;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string getLastID = "SELECT TOP 1 CustomerID FROM Customer ORDER BY CustomerID DESC";
                string newCustID;

                conn.Open();

                using (SqlCommand getCmd = new SqlCommand(getLastID, conn))
                {
                    object lastIdObj = getCmd.ExecuteScalar();
                    if (lastIdObj != null)
                    {
                        string lastId = lastIdObj.ToString();
                        int numPart = int.Parse(lastId.Substring(1));
                        newCustID = $"C{(numPart + 1):D5}";
                    }
                    else
                    {
                        newCustID = "C00001";
                    }
                }

                // Method to generate salt
                byte[] GenerateSalt()
                {
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        byte[] salt = new byte[16]; // Salt size 16 bytes
                        rng.GetBytes(salt);
                        return salt;
                    }
                }

                // Method to hash the password with PBKDF2 using the generated salt
                string HashPassword(string password)
                {
                    // Generate a random salt
                    byte[] salt = GenerateSalt();

                    // Use PBKDF2 to hash the password with the salt
                    using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations
                    {
                        byte[] hash = pbkdf2.GetBytes(32); // Generate a 256-bit hash (32 bytes)

                        // Combine salt and hash for storage
                        byte[] hashBytes = new byte[48]; // 16 bytes for salt + 32 bytes for hash
                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hash, 0, hashBytes, 16, 32);

                        // Convert to Base64 string for storage in the database
                        return Convert.ToBase64String(hashBytes);
                    }
                }



                // Hash the contact number with PBKDF2
                string hashedPassword = HashPassword(txtContactNo.Text);

                string query = "INSERT INTO Customer (CustomerID, CustomerName, Email, ContactNo, DateOfBirth, Gender, CreatedDate, Password) VALUES (@CustomerID, @CustomerName, @Email, @ContactNo, @DateOfBirth, @Gender, @CreatedDate, @Password)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", newCustID);
                    cmd.Parameters.AddWithValue("@CustomerName", txtName.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                    cmd.Parameters.AddWithValue("@Gender", ddlAddGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "AddSuccess", "$('#successModalAdd').modal('show');", true);
            LoadCustData();
            ClearUserModal();
        }



        private void ClearUserModal()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtContactNo.Text = "";
            txtDateOfBirth.Text = "";
            ddlAddGender.SelectedIndex = 0; // Assuming the first index is the default "Please select" option
        }



    }
}
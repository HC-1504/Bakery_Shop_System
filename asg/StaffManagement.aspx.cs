using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace Asg
{
    public partial class StaffManagement : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

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
                    LoadStaffData();
                }
            }            
        }

        private void LoadStaffData(string sortExpression = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT AdminID, Adminname, Email, Gender, DateOfBirth, ContactNo FROM Admin";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(sortExpression))
                    {
                        DataView dv = dt.DefaultView;
                        dv.Sort = $"{sortExpression} {SortDirection}";
                        gvStaff.DataSource = dv;
                    }
                    else
                    {
                        gvStaff.DataSource = dt;
                    }
                }
                else
                {
                    gvStaff.DataSource = null;
                }

                gvStaff.DataBind();
            }
        }

        protected void gvStaff_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStaff.PageIndex = e.NewPageIndex;
            LoadStaffData(); // Reload data for the new page
        }

        protected void gvStaff_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Toggle sort direction
            SortDirection = SortDirection == "ASC" ? "DESC" : "ASC";
            LoadStaffData(e.SortExpression); // Reload data with sorting
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        protected void btnSearchStaff_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearchStaff.Text.Trim();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Admin WHERE AdminID LIKE @Search OR Adminname LIKE @Search";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + searchValue + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvStaff.DataSource = dt;
                gvStaff.DataBind();
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT AdminID, Adminname, Email, Gender, DateOfBirth, ContactNo FROM Admin";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AdminData.xls");
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
            string AdminID = (sender as LinkButton)?.CommandArgument;
            if (!string.IsNullOrEmpty(AdminID))
            {
                lblDeleteStaffID.Text = AdminID;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#DeleteModal').modal('show');", true);
            }
        }

        protected void btnEdit_OnClick(object sender, EventArgs e)
        {
            string AdminID = (sender as LinkButton)?.CommandArgument;
            if (!string.IsNullOrEmpty(AdminID))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#EditModal').modal('show');", true);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Admin WHERE AdminID = @AdminID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AdminID", AdminID);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblEditStaffID.Text = reader["AdminID"].ToString();
                        txtEditStaffName.Text = reader["AdminName"].ToString();
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
            lblErrorEditStaffName.Text = "";
            lblErrorEditEmail.Text = "";
            lblErrorEditContact.Text = "";
            lblErrorEditDate.Text = "";
            lblErrorEditGender.Text = "";

            // Validation checks

            // 1. Staff Name Validation
            if (string.IsNullOrWhiteSpace(txtEditStaffName.Text))
            {
                lblErrorEditStaffName.Text = "Staff name is required.";
                isValid = false;
            }

            // 2. Email Validation
            if (string.IsNullOrWhiteSpace(txtEditEmail.Text))
            {
                lblErrorEditEmail.Text = "Staff's email is required.";
                isValid = false;
            }
            else
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basic email regex pattern
                if (!Regex.IsMatch(txtEditEmail.Text.Trim(), emailPattern))
                {
                    lblErrorEditEmail.Text = "Invalid email format. Example: example@admin.com";
                    isValid = false;
                }
            }

            // 3. Contact Number Validation
            if (string.IsNullOrWhiteSpace(txtEditContactNo.Text))
            {
                lblErrorEditContact.Text = "Staff's contact number is required.";
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
                lblErrorEditDate.Text = "Staff's date of birth is required.";
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
                string query = "UPDATE Admin SET Adminname = @Adminname, Email = @Email, ContactNo = @ContactNo, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE AdminID = @AdminID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AdminID", lblEditStaffID.Text);
                cmd.Parameters.AddWithValue("@Adminname", txtEditStaffName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEditEmail.Text);
                cmd.Parameters.AddWithValue("@ContactNo", txtEditContactNo.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", txtEditDateOfBirth.Text);
                cmd.Parameters.AddWithValue("@Gender", ddlSaveEditGender.SelectedValue);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "$('#successModalUpdate').modal('show');", true);
                LoadStaffData();
            }
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string adminID = lblDeleteStaffID.Text; // Assuming lblDeleteStaffID holds the AdminID.

                // First, delete related OrderProduct records that reference the ProductID
                string deleteOrderProductQuery = "DELETE FROM OrderProduct WHERE ProductID IN (SELECT ProductID FROM Product WHERE AdminID = @AdminID)";
                SqlCommand deleteOrderProductCmd = new SqlCommand(deleteOrderProductQuery, con);
                deleteOrderProductCmd.Parameters.AddWithValue("@AdminID", adminID);
                con.Open();
                deleteOrderProductCmd.ExecuteNonQuery();
                con.Close();

                // Now delete related Product records in the Product table
                string deleteProductQuery = "DELETE FROM Product WHERE AdminID = @AdminID";
                SqlCommand deleteProductCmd = new SqlCommand(deleteProductQuery, con);
                deleteProductCmd.Parameters.AddWithValue("@AdminID", adminID);
                con.Open();
                deleteProductCmd.ExecuteNonQuery();
                con.Close();

                // Now delete the Admin record
                string deleteAdminQuery = "DELETE FROM Admin WHERE AdminID = @AdminID";
                SqlCommand deleteAdminCmd = new SqlCommand(deleteAdminQuery, con);
                deleteAdminCmd.Parameters.AddWithValue("@AdminID", adminID); // Using AdminID to delete the Admin record
                con.Open();
                deleteAdminCmd.ExecuteNonQuery();
                con.Close();

                // Show success modal after deletion
                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccess", "$('#successModalDelete').modal('show');", true);
                LoadStaffData(); // Refresh staff data after deletion
            }
        }


        protected void btnConfirmAddStaff_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblErrorAddName.Text = "";
            lblErrorAddEmail.Text = "";
            lblErrorAddContact.Text = "";
            lblErrorAddDate.Text = "";
            lblErrorAddGender.Text = "";

            // Validation checks
            // 1. Staff Name Validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblErrorAddName.Text = "Staff name is required.";
                isValid = false;
            }

            // 2. Email Validation
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorAddEmail.Text = "Staff's email is required.";
                isValid = false;
            }

            // 3. Contact Number Validation
            if (string.IsNullOrWhiteSpace(txtContactNo.Text))
            {
                lblErrorAddContact.Text = "Staff's contact number is required.";
                isValid = false;
            }

            // 4. Date of Birth Validation
            if (string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
            {
                lblErrorAddDate.Text = "Staff's date of birth is required.";
                isValid = false;
            }

            // 5. Gender Validation
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
                // Get the last AdminID
                string getLastID = "SELECT TOP 1 AdminID FROM Admin ORDER BY AdminID DESC";
                string newAdminID;

                conn.Open();

                using (SqlCommand getCmd = new SqlCommand(getLastID, conn))
                {
                    object lastIdObj = getCmd.ExecuteScalar();
                    if (lastIdObj != null)
                    {
                        string lastId = lastIdObj.ToString();
                        int numPart = int.Parse(lastId.Substring(1));
                        newAdminID = $"A{(numPart + 1):D5}";
                    }
                    else
                    {
                        newAdminID = "A00001";
                    }
                }

                // Method to generate salt
                byte[] GenerateSalt()
                {
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        byte[] salt = new byte[16];
                        rng.GetBytes(salt); // Fill the array with cryptographically secure random bytes
                        return salt;
                    }
                }

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

                // Hash the staff's contact number as a "password"
                string hashedPassword = HashPassword(txtContactNo.Text);

                // Insert the new staff record into the database
                string query = "INSERT INTO Admin (AdminID, Adminname, Email, ContactNo, DateOfBirth, Gender, CreatedDate, Password) VALUES (@AdminID, @Adminname, @Email, @ContactNo, @DateOfBirth, @Gender, @CreatedDate, @Password)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", newAdminID);
                    cmd.Parameters.AddWithValue("@Adminname", txtName.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text); // Store the contact number as plain text or store the hashed password
                    cmd.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                    cmd.Parameters.AddWithValue("@Gender", ddlAddGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "AddSuccess", "$('#successModalAdd').modal('show');", true);
            LoadStaffData();
            ClearStaffModal();
        }



        private void ClearStaffModal()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtContactNo.Text = "";
            txtDateOfBirth.Text = "";
            ddlAddGender.SelectedIndex = 0; // Assuming the first index is the default "Please select" option
        }
    }
}

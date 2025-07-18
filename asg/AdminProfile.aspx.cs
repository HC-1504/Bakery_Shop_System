using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace Asg
{
    public partial class AdminProfile : System.Web.UI.Page
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
                    // Bind data explicitly
                    rptLeftDiv.DataBind();
                    AccSettingDetailsView.DataBind();

                }
            }                   
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            string AdminID = (sender as LinkButton)?.CommandArgument;
            if (!string.IsNullOrEmpty(AdminID))
            {
                lblDeleteAdminID.Text = AdminID;
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
                        lblEditAdminID.Text = reader["AdminID"].ToString();
                        txtEditAdminName.Text = reader["AdminName"].ToString();
                        txtEditAdminEmail.Text = reader["Email"].ToString();
                        txtEditAdminContact.Text = reader["ContactNo"].ToString();
                        txtEditAdminDOB.Text = reader["DateOfBirth"] != DBNull.Value
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
            lblErrorAdminName.Text = "";
            lblErrorAdminEmail.Text = "";
            lblErrorAdminContact.Text = "";
            lblErrorAdminDOB.Text = "";
            lblErrorEditGender.Text = "";

            // Validation checks

            // 1. Staff Name Validation
            if (string.IsNullOrWhiteSpace(txtEditAdminName.Text))
            {
                lblErrorAdminName.Text = "Staff name is required.";
                isValid = false;
            }

            // 2. Email Validation
            if (string.IsNullOrWhiteSpace(txtEditAdminEmail.Text))
            {
                lblErrorAdminEmail.Text = "Staff's email is required.";
                isValid = false;
            }
            else
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basic email regex pattern
                if (!Regex.IsMatch(txtEditAdminEmail.Text.Trim(), emailPattern))
                {
                    lblErrorAdminEmail.Text = "Invalid email format. Example: example@admin.com";
                    isValid = false;
                }
            }

            // 3. Contact Number Validation
            if (string.IsNullOrWhiteSpace(txtEditAdminContact.Text))
            {
                lblErrorAdminContact.Text = "Staff's contact number is required.";
                isValid = false;
            }
            else
            {
                string contactPattern = @"^\d{10,15}$"; // Only digits, 10-15 length
                if (!Regex.IsMatch(txtEditAdminContact.Text.Trim(), contactPattern))
                {
                    lblErrorAdminContact.Text = "Invalid contact number. It should contain 10 to 15 digits.";
                    isValid = false;
                }
            }

            // 4. Date of Birth Validation
            if (string.IsNullOrWhiteSpace(txtEditAdminDOB.Text))
            {
                lblErrorAdminDOB.Text = "Staff's date of birth is required.";
                isValid = false;
            }
            else
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParseExact(txtEditAdminDOB.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                {
                    lblErrorAdminDOB.Text = "Invalid date format. Use YYYY-MM-DD.";
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
                cmd.Parameters.AddWithValue("@AdminID", lblEditAdminID.Text);
                cmd.Parameters.AddWithValue("@Adminname", txtEditAdminName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEditAdminEmail.Text);
                cmd.Parameters.AddWithValue("@ContactNo", txtEditAdminContact.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", txtEditAdminDOB.Text);
                cmd.Parameters.AddWithValue("@Gender", ddlSaveEditGender.SelectedValue);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "$('#successModalUpdate').modal('show');", true);
                rptLeftDiv.DataBind();
                AccSettingDetailsView.DataBind();
            }
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Admin WHERE AdminID = @AdminID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AdminID", lblDeleteAdminID.Text);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            // Show success modal
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccess", "$('#successModalDelete').modal('show');", true);
                        }
                    }
                }

                // Redirect after successful delete
                Response.Redirect("~/Homepage.aspx");
            }
            catch (Exception ex)
            {
                // Log the exception (use proper logging mechanism in production)
                System.Diagnostics.Debug.WriteLine(ex.Message);

                // Display error modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteError", "$('#errorModalDelete').modal('show');", true);
            }
        }



        protected void btnChange_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ChangePasswordModal').modal('show');", true);
        }


        protected void btnSubmitPassword_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;

                lblErrorCurrentPassword.Text = "";
                lblErrorNewPassword.Text = "";
                lblErrorConfirmPassword.Text = "";

                string currentPassword = txtCurrentPassword.Text.Trim();
                string newPassword = txtNewPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();

                // Input validation
                if (string.IsNullOrEmpty(currentPassword))
                {
                    lblErrorCurrentPassword.Text = "Current Password is required.";
                    isValid = false;
                }

                if (string.IsNullOrEmpty(newPassword))
                {
                    lblErrorNewPassword.Text = "New Password is required.";
                    isValid = false;
                }

                if (string.IsNullOrEmpty(confirmPassword))
                {
                    lblErrorConfirmPassword.Text = "Confirm Password is required.";
                    isValid = false;
                }

                if (newPassword != confirmPassword)
                {
                    lblErrorConfirmPassword.Text = "Passwords do not match.";
                    isValid = false;
                }

                if (!isValid)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ChangePasswordModal').modal('show');", true);
                    return;
                }

                string hashedCurrentPassword = HashPassword(currentPassword);
                string hashedNewPassword = HashPassword(newPassword);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to get the current hashed password from the database
                    string query = "SELECT Password FROM Admin WHERE AdminID = @AdminID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AdminID", Session["AdminID"]);
                        string dbPassword = cmd.ExecuteScalar() as string;

                        // Validate the current password by comparing the hashed values
                        if (!ValidatePassword(currentPassword, dbPassword))
                        {
                            lblErrorCurrentPassword.Text = "Current password is incorrect.";
                            // Show modal with the error message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ChangePasswordModal').modal('show');", true);
                            return;
                        }
                    }

                    // Update password query with the hashed new password
                    query = "UPDATE Admin SET Password = @Password WHERE AdminID = @AdminID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", hashedNewPassword);  // Store the hashed new password
                        cmd.Parameters.AddWithValue("@AdminID", Session["AdminID"]);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Show success modal after successful password update
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#successModalChangePass').modal('show');", true);
            }
            catch (Exception ex)
            {
                lblErrorCurrentPassword.Text = "An error occurred while changing the password.";
                // Log exception: ex.Message
                // Show modal with the error message
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ChangePasswordModal').modal('show');", true);
            }
        }

        // Method to generate salt
        private byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // Salt size 16 bytes
                rng.GetBytes(salt);
                return salt;
            }
        }

        // Method to hash the password with PBKDF2 using the generated salt
        private string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations
            {
                byte[] hash = pbkdf2.GetBytes(32); // Generate a 256-bit hash (32 bytes)

                byte[] hashBytes = new byte[48]; // 16 bytes for salt + 32 bytes for hash
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                return Convert.ToBase64String(hashBytes);
            }
        }

        // Method to extract the salt and hash from the stored password
        private (byte[] Salt, byte[] Hash) ExtractSaltAndHash(string storedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(storedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            byte[] hash = new byte[32];
            Array.Copy(hashBytes, 16, hash, 0, 32);

            return (salt, hash);
        }

        // Method to validate if the entered password matches the stored hash
        private bool ValidatePassword(string enteredPassword, string storedPassword)
        {
            var (storedSalt, storedHash) = ExtractSaltAndHash(storedPassword);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, storedSalt, 10000)) // 10,000 iterations
            {
                byte[] hash = pbkdf2.GetBytes(32); // Generate a 256-bit hash (32 bytes)

                return storedHash.SequenceEqual(hash);
            }
        }

        protected void btnSecurityQu_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ChangeSecurityModal').modal('show');", true);
        }

        protected void btnSubmitSecurity_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Clear previous error messages
            lblSecurityQuestion.Text = "";
            lblSecurityAns.Text = "";

            // Validation checks

            if (string.IsNullOrEmpty(ddlSecurityQuestion.SelectedValue))
            {
                lblSecurityQuestion.Text = "Please select a valid security question.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtSecurityAns.Text))
            {
                lblSecurityAns.Text = "Security answer is required.";
                isValid = false;
            }

            // If validation fails, show the modal again
            if (!isValid)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#ChangeSecurityModal').modal('show');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Admin SET SecurityQuestion = @SecurityQuestion, SecurityAnswer = @SecurityAnswer WHERE AdminID = @AdminID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SecurityQuestion", ddlSecurityQuestion.SelectedValue);
                cmd.Parameters.AddWithValue("@SecurityAnswer", txtSecurityAns.Text);
                cmd.Parameters.AddWithValue("@AdminID", Session["AdminID"]);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "$('#successModalChangeSec').modal('show');", true);
                rptLeftDiv.DataBind();
                AccSettingDetailsView.DataBind();
            }
        }






        // Change Password
        //protected void btnSubmitPassword_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string currentPassword = txtCurrentPassword.Text.Trim();
        //        string newPassword = txtNewPassword.Text.Trim();
        //        string confirmPassword = txtConfirmPassword.Text.Trim();

        //        // Input validation
        //        if (string.IsNullOrEmpty(currentPassword) ||
        //            string.IsNullOrEmpty(newPassword) ||
        //            string.IsNullOrEmpty(confirmPassword))
        //        {
        //            lblErrorCurrentPassword.Text = "All fields are required.";
        //            return;
        //        }

        //        if (newPassword != confirmPassword)
        //        {
        //            lblErrorConfirmPassword.Text = "Passwords do not match.";
        //            return;
        //        }

        //        string HashPassword(string pass)
        //        {
        //            using (SHA256 sha256 = SHA256.Create())
        //            {
        //                byte[] bytes = Encoding.UTF8.GetBytes(pass);
        //                byte[] hashBytes = sha256.ComputeHash(bytes);

        //                // Convert hash bytes to hexadecimal string
        //                StringBuilder builder = new StringBuilder();
        //                foreach (byte b in hashBytes)
        //                {
        //                    builder.Append(b.ToString("x2"));
        //                }
        //                return builder.ToString();
        //            }
        //        }

        //        // Hash the current and new passwords
        //        string hashedCurrentPassword = HashPassword(currentPassword);
        //        string hashedNewPassword = HashPassword(newPassword);

        //        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            // Query to get current hashed password
        //            string query = "SELECT Password FROM Admin WHERE AdminID = @AdminID";
        //            using (SqlCommand cmd = new SqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@AdminID", Session["AdminID"]);

        //                conn.Open();
        //                string dbPassword = cmd.ExecuteScalar() as string;

        //                // Validate the current password by comparing the hashed values
        //                if (dbPassword != hashedCurrentPassword)
        //                {
        //                    lblErrorCurrentPassword.Text = "Current password is incorrect.";
        //                    return;
        //                }
        //            }

        //            // Update password query with hashed password
        //            query = "UPDATE Admin SET Password = @Password WHERE AdminID = @AdminID";  // Ensure the table and field names are correct
        //            using (SqlCommand cmd = new SqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@Password", hashedNewPassword);  // Store the hashed new password
        //                cmd.Parameters.AddWithValue("@AdminID", Session["AdminID"]);

        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        lblErrorCurrentPassword.Text = "Password changed successfully.";

        //        // Show success modal after successful password update
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#successModalChangePass').modal('show');", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblErrorCurrentPassword.Text = "An error occurred while changing the password.";
        //        // Log exception: ex.Message
        //    }
        //}


    }
}

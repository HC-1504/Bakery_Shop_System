using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

namespace asg
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set ValueToCompare dynamically to today's date
                cvDOB.ValueToCompare = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void cldDOB_SelectionChanged(object sender, EventArgs e)
        {            
            txtDOB.Text = cldDOB.SelectedDate.ToString("MM/dd/yyyy");
            
            // Trigger page validation manually
            Page.Validate(); // This will re-validate the form and check for any validation errors
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // check if email already exist
                if (isEmailExist())
                {
                    lblEmailExist.Text = "Email already exists. Please try another."; // Assuming lblError is a label for error messages
                }
                else
                {
                    // Proceed with registration logic
                    // create & open db connection
                    string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(strCon);
                    con.Open();

                    // Sql stmt & SqlCommand obj
                    string insertStmt = "INSERT INTO Customer " +
            "                           Values (@CustomerID, @Customername, @Email, @ContactNo, @DateOfBirth, @Gender, @CreatedDate, " +
            "@Password, @ProfilePic, @SecurityQuestion, @SecurityAnswer, @Balance)";
                    SqlCommand insertCmd = new SqlCommand(insertStmt, con);

                    // add param            
                    string customerID = calcCustomerID();

                    // Parse the Date of Birth (DOB) from txtDOB
                    DateTime dateOfBirth = DateTime.Parse(txtDOB.Text.Trim());  // Directly parse the date

                    DateTime createdDate = DateTime.Now;
                    string password = txtPw.Text.Trim();
                    string hashedPassword = PasswordHelper.HashPassword(password);
                    string ProfilePic = getProfilePic();
                    string secQue = ddlSecQue.Text.Trim();
                    string secAns = txtSecAns.Text.Trim().ToString();
                    double defaultBalance = 0.00;

                    insertCmd.Parameters.AddWithValue("@CustomerID", customerID);
                    insertCmd.Parameters.AddWithValue("@Customername", txtName.Text.Trim().ToString());
                    insertCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim().ToString());
                    insertCmd.Parameters.AddWithValue("@ContactNo", txtContact.Text.Trim().ToString());
                    insertCmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    insertCmd.Parameters.AddWithValue("@Gender", rblGender.SelectedValue.Trim());
                    insertCmd.Parameters.AddWithValue("@CreatedDate", createdDate);
                    insertCmd.Parameters.AddWithValue("@Password", hashedPassword);
                    insertCmd.Parameters.AddWithValue("@ProfilePic", ProfilePic);
                    insertCmd.Parameters.AddWithValue("@SecurityQuestion", secQue);
                    insertCmd.Parameters.AddWithValue("@SecurityAnswer", secAns);
                    insertCmd.Parameters.AddWithValue("@Balance", defaultBalance);

                    // execute SqlCommand obj & display status
                    int n = insertCmd.ExecuteNonQuery();
                    if (n > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "confirm",
                        "if(confirm('Account successfully registered! Do you want to go to the login page?')) { window.location='Login.aspx'; }", true);

                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ops! Unable to register an account.');", true);
                    }

                    // close db connection
                    con.Close();
                }
            }            
        }

        private string calcCustomerID()
        {
            string newCustomerID = string.Empty;

            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCommand obj
            string retrieveStmt = "SELECT TOP 1 CustomerID FROM Customer ORDER BY CustomerID DESC";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string lastCustomerID = (string)retrieveCmd.ExecuteScalar();

            if (lastCustomerID == null)
            {
                // Step 2: If no UserID exists, start with P00001
                newCustomerID = "C00001";
            }
            else
            {
                // Step 3: Increment the numeric part of the UserID                
                int numericPart = int.Parse(lastCustomerID.Substring(1)); // Extract numeric part (e.g., 00001)
                newCustomerID = "C" + (numericPart + 1).ToString("D5"); // Increment and format as PXXXXX
            }

            // close db connection
            con.Close();

            return newCustomerID.Trim().ToString();            
        }

        private string getProfilePic()
        {
            string profilePic = "ProfilePic/DefaultProfile.jpeg"; // Default picture

            // Check if user uploaded a file
            if (fuProfilePic.HasFile)
            {
                try
                {
                    // Validate file type (e.g., JPG, PNG)
                    string fileExtension = System.IO.Path.GetExtension(fuProfilePic.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    if (Array.Exists(allowedExtensions, ext => ext == fileExtension))
                    {
                        // Save file to server 
                        string folderPath = Server.MapPath("~/ProfilePic/");                        
                        lblUploadStatus.Text = "Image saved at: " + folderPath;
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + fileExtension; // Unique file name
                        fuProfilePic.SaveAs(System.IO.Path.Combine(folderPath, fileName));  //Saves the uploaded file to the specified location on the server.
                        profilePic = "ProfilePic/" + fileName; // Folder name + file name                        
                    }
                    else
                    {
                        lblUploadStatus.Text = "Only JPG, JPEG, or PNG files are allowed. Default Picture will be used.";                        
                    }
                }
                catch (Exception ex)
                {
                    lblUploadStatus.Text = "Error: " + ex.Message;
                }
            }            

            return profilePic;
        }

        private bool isEmailExist()
        {
            int countAdmin = 0;
            int countCust = 0;

            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveAdminStmt = "SELECT COUNT(*) FROM Admin WHERE Email = @Email";
            SqlCommand retrieveAdminCmd = new SqlCommand(retrieveAdminStmt, con);

            string retrieveCustStmt = "SELECT COUNT(*) FROM Customer WHERE Email = @Email";
            SqlCommand retrieveCustCmd = new SqlCommand(retrieveCustStmt, con);

            string email = txtEmail.Text.Trim();
            retrieveAdminCmd.Parameters.AddWithValue("@Email", email);
            retrieveCustCmd.Parameters.AddWithValue("@Email", email);

            // execute SqlCommand
            countAdmin = (int)retrieveAdminCmd.ExecuteScalar();
            countCust = (int)retrieveCustCmd.ExecuteScalar();

            // close db connection
            con.Close();

            return (countAdmin > 0 || countCust > 0);
        }

        protected void txtDOB_TextChanged(object sender, EventArgs e)
        {
            DateTime parsedDate;

            // Try to parse the date typed into txtDOB
            if (DateTime.TryParse(txtDOB.Text.Trim(), out parsedDate))
            {
                // If valid, set the selected date of the calendar to the parsed date
                cldDOB.SelectedDate = parsedDate;
                cldDOB.VisibleDate = parsedDate; // Optionally set the visible date to the same date
            }
            else
            {
                // If the date is invalid, you can clear the calendar or handle the error
                cldDOB.SelectedDate = DateTime.MinValue;
            }

            // Trigger page validation manually
            Page.Validate(); // This will re-validate the form and check for any validation errors
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContact.Text = string.Empty;
            txtDOB.Text = string.Empty;
            txtPw.Text = string.Empty;
            txtConfirmPw.Text = string.Empty;
            txtSecAns.Text = string.Empty;

            cldDOB.SelectedDate = DateTime.MinValue;

            rblGender.ClearSelection();
            ddlSecQue.SelectedIndex = -1;

            fuProfilePic.Attributes.Clear();

            // Reset validators to avoid showing errors after clearing
            rfvName.IsValid = true;
            rfvEmail.IsValid = true;
            rfvContact.IsValid = true;
            rfvDOB.IsValid = true;
            rfvGender.IsValid = true;
            rfvPw.IsValid = true;
            rfvConfirmPw.IsValid = true;
            rfvSecQue.IsValid = true;
            rfvSecAns.IsValid = true;
            cvConfirmPw.IsValid = true;

            // Reset password visibility toggle
            btnTogglePassword1.Text = "Show";
            btnTogglePassword2.Text = "Show";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace asg
{
    public partial class userProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (Session["IsLoggedIn"] == null || (bool)Session["IsLoggedIn"] == false)
            {
                // Redirect to login page if the user is not logged in
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                string customerID = (string)Session["CustomerID"];
                if (!IsPostBack)
                {
                    AccSettingDetailsView.DataBind();
                    BindRepeater();
                }
            }            
        }

        protected void btnChangePw_Click(object sender, EventArgs e)
        {
            pnlChangeSec.Visible = false;
            pnlChangePw.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlChangePw.Visible = false;
            pnlChangeSec.Visible=false;

            txtCurrentPw.Text = string.Empty;
            txtNewPw.Text = string.Empty;
            txtConfirmPw.Text = string.Empty;
        }

        protected void btnNewPw_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Compare current pw
                if (isCorrectCurrentPw())
                {
                    string newPw = txtNewPw.Text.Trim();

                    // Hash the new password before storing it
                    string hashedNewPw = PasswordHelper.HashPassword(newPw);

                    // Update hashedNewPw in database + display successful msg
                    updateNewPw(hashedNewPw);
                }
                else
                {
                    lblChangePwMsg.Text = "Incorrect Current Password! Please try again.";
                }
            }
            else
            {
                lblChangePwMsg.Text = "Please make sure all the fields are validated";                
            }
        }

        private bool isCorrectCurrentPw()
        {
            // connect & open DB
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT Password FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string customerID = (string)Session["CustomerID"];
            retrieveCmd.Parameters.AddWithValue("@CustomerID", customerID);

            // execute SqlCommand
            string correctPw = retrieveCmd.ExecuteScalar().ToString();

            string enteredPw = txtCurrentPw.Text.Trim();
            
            // close db connection
            con.Close();

            // Verify password using PasswordHelper
            if (PasswordHelper.VerifyPassword(enteredPw, correctPw))
            {
                return true;
            }
            else
            {
                // Password mismatch
                return false;
            }            
        }

        private bool isCorrectCurrentPwSec()
        {
            // connect & open DB
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT Password FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string customerID = (string)Session["CustomerID"];
            retrieveCmd.Parameters.AddWithValue("@CustomerID", customerID);

            // execute SqlCommand
            string correctPw = retrieveCmd.ExecuteScalar().ToString();

            string enteredPw = txtCurrentPwSec.Text.Trim();

            // close db connection
            con.Close();

            // Verify password using PasswordHelper
            if (PasswordHelper.VerifyPassword(enteredPw, correctPw))
            {
                return true;
            }
            else
            {
                // Password mismatch
                return false;
            }
        }

        private void updateNewPw(string hashedNewPw)
        {
            // connect & open DB
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string updateStmt = "UPDATE Customer SET Password = @hashedNewPw WHERE CustomerID = @CustomerID";
            SqlCommand updateCmd = new SqlCommand(updateStmt, con);

            string customerID = (string)Session["CustomerID"];
            updateCmd.Parameters.AddWithValue("@hashedNewPw", hashedNewPw);
            updateCmd.Parameters.AddWithValue("@CustomerID", customerID);

            // execute SqlCommand obj & display status
            int n = updateCmd.ExecuteNonQuery();
            if (n > 0)
            {
                lblChangePwMsg.Text = "Password updated successfully!";
                lblChangePwMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblChangePwMsg.Text = "Error updating password. Please try again later.";
            }


            // close db connection
            con.Close();
        }

        protected void btnDeleteAccount_Click(object sender, EventArgs e)
        {    
            pnlChangePw.Visible = false;
            pnlChangeSec.Visible = false;

            // connect & open DB
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string customerID = (string)Session["CustomerID"];

            // delete payment
            string deleteStmtPayment = "DELETE FROM Payment WHERE OrderID IN (SELECT OrderID FROM [Order] WHERE CustomerID = @CustomerID)";
            SqlCommand deleteCmdPayment = new SqlCommand(deleteStmtPayment, con);
            deleteCmdPayment.Parameters.AddWithValue("@CustomerID", customerID);

            // delete orderProd
            string deleteStmtOP = "DELETE FROM OrderProduct WHERE OrderID IN (SELECT OrderID FROM [Order] WHERE CustomerID = @CustomerID)";
            SqlCommand deleteCmdOP = new SqlCommand(deleteStmtOP, con);
            deleteCmdOP.Parameters.AddWithValue("@CustomerID", customerID);

            // delete order
            string deleteStmtOrder = "DELETE FROM [Order] WHERE CustomerID = @CustomerID";
            SqlCommand deleteCmdOrder = new SqlCommand(deleteStmtOrder, con);
            deleteCmdOrder.Parameters.AddWithValue("@CustomerID", customerID);

            // delete cust
            string deleteStmtCust = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand deleteCmdCust = new SqlCommand(deleteStmtCust, con);
            deleteCmdCust.Parameters.AddWithValue("@CustomerID", customerID);

            // execute SqlCommand obj & display status
            deleteCmdPayment.ExecuteNonQuery();
            deleteCmdOP.ExecuteNonQuery();
            deleteCmdOrder.ExecuteNonQuery();
            int n = deleteCmdCust.ExecuteNonQuery();

            if (n > 0)
            {
                // Account deleted successfully
                Session.Clear(); // Clear user session
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Your account has been deleted successfully. Redirecting to the Homepage.'); window.location='Homepage.aspx';", true);
            }
            else
            {
                // Account deletion failed
                lblDeleteAccMessage.Text = "Error: Unable to delete your account. Please try again later.";
            }

            // close db connection
            con.Close();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Get the Repeater item containing the FileUpload control
            RepeaterItem item = ((Button)sender).NamingContainer as RepeaterItem; // Get the RepeaterItem from the Button

            // Get the FileUpload control from the Repeater item
            FileUpload fileUploadProfilePic = (FileUpload)item.FindControl("fuProfilePic");

            // Check if a file has been selected
            if (fileUploadProfilePic.HasFile)
            {
                try
                {
                    // Validate file type (e.g., JPG, PNG)
                    string fileExtension = System.IO.Path.GetExtension(fileUploadProfilePic.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    if (Array.Exists(allowedExtensions, ext => ext == fileExtension))
                    {
                        // Save file to server 
                        string folderPath = Server.MapPath("~/ProfilePic/");
                        // lblUploadStatus.Text = "Image saved at: " + folderPath;
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + fileExtension; // Unique file name
                        fileUploadProfilePic.SaveAs(System.IO.Path.Combine(folderPath, fileName));  //Saves the uploaded file to the specified location on the server.
                        string newProfilePic = "ProfilePic/" + fileName; // Folder name + file name
                        string oldProfilePic = getOldProfilePic();

                        // Update the profile picture in the database
                        updateProfilePicInDatabase(newProfilePic);


                        // Delete the previous profile picture in the database
                        deleteProfilePic(oldProfilePic);
                    }
                    else
                    {
                        lblUploadStatus.Text = "Only JPG, JPEG, or PNG files are allowed.";
                        lblUploadStatus.ForeColor = System.Drawing.Color.Red;

                    }
                }
                catch (Exception ex)
                {
                    lblUploadStatus.Text = "Error: " + ex.Message;
                    lblUploadStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        private void updateProfilePicInDatabase(string profilePic)
        {
            // Update the profile picture in the database (example)
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // SQL statement to update the profile picture path
            string updateStmt = "UPDATE Customer SET ProfilePic = @ProfilePic WHERE CustomerID = @CustomerID";
            SqlCommand updateCmd = new SqlCommand(updateStmt, con);

            // Get the Customer ID (assuming it's stored in session)
            string customerID = (string)Session["CustomerID"];
            updateCmd.Parameters.AddWithValue("@ProfilePic", profilePic);
            updateCmd.Parameters.AddWithValue("@CustomerID", customerID);

            // execute SqlCommand obj & display status
            int n = updateCmd.ExecuteNonQuery();
            if (n > 0)
            {
                lblUploadStatus.Text = "New Profile Picture uploaded successfully!";
                lblUploadStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblUploadStatus.Text = "Something went wrong! Update db failed..";
                lblUploadStatus.ForeColor = System.Drawing.Color.Red;
            }

            rptLeftDiv.DataBind();

            // Close connection
            con.Close();
        }

        private void BindRepeater()
        {
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customer", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
        }

        private string getOldProfilePic()
        {
            string oldProfilePic = string.Empty;
            // connect & open DB
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT ProfilePic FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string customerID = (string)Session["CustomerID"];
            retrieveCmd.Parameters.AddWithValue("@CustomerID", customerID);

            // execute SqlCommand
            oldProfilePic = (string)retrieveCmd.ExecuteScalar();

            return oldProfilePic;
        }

        private void deleteProfilePic(string oldProfilePic)
        {
            if (!string.IsNullOrEmpty(oldProfilePic) && oldProfilePic != "DefaultProfile.jpeg")
            {
                string folderPath = Server.MapPath("~/");
                string oldFilePath = Path.Combine(folderPath, oldProfilePic);
                if (File.Exists(oldFilePath))
                {
                    try
                    {
                        // Attempt to delete the old image file
                        File.Delete(oldFilePath);
                        // lblDeleteAccMessage.Text = "File deleted at" + oldFilePath;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the error (e.g., file may be in use or permissions issue)
                        // Example: Log the exception or show a message
                        Console.WriteLine($"Error deleting file {oldFilePath}: {ex.Message}");
                    }
                }
                else
                {
                    lblDeleteAccMessage.Text = "";
                }
            }
        }

        protected void ValidateEmailUniqueness(object source, ServerValidateEventArgs args)
        {
            string updatedEmail = args.Value.Trim().ToString(); // The email entered in the TextBox: txtEmail
            string currentEmail = getCurrentEmail();

            if (updatedEmail == currentEmail)
            {
                // If the email has not been changed, validation passes.
                args.IsValid = true;
                return;
            }
            // Check the database to see if the email already exists
            args.IsValid = !isEmailExist(updatedEmail);
        }

        // Method to check if the email exists in the database
        private bool isEmailExist(string email)
        {
            bool exists = false;

            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT COUNT(*) FROM Customer WHERE Email = @Email AND CustomerID != @CustomerID";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string customerID = Session["CustomerID"].ToString();
            retrieveCmd.Parameters.AddWithValue("@Email", email);
            retrieveCmd.Parameters.AddWithValue("@CustomerID", customerID);  // Use CustomerID to exclude the current record

            // execute SqlCommand
            int count = (int)retrieveCmd.ExecuteScalar();            
            exists = count > 0;
            
            con.Close();

            return exists;
        }

        private string getCurrentEmail()
        {
            string currentEmail = string.Empty;

            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT Email FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string customerID = Session["CustomerID"].ToString();
            retrieveCmd.Parameters.AddWithValue("@CustomerID", customerID);            

            // execute SqlCommand
            currentEmail = retrieveCmd.ExecuteScalar().ToString();

            return currentEmail.Trim();
        }

        protected void ValidateDOB(object source, ServerValidateEventArgs args)
        {
            // Parse the entered date
            DateTime enteredDate;
            bool isValidDate = DateTime.TryParseExact(args.Value, "MM/dd/yyyy",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None, out enteredDate);

            if (isValidDate)
            {
                // Check if the entered date is in the future
                if (enteredDate > DateTime.Today)
                {
                    args.IsValid = false;  // Invalid if the entered DOB is in the future
                }
                else
                {
                    args.IsValid = true;   // Valid if the entered DOB is not in the future
                }
            }
            else
            {
                args.IsValid = false;  // Invalid if the date format is incorrect
            }
        }

        protected void btnSubmitSec_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Compare current pw
                if (isCorrectCurrentPwSec())
                {
                    // update db
                    // connect & open DB
                    string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(strCon);
                    con.Open();

                    // Sql stmt & SqlCmd obj
                    string updateStmt = "UPDATE Customer SET SecurityQuestion = @newSecQue, SecurityAnswer = @newSecAns WHERE CustomerID = @CustomerID";
                    SqlCommand updateCmd = new SqlCommand(updateStmt, con);

                    string newSecQue = ddlSecQue.SelectedValue.Trim().ToString();
                    string newSecAns = txtSecAns.Text.Trim();
                    string customerID = (string)Session["CustomerID"];

                    updateCmd.Parameters.AddWithValue("@newSecQue", newSecQue);
                    updateCmd.Parameters.AddWithValue("@newSecAns", newSecAns);
                    updateCmd.Parameters.AddWithValue("@CustomerID", customerID);

                    // execute SqlCommand obj & display status
                    int n = updateCmd.ExecuteNonQuery();

                    // close db
                    con.Close();

                    if (n > 0)
                    {
                        ddlSecQue.ClearSelection();
                        ddlSecQue.Items[0].Selected = true;
                        txtSecAns.Text = string.Empty;

                        lblErrorMsgSec.Text = "Security QnA updated successfully!";
                        lblErrorMsgSec.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblChangePwMsg.Text = "Error updating Security QnA. Please try again later.";
                    }

                }
                else
                {
                    lblErrorMsgSec.Text = "Incorrect Current Password! Please try again.";
                }
            }
            else
            {
                lblErrorMsgSec.Text = "Please make sure all the fields are validated";
            }
        }

        protected void btnChangeSec_Click(object sender, EventArgs e)
        {
            pnlChangePw.Visible = false;
            pnlChangeSec.Visible = true;
        }

        protected void btnCancelSec_Click(object sender, EventArgs e)
        {
            pnlChangePw.Visible = false;
            pnlChangeSec.Visible = false;

            txtCurrentPwSec.Text = string.Empty;
            ddlSecQue.ClearSelection();
            ddlSecQue.Items[0].Selected = true;
            txtSecAns.Text = string.Empty;
        }
    }
}

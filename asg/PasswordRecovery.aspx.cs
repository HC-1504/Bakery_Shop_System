using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asg
{
    public partial class PasswordRecovery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMsg.Text = string.Empty;
        }

        protected void btnPwRec_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Check if the user has exceeded the allowed number of attempts
                if (Session["FailedAttemptsSec"] != null && (int)Session["FailedAttemptsSec"] >= 3)
                {
                    // If 3 attempts failed, check if 60 seconds have passed since the last failed attempt
                    if (Session["LastFailedAttemptSec"] != null)
                    {
                        DateTime lastFailedAttempt = (DateTime)Session["LastFailedAttemptSec"];
                        int secondsElapsed = (int)(DateTime.Now - lastFailedAttempt).TotalSeconds;
                        int remainingTime = 10 - secondsElapsed;

                        if (remainingTime > 0)
                        {
                            // Show a pop-up with the remaining time
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('You have exceeded 3 failed attempts. Please wait {remainingTime} seconds before trying again.');", true);
                            return;
                        }
                        else
                        {
                            // Reset the failed attempts after 60 seconds
                            Session["FailedAttemptsSec"] = 0;
                        }
                    }
                }

                // check if email exist                
                if (isEmailExist())
                {
                    // get correct secQue from DB
                    string enteredSecQue = ddlSecQue.SelectedValue.Trim().ToString();                    
                    string correctSecQue = getCorrectSecQue();

                    // Check if correct security question is NULL or empty
                    if (string.IsNullOrEmpty(correctSecQue)) // Check if null or empty
                    {
                        lblErrorMsg.Text = "Security question and answer not set for this account."; // Display error message                        
                        return; // Stop further execution
                    }

                    correctSecQue = correctSecQue.Trim(); // Trim after null check

                    // get correct secAns from DB
                    string enteredSecAns = txtSecAns.Text.Trim().ToString();
                    string correctSecAns = getCorrectSecAns();

                    // Check if correct security answer is NULL or empty
                    if (string.IsNullOrEmpty(correctSecAns))
                    {
                        lblErrorMsg.Text = "Security question and answer not set for this account.";
                        return; // Stop further execution
                    }

                    correctSecAns = correctSecAns.Trim(); // Trim after null/empty check

                    // if enteredSecQue == correctSecQue, check ans
                    if ((enteredSecQue == correctSecQue) && (enteredSecAns == correctSecAns))
                    {
                        // login is successful, reset failed attempts and redirect
                        Session["FailedAttemptsSec"] = 0;
                        string customerID = getCustID();
                        Session["CustomerID"] = customerID;
                        Session["IsLoggedIn"] = true;

                        // display changePwDiv
                        pnlChangePw.Visible = true;
                    }
                    else // incorrect secQue & secAns
                    {
                        // login fails, increase failed attempts and show remaining attempts
                        if (Session["FailedAttemptsSec"] == null)
                        {
                            Session["FailedAttemptsSec"] = 0;
                        }

                        int failedAttempts = (int)Session["FailedAttemptsSec"];
                        failedAttempts++;

                        Session["FailedAttemptsSec"] = failedAttempts;
                        Session["LastFailedAttemptSec"] = DateTime.Now;

                        // Show an alert with remaining attempts
                        int remainingAttempts = 3 - failedAttempts;
                        if (remainingAttempts > 0)
                        {
                            pnlChangePw.Visible = false;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Oops! Incorrect Security Question or Answer. You have {remainingAttempts} attempts left.');", true);
                        }
                        else
                        {
                            pnlChangePw.Visible = false;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You have exceeded 3 failed attempts. Please wait 60 seconds before trying again.');", true);
                        }
                    }
                }
                else
                {
                    lblErrorMsg.Text = "Email does not exist! Please register.";
                    pnlChangePw.Visible = false;
                }
            }
            else
            {
                lblErrorMsg.Text = "Please make sure all fields are validated.";
            }
        }

        private string getCorrectSecQue()
        {
            string correctSecQue = string.Empty;

            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT SecurityQuestion FROM Customer WHERE Email = @Email";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string email = txtEmail.Text;
            retrieveCmd.Parameters.AddWithValue("@Email", email);

            // execute SqlCommand
            object result = retrieveCmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                correctSecQue = result.ToString();
            }

            // close db connection
            con.Close();

            // return 
            return correctSecQue;
        }

        private bool isEmailExist()
        {            
            // retrieve email from db
            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT COUNT(1) FROM Customer WHERE Email = @Email";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string email = txtEmail.Text.Trim();
            retrieveCmd.Parameters.AddWithValue("@Email", email);

            // execute SqlCommand
            int emailCount = (int)retrieveCmd.ExecuteScalar();

            // close db connection
            con.Close();

            // return
            return emailCount > 0;
        }

        private string getCustID()
        {
            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT CustomerID FROM Customer WHERE Email = @Email";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string email = txtEmail.Text;
            retrieveCmd.Parameters.AddWithValue("@Email", email);

            // execute SqlCommand
            string customerID = retrieveCmd.ExecuteScalar().ToString();

            // close db connection
            con.Close();

            return customerID;
        }

        private string getCorrectSecAns()
        {
            string correctSecAns = string.Empty;

            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT SecurityAnswer FROM Customer WHERE Email = @Email";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string email = txtEmail.Text;
            retrieveCmd.Parameters.AddWithValue("@Email", email);

            // execute SqlCommand
            object result = retrieveCmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                correctSecAns = result.ToString();
            }

            // close db connection
            con.Close();

            // return 
            return correctSecAns;
        }

        protected void btnNewPw_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string newPw = txtNewPw.Text.Trim();

                // Hash the new password before storing it
                string hashedNewPw = PasswordHelper.HashPassword(newPw);

                // Update hashedNewPw in database
                updateNewPw(hashedNewPw);

                // display successful pw update
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('New password updated sucessfully! Directing to the Login Page now.'); window.location='Login.aspx';", true);
            }
            else
            {
                lblErrorMsg.Text = "Please make sure all fields are validated.";
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

            // close db connection
            con.Close();

            if (n > 0)
            {
                lblChangePwMsg.Text = "Password updated successfully!";
                lblChangePwMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblChangePwMsg.Text = "Error updating password. Please try again later.";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
}
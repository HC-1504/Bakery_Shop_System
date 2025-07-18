using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace asg
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Register.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // check if email already exist
                if (!isEmailExist())
                {
                    lblEmailExist.Text = "Email does not exist. Please register."; // Assuming lblError is a label for error messages
                    return;
                }

                // Check if the user has exceeded the allowed number of attempts
                if (Session["FailedAttempts"] != null && (int)Session["FailedAttempts"] >= 3)
                {
                    // If 3 attempts failed, check if 60 seconds have passed since the last failed attempt
                    if (Session["LastFailedAttempt"] != null)
                    {
                        DateTime lastFailedAttempt = (DateTime)Session["LastFailedAttempt"];
                        int secondsElapsed = (int)(DateTime.Now - lastFailedAttempt).TotalSeconds;
                        int remainingTime = 10 - secondsElapsed;

                        if (remainingTime > 0)
                        {
                            // Show a pop-up with the remaining time
                            lblEmailExist.Text = string.Empty;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('You have exceeded 3 failed attempts. Please wait {remainingTime} seconds before trying again.');", true);
                            return;
                        }
                        else
                        {
                            // Reset the failed attempts after 60 seconds
                            Session["FailedAttempts"] = 0;
                        }
                    }
                }
            }


            // check if identity = customer
            // get correct email from DB
            Object customerIdObj = getCustIdObj();

            // compare correctPw and enteredPw                
            if (customerIdObj != null)
            {
                // If login is successful, reset failed attempts and redirect
                Session["FailedAttempts"] = 0;
                string customerID = customerIdObj.ToString();
                Session["CustomerID"] = customerID;
                Session["IsLoggedIn"] = true;

                // display msg
                lblEmailExist.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You have successfully login!'); window.location='Homepage.aspx';", true);
            }
            else // incorrect Pw
            {
                // check if identity = admin
                // get correct email from DB
                Object adminIdObj = getAdminIdObj();
                // compare correctPw and enteredPw                
                if (adminIdObj != null)
                {
                    // If login is successful, reset failed attempts and redirect
                    Session["FailedAttempts"] = 0;
                    string adminID = adminIdObj.ToString();
                    Session["AdminID"] = adminID;
                    Session["IsLoggedIn"] = true;

                    // display msg
                    lblEmailExist.Text = string.Empty;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You have successfully login!'); window.location='AdminHome.aspx';", true);
                }
                else
                {
                    // If login fails, increase failed attempts and show remaining attempts
                    if (Session["FailedAttempts"] == null)
                    {
                        Session["FailedAttempts"] = 0;
                    }

                    int failedAttempts = (int)Session["FailedAttempts"];
                    failedAttempts++;

                    Session["FailedAttempts"] = failedAttempts;
                    Session["LastFailedAttempt"] = DateTime.Now;

                    // Show an alert with remaining attempts
                    int remainingAttempts = 3 - failedAttempts;
                    if (remainingAttempts > 0)
                    {
                        lblEmailExist.Text = string.Empty;
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Oops! Incorrect email or password. You have {remainingAttempts} attempts left.');", true);
                    }
                    else
                    {
                        lblEmailExist.Text = string.Empty;
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You have exceeded 3 failed attempts. Please wait 60 seconds before trying again.');", true);
                    }
                }
            }
        }

        private object getCustIdObj()
        {
            // Create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql statement & SqlCommand object
            string retrieveStmt = "SELECT CustomerID, Password FROM Customer WHERE Email = @Email";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string email = txtEmail.Text.Trim();
            string enteredPw = txtPw.Text.Trim();

            retrieveCmd.Parameters.AddWithValue("@Email", email);

            // Execute SqlCommand to retrieve data
            SqlDataReader dtrCust = retrieveCmd.ExecuteReader();

            object customerIdObj = null;

            if (dtrCust.HasRows)
            {
                // Since we expect only one row, we can use Read() just once
                dtrCust.Read(); // Read the first (and likely only) record

                string correctPw = dtrCust["Password"].ToString();
                customerIdObj = dtrCust["CustomerID"];

                // Verify password using PasswordHelper
                if (PasswordHelper.VerifyPassword(enteredPw, correctPw))
                {
                    // close db connection
                    con.Close();

                    return customerIdObj;
                }
                else
                {
                    // close db connection
                    con.Close();

                    // Password mismatch
                    return null;
                }
            }
            else
            {
                // close db connection
                con.Close();

                // No matching email found
                return null;
            }
        }


        private Object getAdminIdObj()
        {
            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT AdminID, Password FROM Admin WHERE Email = @Email";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            string email = txtEmail.Text.Trim();
            string enteredPw = txtPw.Text.Trim();

            retrieveCmd.Parameters.AddWithValue("@Email", email);
            retrieveCmd.Parameters.AddWithValue("@Password", enteredPw);

            // execute SqlCommand            
            SqlDataReader dtrAdmin = retrieveCmd.ExecuteReader();

            object adminIdObj = null;

            if (dtrAdmin.HasRows)
            {
                // Since we expect only one row, we can use Read() just once
                dtrAdmin.Read(); // Read the first (and likely only) record

                string correctPw = dtrAdmin["Password"].ToString();
                adminIdObj = dtrAdmin["AdminID"];

                // Verify password using PasswordHelper
                if (PasswordHelper.VerifyPassword(enteredPw, correctPw))
                {
                    // close db connection
                    con.Close();

                    return adminIdObj;
                }
                else
                {
                    // close db connection
                    con.Close();

                    // Password mismatch
                    return null;
                }
            }
            else
            {
                // close db connection
                con.Close();

                // No matching email found
                return null;
            }
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
    }
}
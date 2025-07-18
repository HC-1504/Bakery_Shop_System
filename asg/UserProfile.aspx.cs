using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace asg
{
    public partial class userProfile1 : System.Web.UI.Page
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
                    rptProfile.DataBind();                    
                }
            }
        }       

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Clear all session variables
            Session.Abandon(); // End the session
            Response.Redirect("~/Homepage.aspx"); // Redirect to the homepage        
        }

        protected void btnAccountSetting_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountSetting.aspx");
        }

        protected void btnOrderTracking_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/OrderTracking.aspx");
        }
    }
}
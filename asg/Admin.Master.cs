using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asg
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Clear all session variables
            Session.Abandon(); // End the session
            Response.Redirect("~/Homepage.aspx"); // Redirect to the homepage
        }
    }
}
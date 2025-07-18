using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asg
{
    public partial class customerMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"])
            {
                hplLogin.Visible = false;  // Hide the "Login" link
                hplProfile.Visible = true; // Show the "Profile" link
            }
            else
            {
                hplLogin.Visible = true;  // Show the "Login" link
                hplProfile.Visible = false; // Hide the "Profile" link
            }
        }
    }
}
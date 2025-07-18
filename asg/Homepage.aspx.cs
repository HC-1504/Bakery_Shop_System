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
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPopularProducts();
                moreDataSource.DataBind();
            }
        }

        private void BindPopularProducts()
        {
            // create & open db connection
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            // Sql stmt & SqlCmd obj
            string retrieveStmt = "SELECT [Name], [Description], [Image] " +
                           "FROM [Product]" +
                           "WHERE [Name] IN ('Butter Biscuits', 'Cheesecake', 'French Baguette')";
            SqlCommand retrieveCmd = new SqlCommand(retrieveStmt, con);

            // execute SqlCommand
            SqlDataReader reader = retrieveCmd.ExecuteReader();

            // Bind data to the rptPopular Repeater control
            rptPopular.DataSource = reader;
            rptPopular.DataBind();

            reader.Close();
            con.Close();
            
        }
    }
}
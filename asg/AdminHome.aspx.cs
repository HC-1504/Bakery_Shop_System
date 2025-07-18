using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asg
{
    public partial class AdminHome : System.Web.UI.Page
    {
        public string CategoryChartData { get; private set; }
        public string SalesChartData { get; private set; }

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
                    try
                    {
                        LoadDashboardData();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "An error occurred: " + ex.Message + " - " + ex.StackTrace;
                    }
                }
            }            
        }

        private void LoadDashboardData()
        {
            string connStr = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                //total completed order
                try
                {
                    SqlCommand sessionCmd = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Order] WHERE OrderStatus = 'Completed';", conn);
                    lblTotalCompleteOrder.Text = sessionCmd.ExecuteScalar().ToString();
                }
                catch (Exception)
                {
                    lblTotalCompleteOrder.Text = "0 (Error)";
                }


                // Fetch Total Orders
                try
                {
                    SqlCommand orderCmd = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Order]", conn);
                    lblOrder.Text = orderCmd.ExecuteScalar().ToString();
                }
                catch (Exception)
                {
                    lblOrder.Text = "0 (Error)";
                }

                // Fetch Total Sales
                try
                {
                    SqlCommand salesCmd = new SqlCommand("SELECT ISNULL(SUM(TotalPrice),0) FROM [dbo].[Payment]", conn);
                    lblTotalSales.Text = ((decimal)salesCmd.ExecuteScalar()).ToString("0.00");
                }
                catch (Exception)
                {
                    lblTotalSales.Text = "0.00 (Error)";
                }

                // Fetch Popular Categories Data
                SqlCommand categoryCmd = new SqlCommand("SELECT Category, COUNT(*) AS Count FROM [dbo].[Product] GROUP BY Category", conn);
                DataTable categoryDt = new DataTable();
                SqlDataAdapter categoryDa = new SqlDataAdapter(categoryCmd);
                categoryDa.Fill(categoryDt);

                CategoryChartData = ConvertToGoogleChartsArray(categoryDt, "Category", "Count");

                // Fetch Sales Performance Data
                try
                {
                    SqlCommand salesDataCmd = new SqlCommand(
                        "SELECT CONVERT(VARCHAR(10), PaymentDate, 120) AS Date, ISNULL(SUM(TotalPrice), 0) AS Total FROM [dbo].[Payment] GROUP BY CONVERT(VARCHAR(10), PaymentDate, 120)", conn);
                    DataTable salesDt = new DataTable();
                    SqlDataAdapter salesDataDa = new SqlDataAdapter(salesDataCmd);
                    salesDataDa.Fill(salesDt);

                    SalesChartData = ConvertToGoogleChartsArray(salesDt, "Date", "Total");
                }
                catch (Exception)
                {
                    SalesChartData = "[['Date', 'Total'], ['No Data', 0]]";
                }
            }
        }

        private string ConvertToGoogleChartsArray(DataTable dataTable, string labelColumn, string valueColumn)
        {
            StringBuilder chartData = new StringBuilder();
            chartData.Append("[['" + labelColumn + "', '" + valueColumn + "'],"); // Header row

            foreach (DataRow row in dataTable.Rows)
            {
                chartData.Append("['" + row[labelColumn].ToString() + "', " + row[valueColumn].ToString() + "],");
            }

            // Remove the trailing comma and close the array
            chartData.Length--;
            chartData.Append("]");

            return chartData.ToString();
        }
    }
}

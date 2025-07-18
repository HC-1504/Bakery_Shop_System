<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="Asg.AdminHome" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br /><br />
    <!-- Summary Section -->
    <div class="container">
        <br />
        <br />
        <h1 class="text-center title" style="margin-top: 1%;">Admin Home</h1>
        <br />
        <h2>Summary</h2>
        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>

        <div class ="bigframe" style="height:550px;">
            <div class="frame-container">
                <div class="frame">
                    <h5>Total Completed Order:</h5>
                    <asp:Label ID="lblTotalCompleteOrder" runat="server"></asp:Label>
                </div>
                <div class="frame">
                    <h5>Total Order:</h5>
                    <asp:Label ID="lblOrder" runat="server"></asp:Label>
                </div>
                <div class="frame">
                    <h5>Total Sales:</h5>
                    RM <asp:Label ID="lblTotalSales" runat="server"></asp:Label>
                </div>
            </div>

            <br />

            <div>
                <!-- Popular Categories -->
                <div id="categoryChart" style="width: 32%; height: 400px; float: left; left:100px; border: 1px solid black; border-collapse: separate; margin: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);"></div>
               
                <!-- Sales Performance -->
                <div id="salesChart" style="width: 65%; height: 400px; float: right; right:25px;  border: 1px solid black; border-collapse: separate; margin: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);"></div>
            </div>
            
        </div>
    </div>

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        // Load Google Charts
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawDashboardCharts);

        function drawDashboardCharts() {
            // Data for Popular Categories
            var categoryData = google.visualization.arrayToDataTable(<%= CategoryChartData %>);

            var categoryOptions = {
                title: 'Popular Categories',
                pieHole: 0.2
            };

            var categoryChart = new google.visualization.PieChart(document.getElementById('categoryChart'));
            categoryChart.draw(categoryData, categoryOptions);

            // Data for Sales Performance
            var salesData = google.visualization.arrayToDataTable(<%= SalesChartData %>);

            var salesOptions = {
                title: 'Sales Performance',
                hAxis: { title: 'Date' },
                vAxis: { title: 'Sales Amount' },
                legend: 'none',
                bar: { groupWidth: "40%" }
            };

            var salesChart = new google.visualization.ColumnChart(document.getElementById('salesChart'));
            salesChart.draw(salesData, salesOptions);
        }
    </script>

</asp:Content>


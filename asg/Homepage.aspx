<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="asg.Homepage" %>

<asp:Content ID="HomepageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    Homepage
</asp:Content>

<asp:Content ID="HomepageCss" ContentPlaceHolderID="CssLink" runat="server">
    <link rel="stylesheet" type="text/css" href="Homepage.css" />
</asp:Content>

<asp:Content ID="HomepageContent" ContentPlaceHolderID="MainContent" runat="server">    
<div class="homeContent">
    <div class="poster">
        <img src="Images/bakeryPoster.png" alt="poster" />
    </div>
    <br /><br /><br />
    
    <div class="slogans">
        <div style="border-right: 1px solid black;  height: 50px;">   
            <div><img src="Images/handcrafted.jpeg" alt="handcraftedIcon" style="width: 30px;"/></div>
            <div>Handcrafted</div>
        </div>
        
        <div style="border-right: 1px solid black;  height: 50px;">
            <div><img src="Images/freshlyBaked.jpeg" alt="freshlyBakedIcon" style="width: 30px;" /></div>
            <div>Freshly Baked</div>
        </div>
        
        <div>
            <div><img src="Images/zeroAdditives.jpeg" alt="zeroAdditivesIcon" style="width: 30px;" /></div>
            <div>0 Artificial Additives </div>
        </div>
    </div>
    <br /><br />

    <asp:SqlDataSource ID="dsPopular" runat="server"></asp:SqlDataSource>
    <h1>Popular Products!</h1>
    <div class="popular">         
        <asp:Repeater ID="rptPopular" runat="server">
            <ItemTemplate>
                <div>
                    <div class="popularImg">
                        <img src='<%# Eval("Image") %>' alt='<%# Eval("Name") %>' />
                    </div>
                    <div class="popularDesc">
                        <b><%# Eval("Name") %></b><br />
                        <%# Eval("Description") %>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <br /><br />

    <h1>Drop By Now!</h1>
    <div class="dropBy">
        <table style="width:100%">        
            <tr>
                <td style="padding: 30px 20px 5px; border-right: 1px solid black; font-weight:bold;">Opening Hours</td>
                <td style="padding: 30px 20px 5px; font-weight:bold;">Location</td>
            </tr>
            <tr>
                <td style="padding: 5px 20px 30px; border-right: 1px solid black;">10:00 a.m - 10.00 p.m</td>
                <td style="padding: 5px 20px 30px">Segamat Central</td>
            </tr>
        </table>
    </div>
    <br /><br />

    <h1>Check Out More!</h1>
    <div class="moreDiv">
        <asp:SqlDataSource ID="moreDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT 
                            p.ProductID, 
                            p.[Name], 
                            p.[LongDescription], 
                            p.[Category], 
                            p.[Image]
                        FROM [Product] p
                        INNER JOIN (
                            SELECT 
                                [Category], 
                                MIN(ProductID) AS MinProductID
                            FROM [Product]
                            GROUP BY [Category]
                        ) grouped ON p.ProductID = grouped.MinProductID;">
        </asp:SqlDataSource>
        <asp:Repeater ID="rptMore" runat="server" DataSourceID="moreDataSource">
            <ItemTemplate>
                <div class="moreItem">
                    <table class="prodIntro">
                        <tr>
                            <td>
                                <img src="<%#Eval("Image") %>" alt="prodIntroImg"/>
                            </td>
                            <td style="text-align:center; padding: 0px 10px;">
                                <b><%#Eval("Category") %> - <%#Eval("Name") %></b>
                                <br />
                                <%#Eval("LongDescription") %>
                                
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </ItemTemplate>

            <AlternatingItemTemplate>
                <div class="moreAltItem">
                    <table class="prodIntro">
                        <tr>
                            <td style="text-align:center; padding: 0px 10px;">
                                <b><%#Eval("Category") %> - <%#Eval("Name") %></b>
                                <br />
                                <%#Eval("LongDescription") %>
                                
                            </td>
                            <td>
                                <img src="<%#Eval("Image") %>" alt="prodIntroImg"/>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </div>
    <br /><br />    
</div>
</asp:Content>

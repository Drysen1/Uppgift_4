<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

            <title>ADMIN</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">
        <h1>ADMIN</h1>
    </div>
    <div class="fullBox">
        <div class="fullBox" >
            <h2 class="home-h2">Välkommen <asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label></h2>
            <p class="home-text">Välkommen till administrationsdelen av JE Bankens kompetensportal. <br/> På denna sida kan du som provledare se när personerna i ditt team gjort Licenceringstestet och det årliga ÅKU.
               
                <br />
            </p>
            <p class="home-text">Du kan även se statistik över ditt teams utförda prov.<br />
            </p>
            <p class="home-text">Om du upplever några problem med kompetensportalen eller saknar personer i ditt team eller prov. Kontakta då JE bankens IT-support för hjälp.
            </p>
        </div>
        <!--<div class="halfBox">
            <div class="fullBox">
                <img class="home-img round-img" src="img/mote.jpg" />
            </div>
        </div>-->
    </div>
    <div class="fullBox" >
        <div class="halfBox center-text" id="center-team">
            <div class="admin-h3-box">
                <h3 class="home-h3">MEDARBETARE</h3>
            </div>
            <div class="fullBox">
                <asp:Chart ID="Chart1" runat="server" Width="300" Height="300">
                    <Titles>
                        <asp:Title Name="title" Text="Teammedlemmar med test att utföra" Alignment="MiddleCenter"></asp:Title>
                    </Titles>
                    <Series>
                        <asp:Series Name="series" ChartType="Pie"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" ></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="Button1" runat="server" Text="SE TEAM" CssClass="my-button button-80" OnClick="Button1_Click" />
            </div>
        </div>

        <div class="halfBox center-text" id="center-stats">
            <div class="fullBox admin-h3-box">
                <h3 class="home-h3">STATISTIK</h3>
            </div>

            <img class="round-img" src="img/statspic.jpg" />
            <asp:Button ID="Button2" runat="server" Text="SE STATISTIK" CssClass="my-button button-80" OnClick="Button2_Click" />
        </div>
    </div>
</asp:Content>

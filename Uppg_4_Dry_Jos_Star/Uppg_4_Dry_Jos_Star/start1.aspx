<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="start1.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.start1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <title>START</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="startButtons.js"></script>
    <div class="fullBox" id="page-title">
        <h1>START</h1>
    </div>
    <div class="fullBox">
        <div class="halfBox" >
            <h2 class="home-h2" id="test">Välkommen <asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label></h2>
            <p class="home-text">Välkommen till JE Bankens kompetensportal. På denna sida har du som anställd möjlighet att göra 
                ditt licencieringstest som nyanställd och som licensierad anställd har du möjlighet att göra ditt årliga 
                kunskapstest. 
                <br />
                <br />
            </p>
            <p class="home-text">Om du upplever några problem med kompetensportalen eller saknar prov att göra eller se. Kontakta då JE bankens IT-support för hjälp.
            </p>
        </div>
        <div class="halfBox">
            <div class="fullBox">
                <img class="home-img" src="img/mote.jpg" />
            </div>
        </div>
    </div>

    <div class="fullBox" >
        <div class="fullBox" id="page-title">
            <h2 class="home-h2">DINA PROV</h2>
        </div>
        <div class="halfBox center-text">
            <h3 class="home-h3" >PROVSTATUS</h3>
            <p class="home-text">Du har för närvarande <asp:Label ID="lbltestToDo" runat="server" Text="Label"></asp:Label> prov att genomföra.</p>
            <p class="home-text">Prov kan tidigast genomföras: <asp:Label ID="lblNextTestDate" runat="server" Text="Label"></asp:Label></p>
            <p class="home-text">Provtyp att göra: <asp:Label ID="lblTestType" runat="server" Text="Label"></asp:Label></p>
            <br />
            <asp:Button title="Klicka här för att starta ditt test. Har du inget test att göra är knappen avstängd." ID="btnStartTest" runat="server" Text="STARTA PROV" CssClass="my-button button-80" OnClick="Button1_Click" />
        </div>
        <div class="halfBox center-text">
            <h3 class="home-h3">DITT SENASTE PROV</h3>
            <p class="home-text">Provresultat: <asp:Label ID="lblresult" runat="server" Text="Label"></asp:Label></p>
            <p class="home-text">Provdatum: <asp:Label ID="lbldate" runat="server" Text="Label"></asp:Label></p>  
            <p class="home-text">Provtyp: <asp:Label ID="lblTestTypeDone" runat="server" Text="Label"></asp:Label></p>
            <br />
            <asp:Button title="Du kan bara se prov som du är godkänd på" ID="btnGoToOldTest" runat="server" Text="SE SENASTE PROVET" CssClass="my-button button-80"  OnClick="Button2_Click" />
        </div>
    </div>
</asp:Content>

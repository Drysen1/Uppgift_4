<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">
        <h1>Administration</h1>
    </div>
    <div class="fullBox">
        <div class="halfBox" >
            <h2 class="home-h2">Välkommen <asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label></h2>
            <p class="home-text">Välkommen till administationsdelen av JE Bankens kompetensportal. På denna sida kan du som provledare se när personerna i ditt team gjort Licenceringstestet och det årliga ÅKU.
               
                <br />
            </p>
            <p class="home-text">Du kan även titta statistiskt på ditt teams utförda prov.<br />
            </p>
            <p class="home-text">Om du upplever några problem med kompetensportalen eller saknar personer i ditt team eller prov. Kontakta då JE bankens IT-support för hjälp.
            </p>
        </div>
        <div class="halfBox">
            <div class="fullBox">
                <img class="home-img" src="img/mote.jpg" />
            </div>
        </div>
    </div>
    <div class="fullBox" >
        <div class="fullBox" id="page-body">
            <h2 class="home-h2">&nbsp;</h2>
        </div>
        <div class="halfBox" id="center-team">
            <h3 class="home-h3">Medarbetare</h3>
            <br/>
            <p class="home-text"></p>
            <div style="text-align:center;">
                <asp:Chart ID="Chart1" runat="server" Width="300" Height="300">
                    <Titles>
                        <asp:Title Name="title" Text="Teammedlemmar med test att utföra"></asp:Title>
                    </Titles>
                    <Series>
                        <asp:Series Name="series" ChartType="Pie"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" ></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="Button1" runat="server" Text="Se team" CssClass="my-button button-80" OnClick="Button1_Click" />
            </div>
        </div>
        <div class="halfBox" id="center-stats">
            <h3 class="home-h3">Statistik</h3>
            <p class="home-text">&nbsp;</p>
            <p class="home-text">&nbsp;</p> 
            <br />
            <br />           
            <asp:Button ID="Button2" runat="server" Text="Se statistik" CssClass="my-button button-80" OnClick="Button2_Click" />
        </div>
    </div>
</asp:Content>

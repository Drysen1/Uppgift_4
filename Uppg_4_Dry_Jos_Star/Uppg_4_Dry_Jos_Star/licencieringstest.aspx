<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="licencieringstest.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.licencieringstest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="fullBox" id="page-title1">
        <h1>Licensieringstest</h1>
    </div>
    <div class="fullBox">
       
        
    </div>

    <div class="fullBox" >
        <div class="fullBox" id="page-title2">
            <h2 class="home-h2">Översikt/Licencieringstest</h2>
            <p>Provdeltagare som ska göra licencieringstestet. 
                <br />
                <asp:GridView AutoGenerateColumns="False" ID="GridView1" runat="server">
                </asp:GridView>
                <br />
            </p>
        </div>
    </div>
</asp:Content>


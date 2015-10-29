<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="adminStats.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.adminStats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">
        <h1>Statistik</h1>
    </div>
    <div class="fullBox">
        <div id="gridCategegor1" class="fullBox" style="border: 1px solid black;">
            <h3>Etik och regelverk</h3>
            <asp:GridView ID="gViewStatsCategory1" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

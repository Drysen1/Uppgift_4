<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="adminStats.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.adminStats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">
        <h1>Statistik</h1>
    </div>
    <div class="fullBox">
        <div class="quarterBox" style="margin-bottom: 20px">
            <asp:Label ID="Label1" runat="server">Typ av prov:</asp:Label>
            <asp:DropDownList  ID="pickTestType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="pickTestType_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="sevenFiveBox" style="margin-bottom: 20px">
            <asp:Label ID="Label2" runat="server">Kategori:</asp:Label>
            <asp:DropDownList ID="pickTestCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="pickTestCategory_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div id="gridCategory" class="fullBox" style="border: 1px solid black;">
            <h3 id="header" runat="server"></h3>
            <asp:GridView ID="gViewStatsCategory" runat="server" OnRowDataBound="gViewStatsCategory_RowDataBound">
            </asp:GridView>
        </div>
    </div>
</asp:Content>

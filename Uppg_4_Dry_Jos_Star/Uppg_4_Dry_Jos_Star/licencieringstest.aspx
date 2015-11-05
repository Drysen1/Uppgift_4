<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="licencieringstest.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.licencieringstest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jquery.responsivetable.js"></script>
    <script src="js/screensize.js"></script>
    <script>
        $(document).ready(function () {
            $('table').responsiveTable({
                staticColumns: 0
            });
        });
    </script>

    <div class="fullBox" id="page-title">
        <h1>ÖVERSIKT</h1>
    </div>
    <div class="fullBox" id="body-content">
        <p class="home-text">
            Filtrera: <asp:DropDownList ID="DropDownList1" runat="server" CssClass="DropDown" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </p>
        <p class="home-text">
            <br />
            LST = Licensieringstest
            <br />
            ÅKU = Årligt kunskapsprov
        </p>
        <br />
        <asp:GridView ID="GridView1" CssClass="lst-grid" runat="server" OnRowDataBound="GridView1_RowDataBound">
        </asp:GridView>
        <br />
    </div>
</asp:Content>


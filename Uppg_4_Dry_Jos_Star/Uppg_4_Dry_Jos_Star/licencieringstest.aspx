﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="licencieringstest.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.licencieringstest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="fullBox" id="page-title">
        <h1>ÖVERSIKT</h1>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script src="js/jquery.responsivetable.js"></script>
            <script src="js/screensize.js"></script>
            <script>
                $(document).ready(function () {
                    $('table').responsiveTable({
                        staticColumns: 0
                    });
                });
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                    $('table').responsiveTable({
                        staticColumns: 0
                    });
                });
            </script>
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
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>


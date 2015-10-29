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
            <p>Provdeltagare som ska göra licencieringstestet. </p>
                <br />
                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged1">
                </asp:GridView>
                <br />
            </p>
        </div>
    </div>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="aku.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.aku" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="fullBox" id="page-title1">
        <h1>Årlig kunskapsuppdatering</h1>
    </div>
    <div class="fullBox">
       
        
    </div>

    <div class="fullBox" >
        <div class="fullBox" id="page-title2">
            <p>Provdeltagare som ska göra årlig kunskapsuppdatering. </p>
                <br />
                <asp:GridView ID="GridView11" runat="server" OnRowDataBound="GridView11_RowDataBound1"  >
                </asp:GridView>
                <br />
        </div>
    </div>
</asp:Content>
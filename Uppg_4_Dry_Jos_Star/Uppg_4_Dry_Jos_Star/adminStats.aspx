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
        <div id="gridCategory" class="fullBox">
            <h3 id="header" runat="server"></h3>
            <asp:GridView ID="gViewStatsCategory" runat="server" OnRowDataBound="gViewStatsCategory_RowDataBound" Width="1200px">
            </asp:GridView>
        </div>
        <div class="fullBox" style="margin-top: 50px">
            <asp:Chart ID="chartTotalStats" runat="server" Width="1200px">
                <Titles>
                    <asp:Title Name="title" Text="Stapeldiagram över alla frågor oavsett provtyp"></asp:Title>
                </Titles>
                <Legends>
                    <asp:Legend Name="legend" >
                    </asp:Legend>
                </Legends>
                <Series>
                    <asp:Series Name="correctAnswers" IsValueShownAsLabel="True"></asp:Series>
                    <asp:Series Name="totalAnswers" IsValueShownAsLabel="True"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="chartArea">
                        <AxisX Interval="1" Title="Fråga" TitleAlignment="Near" ></AxisX>
                        <AxisY Interval="1" Title="Antal"></AxisY>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
    </div>
</asp:Content>

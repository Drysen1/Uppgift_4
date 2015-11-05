<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="adminStats.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.adminStats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>STATISTIK</title>
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
        <h1>STATISTIK</h1>
    </div>
    <div class="fullBox">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="fullBox dropDown-Box">
                    <div class="quarterBox">
                        <p class="home-text">
                            <asp:Label ID="Label1" runat="server">Välj typ av prov:</asp:Label>
                            <asp:DropDownList  ID="pickTestType" CssClass="DropDown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="pickTestType_SelectedIndexChanged"></asp:DropDownList>
                        </p>
                    </div>

                    <div class="sevenFiveBox" >
                        <p class="home-text">
                            <asp:Label ID="Label2" runat="server">Välj kategori:</asp:Label>            
                            <asp:DropDownList ID="pickTestCategory" CssClass="DropDown DropDown-75" runat="server" AutoPostBack="True" OnSelectedIndexChanged="pickTestCategory_SelectedIndexChanged"></asp:DropDownList>
                        </p>
                    </div>
                </div>

                <div id="gridCategory" class="fullBox">
                    <h3 id="header" runat="server"></h3>
                    <asp:GridView ID="gViewStatsCategory" runat="server" OnRowDataBound="gViewStatsCategory_RowDataBound" RowStyle-Wrap="false" CssClass="Grid">

                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="fullBox chart-box" id="page-title">
            <h2 class="home-h2">STAPELDIAGRAM</h2>
        </div>
        <div class="fullBox chart-box center-text">
            <div class="fullBox"></div>
                <asp:Chart ID="chartTotalStats" runat="server" Width="1024px">
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

    </div>
</asp:Content>

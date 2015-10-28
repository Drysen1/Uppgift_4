<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="testPage.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star._default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">        
        <h1>KOMPETENSPORTALEN</h1>
    </div>
    <div id="bodyContent" class="fullBox" runat="server"> 
         <div id="finalResult" class="fullBox" runat="server"> 
             <div class="quarterBox">
                <asp:Chart ID="totalChart" CssClass="home-img" runat="server" Width="300" Height="300">
                 <Titles>
                     <asp:Title Text=""></asp:Title>
                 </Titles>
                 <Series>
                     <asp:Series Name="seriesTotal" ChartType="Pie"></asp:Series>
                 </Series>
                 <ChartAreas>
                     <asp:ChartArea Name="chartAreaTotal"></asp:ChartArea>
                 </ChartAreas>
             </asp:Chart>

             </div>       
            <div class="quarterBox">
                    <asp:Chart ID="categoryChart1" CssClass="home-img"  runat="server" Width="300" Height="300">
                 <Titles>
                     <asp:Title Text=""></asp:Title>
                 </Titles>
                 <Series>
                     <asp:Series Name="seriesCategory1" ChartType="Pie"></asp:Series>
                 </Series>
                 <ChartAreas>
                     <asp:ChartArea Name="chartAreaCategory1"></asp:ChartArea>
                 </ChartAreas>
             </asp:Chart>
            </div> 
            <div class="quarterBox">
                <asp:Chart ID="categoryChart2" CssClass="home-img" runat="server" Width="300" Height="300">
                 <Titles>
                     <asp:Title Text=""></asp:Title>
                 </Titles>
                 <Series>
                     <asp:Series Name="seriesCategory2" ChartType="Pie"></asp:Series>
                 </Series>
                 <ChartAreas>
                     <asp:ChartArea Name="chartAreaCategory2"></asp:ChartArea>
                 </ChartAreas>
             </asp:Chart>
            </div> 
            <div class="quarterBox">
                             <asp:Chart ID="categoryChart3" CssClass="home-img" runat="server" Width="300" Height="300">
                 <Titles>
                     <asp:Title Text=""></asp:Title>
                 </Titles>
                 <Series>
                     <asp:Series Name="seriesCategory3" ChartType="Pie"></asp:Series>
                 </Series>
                 <ChartAreas>
                     <asp:ChartArea Name="chartAreaCategory3"></asp:ChartArea>
                 </ChartAreas>
             </asp:Chart>
            </div> 




        </div>
        <div id="repeaters" class="fullBox" runat="server"> 
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <div id="category_container" style="border:1px solid black; margin: 20px 0;">
                        <asp:Label ID="categoryText" style="font-size: 30px; font-weight: bold;" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                        <div style="margin:20px 0;">
                            <asp:Label ID="questionId" style="font-weight:bold;" Text=<%# Eval("AnswerOrder") %> runat="server"></asp:Label>
                            <asp:Label ID="question" style="font-weight:bold;" text=<%# Eval("Text") %> runat="server"></asp:Label>
                            <div style="margin: 5px 0 10px 0;">
                                <asp:Label ID="numOfcorrect" style="font-style: italic;" Text=<%# Eval("NumOfCorrect") %> runat="server"></asp:Label>
                            </div>
                            <table>
                                <tr>
                                    <td><asp:CheckBox ID="cBox1" class=<%# Eval("CssClasses[0]") %> runat="server" Text=<%# Eval("Answers[0]") %> /></td>
                                </tr> 
                                <tr>
                                    <td><asp:CheckBox ID="cBox2" class=<%# Eval("CssClasses[1]") %> runat="server" Text=<%# Eval("Answers[1]") %> /></td>
                                </tr> 
                                <tr>
                                    <td><asp:CheckBox ID="CBox3" class=<%# Eval("CssClasses[2]") %> runat="server" Text=<%# Eval("Answers[2]") %> /></td>
                                </tr> 
                            </table>
                        </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Repeater2_ItemDataBound">
                <HeaderTemplate>
                    <div id="category_container" style="border:1px solid black; margin: 20px 0;">
                        <asp:Label ID="categoryText" style="font-size: 30px; font-weight: bold;" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                        <div style="margin:20px 0;">
                            <asp:Label ID="questionId" style="font-weight:bold;" Text=<%# Eval("AnswerOrder") %> runat="server"></asp:Label>
                            <asp:Label ID="question" style="font-weight:bold;" Text=<%# Eval("Text") %> runat="server"></asp:Label>
                            <div style="margin: 5px 0 10px 0;">
                                <asp:Label ID="numOfcorrect" style="font-style: italic;" Text=<%# Eval("NumOfCorrect") %> runat="server"></asp:Label>
                            </div>
                            <table>
                                <tr>
                                    <td><asp:CheckBox ID="cBox1" class=<%# Eval("CssClasses[0]") %> runat="server" Text=<%# Eval("Answers[0]") %> /></td>
                                </tr> 
                                <tr>
                                    <td><asp:CheckBox ID="cBox2" class=<%# Eval("CssClasses[1]") %> runat="server" Text=<%# Eval("Answers[1]") %> /></td>
                                </tr> 
                                <tr>
                                    <td><asp:CheckBox ID="CBox3" class=<%# Eval("CssClasses[2]") %> runat="server" Text=<%# Eval("Answers[2]") %> /></td>
                                </tr> 
                            </table>
                        </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="Repeater3" runat="server" OnItemDataBound="Repeater3_ItemDataBound">
                <HeaderTemplate>
                    <div id="category_container" style="border:1px solid black; margin: 20px 0;">
                        <asp:Label ID="categoryText" style="font-size: 30px; font-weight: bold;" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                        <div style="margin:20px 0;">
                            <asp:Label ID="questionId" style="font-weight:bold;" Text=<%# Eval("AnswerOrder") %> runat="server"></asp:Label>
                            <asp:Label ID="question" style="font-weight:bold;" Text=<%# Eval("Text") %> runat="server"></asp:Label>
                            <div style="margin: 5px 0 10px 0;">
                                <asp:Label ID="numOfcorrect" style="font-style: italic;" Text=<%# Eval("NumOfCorrect") %> runat="server"></asp:Label>
                            </div>
                            <table>
                                <tr>
                                    <td><asp:CheckBox ID="cBox1" class=<%# Eval("CssClasses[0]") %> runat="server" Text=<%# Eval("Answers[0]") %> /></td>
                                </tr> 
                                <tr>
                                    <td><asp:CheckBox ID="cBox2" class=<%# Eval("CssClasses[1]") %> runat="server" Text=<%# Eval("Answers[1]") %> /></td>
                                </tr> 
                                <tr>
                                    <td><asp:CheckBox ID="CBox3" class=<%# Eval("CssClasses[2]") %> runat="server" Text=<%# Eval("Answers[2]") %> /></td>
                                </tr> 
                            </table>
                        </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <asp:Button ID="btnSend" runat="server" Text="Lämna in" OnClick="btnSend_Click"/>
    </div>
</asp:Content>

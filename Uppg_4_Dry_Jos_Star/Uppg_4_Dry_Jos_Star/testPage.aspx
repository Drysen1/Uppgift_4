﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TestMaster.Master" AutoEventWireup="true" CodeBehind="testPage.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star._default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1830000"></asp:Timer>


    <!-- DIV AND HOLDER FOR TIMER ======================================================================= -->
     <div class="timer-holder">
          <p id="timer">30:00</p>
      </div>
    <!-- CONTENT START ======================================================================= -->
    <div class="fullBox" id="page-title">        
        <h1>PROV</h1>
    </div>
    <div class="fullBox"> 
        <div class="halfBox center-text">       
            <h3 class="home-h3">PROVDELTAGARE: <asp:Label ID="lblUserName1" runat="server" Text="Label"></asp:Label></h3>
        </div>
        <div class="halfBox center-text">        
            <h3 class ="home-h3">PROVTYP: <asp:Label ID="lblTestType" runat="server" Text="Label"></asp:Label></h3>
        </div>
    </div>

    <!-- PIE CHARTS ======================================================================= -->
    <div id="bodyContent" class="fullBox" runat="server">
        <div id="finalResult" class="fullBox result-box" runat="server">
            <div class="fullBox" id="page-title">
                <h2 class="home-h2">PROVRESULTAT</h2>
            </div>
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
                <asp:Label Style="font-weight: bold" ID="resultTotal" runat="server" Text="Label"></asp:Label>
            </div>
            <div class="quarterBox center-text">
                <asp:Chart ID="categoryChart1" CssClass="home-img" runat="server" Width="300" Height="300">
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
                <asp:Label ID="resultCategory1" runat="server" Text="Label"></asp:Label>
            </div>
            <div class="quarterBox center-text">
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
                <asp:Label ID="resultCategory2" runat="server" Text="Label"></asp:Label>
            </div>
            <div class="quarterBox center-text">
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
                <asp:Label ID="resultCategory3" runat="server" Text="Label"></asp:Label>
            </div>
            <div class="fullBox">
                <h3 class="category-h3"><asp:Label ID="testPassed" runat="server" >Provresultat:</asp:Label> <asp:Image  ID="yesNoImg"  runat="server"/></h3>

            </div>
        </div>

        <!-- REPEATERS ======================================================================= -->
        <div id="repeaters" class="fullBox" runat="server"> 
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <div id="category_container" class="fullBox" >
                        <div class="fullBox" id="page-title">
                            <h3 class="category-h3"><asp:Label ID="categoryText" runat="server"></asp:Label></h3>
                        </div>
                </HeaderTemplate>

                <ItemTemplate>
                        <div class="fullBox">
                            <div class="fullBox">
                                <h4 class="category-h4">
                                    <asp:Label ID="questionId" Text=<%# Eval("AnswerOrder") %> runat="server"></asp:Label>
                                    <asp:Label ID="question" text=<%# Eval("Text") %> runat="server"></asp:Label>
                                    <asp:Image ID="questionImage" ImageUrl=<%# Eval("AnswerImageUrl") %> runat="server" />
                                </h4>
                            </div>
                            <div class="fullBox">
                                <p class="num-text"><asp:Label ID="numOfcorrect" Text=<%# Eval("NumOfCorrect") %> runat="server"></asp:Label></p>
                            </div>
                            <div class="fullBox">
                                <asp:Image ID="questionPicture" CssClass="test-img" ImageUrl=<%# Eval("QuestionPictureUrl") %> runat="server" />
                            </div>
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="cBox1" class=<%# Eval("CssClasses[0]") %> runat="server" Text=<%# Eval("Answers[0]") %> />
                                </div> 
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="cBox2" class=<%# Eval("CssClasses[1]") %> runat="server" Text=<%# Eval("Answers[1]") %> />
                                </div> 
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="CBox3" class=<%# Eval("CssClasses[2]") %> runat="server" Text=<%# Eval("Answers[2]") %> />
                                </div> 
                        </div>
                </ItemTemplate>

                <FooterTemplate>
                    </div>
                </FooterTemplate>

            </asp:Repeater>

            <asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Repeater2_ItemDataBound">
                <HeaderTemplate>
                    <div id="category_container"  class="fullBox">
                        <div class="fullBox" id="page-title">
                            <h3 class="category-h3"><asp:Label ID="categoryText" runat="server"></asp:Label></h3>
                        </div>
                </HeaderTemplate>

                <ItemTemplate>
                        <div class="fullBox">
                            <div class="fullBox">
                                <h4 class="category-h4">
                                    <asp:Label ID="questionId" Text=<%# Eval("AnswerOrder") %> runat="server"></asp:Label>
                                    <asp:Label ID="question" Text=<%# Eval("Text") %> runat="server"></asp:Label>
                                    <asp:Image ID="questionImage" ImageUrl=<%# Eval("AnswerImageUrl") %> runat="server" />
                                </h4>
                            </div>                            
                            <div class="fullBox">
                                <p class="num-text"><asp:Label ID="numOfcorrect" Text=<%# Eval("NumOfCorrect") %> runat="server"></asp:Label></p>
                            </div>
                            <div>
                                <asp:Image ID="questionPicture" CssClass="test-img" ImageUrl=<%# Eval("QuestionPictureUrl") %> runat="server" />
                            </div>
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="cBox1" class=<%# Eval("CssClasses[0]") %> runat="server" Text=<%# Eval("Answers[0]") %> />
                                </div> 
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="cBox2" class=<%# Eval("CssClasses[1]") %> runat="server" Text=<%# Eval("Answers[1]") %> />
                                </div> 
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="CBox3" class=<%# Eval("CssClasses[2]") %> runat="server" Text=<%# Eval("Answers[2]") %> />
                                </div> 
                        </div>
                </ItemTemplate>

                <FooterTemplate>
                    </div>
                </FooterTemplate>

            </asp:Repeater>

            <asp:Repeater ID="Repeater3" runat="server" OnItemDataBound="Repeater3_ItemDataBound">
                <HeaderTemplate>
                    <div id="category_container" class="fullBox">
                        <div class="fullBox" id="page-title">
                            <h3 class="category-h3"><asp:Label ID="categoryText" runat="server"></asp:Label></h3>
                        </div>
                </HeaderTemplate>

                <ItemTemplate>
                        <div class="fullBox">
                            <div class="fullBox">
                                <h4 class="category-h4">
                                    <asp:Label ID="questionId" Text=<%# Eval("AnswerOrder") %> runat="server"></asp:Label>
                                    <asp:Label ID="question" Text=<%# Eval("Text") %> runat="server"></asp:Label>
                                    <asp:Image ID="questionImage" ImageUrl=<%# Eval("AnswerImageUrl") %> runat="server" />
                                </h4>
                            </div>
                            <div class="fullBox">
                                <p class="num-text"><asp:Label ID="numOfcorrect" Text=<%# Eval("NumOfCorrect") %> runat="server"></asp:Label></p>
                            </div>
                            <div>
                                <asp:Image ID="questionPicture" CssClass="test-img" ImageUrl=<%# Eval("QuestionPictureUrl") %> runat="server" />
                            </div>
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="cBox1" class=<%# Eval("CssClasses[0]") %> runat="server" Text=<%# Eval("Answers[0]") %> />
                                </div> 
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="cBox2" class=<%# Eval("CssClasses[1]") %> runat="server" Text=<%# Eval("Answers[1]") %> />
                                </div> 
                                <div class="fullBox checkbox-style">
                                    <asp:CheckBox ID="CBox3" class=<%# Eval("CssClasses[2]") %> runat="server" Text=<%# Eval("Answers[2]") %> />
                                </div> 
                        </div>
                </ItemTemplate>

                <FooterTemplate>
                    </div>
                </FooterTemplate>

            </asp:Repeater>
        </div>

        <!-- BUTTON ======================================================================= -->
        <asp:Button ID="btnSend" runat="server" CssClass="my-button button-80" Text="Lämna in" OnClick="btnSend_Click"/>
    </div>
</asp:Content>

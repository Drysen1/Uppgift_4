﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">        
        <h1>KOMPETENSPORTALEN</h1>
    </div>
    <div class="fullBox">  
        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
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
                                <td><asp:CheckBox ID="cBox1" runat="server" /></td>
                                <td><asp:Label ID="answerText1" Text=<%# Eval("Answers[0]") %> runat="server" ></asp:Label></td>
                            </tr> 
                            <tr>
                                <td><asp:CheckBox ID="cBox2" runat="server" /></td>
                                <td><asp:Label ID="answerText2" Text=<%# Eval("Answers[1]") %> runat="server"></asp:Label></td>
                            </tr> 
                            <tr>
                                <td><asp:CheckBox ID="CBox3" runat="server" /></td>
                                <td><asp:Label ID="answerText3" Text=<%# Eval("Answers[2]") %> runat="server"></asp:Label></td>
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
                                <td><asp:CheckBox ID="cBox1" runat="server" /></td>
                                <td><asp:Label ID="answerText1" Text=<%# Eval("Answers[0]") %> runat="server" ></asp:Label></td>
                            </tr> 
                            <tr>
                                <td><asp:CheckBox ID="cBox2" runat="server" /></td>
                                <td><asp:Label ID="answerText2" Text=<%# Eval("Answers[1]") %> runat="server"></asp:Label></td>
                            </tr> 
                            <tr>
                                <td><asp:CheckBox ID="CBox3" runat="server" /></td>
                                <td><asp:Label ID="answerText3" Text=<%# Eval("Answers[2]") %> runat="server"></asp:Label></td>
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
                                <td><asp:CheckBox ID="cBox1" runat="server" /></td>
                                <td><asp:Label ID="answerText1" Text=<%# Eval("Answers[0]") %> runat="server" ></asp:Label></td>
                            </tr> 
                            <tr>
                                <td><asp:CheckBox ID="cBox2" runat="server" /></td>
                                <td><asp:Label ID="answerText2" Text=<%# Eval("Answers[1]") %> runat="server"></asp:Label></td>
                            </tr> 
                            <tr>
                                <td><asp:CheckBox ID="CBox3" runat="server" /></td>
                                <td><asp:Label ID="answerText3" Text=<%# Eval("Answers[2]") %> runat="server"></asp:Label></td>
                            </tr> 
                        </table>
                    </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

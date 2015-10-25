<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="testPage.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">        
        <h1>KOMPETENSPORTALEN</h1>
    </div>
    <div id="bodyContent" class="fullBox" runat="server"> 
         <div id="finalResult" class="fullBox" runat="server">        
             <asp:Label ID="result" runat="server" Text="Label"></asp:Label>
        </div>
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
        <asp:Button ID="btnSend" runat="server" Text="Lämna in" OnClick="btnSend_Click"/>
    </div>
</asp:Content>

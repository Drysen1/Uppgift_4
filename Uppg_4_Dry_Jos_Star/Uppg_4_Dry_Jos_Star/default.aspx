<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">        
        <h1>KOMPETENSPORTALEN</h1>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <div style="border:1px solid black; margin:20px 0;">
                    <asp:Label ID="questionId" style="font-weight:bold" Text=<%# Eval("QuestionNumber") %> runat="server"></asp:Label>
                    <asp:Label ID="question" style="font-weight:bold" Text=<%# Eval("Text") %> runat="server"></asp:Label>
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
        </asp:Repeater>
    </div>
</asp:Content>

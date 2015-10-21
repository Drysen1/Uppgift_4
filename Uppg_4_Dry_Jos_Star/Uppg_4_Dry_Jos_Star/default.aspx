<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox" id="page-title">        
        <h1>KOMPETENSPORTALEN</h1>
        <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate></HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="question" runat="server" Text=<%# Eval("Text") %>></asp:Label>
                <table>
                    <tr>
                        <td><asp:CheckBox ID="cBox1" runat="server" /></td>
                        <td><asp:Label ID="Label1" runat="server" Text=<%# Eval("Answers[0]") %>></asp:Label></td>
                    </tr> 
                    <tr>
                        <td><asp:CheckBox ID="cBox2" runat="server" /></td>
                        <td><asp:Label ID="answerText2" runat="server" Text=<%# Eval("Answers[1]") %>></asp:Label></td>
                    </tr> 
                    <tr>
                        <td><asp:CheckBox ID="CBox3" runat="server" /></td>
                        <td><asp:Label ID="answerText3" runat="server" Text=<%# Eval("Answers[2]") %>></asp:Label></td>
                    </tr> 
                </table>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

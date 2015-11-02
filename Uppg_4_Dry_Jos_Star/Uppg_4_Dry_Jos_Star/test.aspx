<%@ Page Title="" Language="C#" MasterPageFile="~/TestMaster.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="timer-holder">
          <p id="timer">clock</p>
      </div>
    <div class="fullBox">


        <asp:Label ID="lbltestitest" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="lblTypeOfTest" runat="server" Text="Label"></asp:Label>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick1" Interval="180000"></asp:Timer>
            <!--<asp:GridView ID="GridView1" runat="server"></asp:GridView>-->
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label><br />
            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label><br />
            <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label><br />
            <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label><br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />       
        </div>
</asp:Content>

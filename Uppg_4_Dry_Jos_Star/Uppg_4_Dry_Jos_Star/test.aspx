<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Uppg_4_Dry_Jos_Star.test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullBox">
<%--        <script>
            var count = 30;

            var counter = setInterval(timer, 1000);

            function timer()
            {
                count = count - 1;
                document.getElementById("clock").innerHTML = count;
                if(count <= 0)
                {
                    clearInterval(counter);
                    alert("Slut!");
                    return;
                }

            }--%>
            
        </script>

        <p id="clock"></p>
       <!--         <noscript>
          <style type="text/css">
        .pagecontainer {display:none;}
    </style>

    <div class="noscriptmsg">
    You don't have javascript enabled.  Good luck with that.
    </div>
    </noscript>

    <div class="pagecontainer">


        -->
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick1" Interval="5000"></asp:Timer>
        <!--<asp:GridView ID="GridView1" runat="server"></asp:GridView>-->
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label><br />
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label><br />
        <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label><br />
        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label><br />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        
    <!--    </div> -->

    </div>
</asp:Content>

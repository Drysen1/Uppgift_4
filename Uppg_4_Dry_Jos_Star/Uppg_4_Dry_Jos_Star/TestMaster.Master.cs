//TestMaster.Master.cs Code behind for TestMaster.Master
//2015-10-28 Erik Drysén

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using Npgsql;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class TestMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// To redircet user to adminpage and send username to that page.
        /// Currently only test purpose with response.write.
        /// </summary>
        protected void adminButton_ServerClick(object sender, EventArgs e)
        {
            var test = this.ContentPlaceHolder1.FindControl("lblUserName") as Label;
            string test1 = test.Text;
            Response.Write(test1);
        }

        /// <summary>
        /// Button event for user to click if use exceeds the time limit of the test.
        /// Calls method failuser and redirects to start1.aspx.
        /// </summary>
        protected void Button_Go_To_Start_Click(object sender, EventArgs e)
        {
            DatabaseConnection dc = new DatabaseConnection();
            var labelUserName1 = this.ContentPlaceHolder1.FindControl("lblUserName1") as Label;
            var labelTestType = this.ContentPlaceHolder1.FindControl("lblTestType") as Label;
            string userName = labelUserName1.Text;
            string typeOfTest = labelTestType.Text;
            dc.FailUser(userName, typeOfTest);
            Response.Redirect("~/start1.aspx");
        }
    }
}
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
            var test = this.ContentPlaceHolder1.FindControl("lblUserName") as Label;
            string test1 = test.Text;
            Response.Write(test1);
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
            //Response.Redirect("~/admin.aspx?userName=" + test1);
        }

        /// <summary>
        /// Button event for user to click if use exceeds the time limit of the test.
        /// Calls method failuser and redirects to start1.aspx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Go_To_Start_Click(object sender, EventArgs e)
        {
            FailUser();
            var labelUserName = this.ContentPlaceHolder1.FindControl("lblUserName") as Label;
            string userName = labelUserName.Text;
            //Response.Write(userName); //Test purpose
            Response.Redirect("~/start1.aspx");
        }


        /// <summary>
        /// Method sends data to database and fails current user if 
        /// the time for the test has been exceeded.
        /// Code also te be found with slight modification in test.aspx.cs
        /// </summary>
        private void FailUser()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Database=kompetensportal;Server=localhost;User Id=postgres;Password=anna;");
            var labelUserName = this.ContentPlaceHolder1.FindControl("lblUserName") as Label;
            string userName = labelUserName.Text;
            DateTime dateNow = DateTime.Now;
            try
            {
                conn.Open();
                NpgsqlCommand cmdUpdateAndFailUser = new NpgsqlCommand("UPDATE testoccasion " +
                                                                 "SET passed = false, date = @date " +
                                                                 "WHERE id_user = (SELECT id FROM person WHERE username = @userName);", conn);
                cmdUpdateAndFailUser.Parameters.AddWithValue("@userName", userName);
                cmdUpdateAndFailUser.Parameters.AddWithValue("@date", dateNow);
                cmdUpdateAndFailUser.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
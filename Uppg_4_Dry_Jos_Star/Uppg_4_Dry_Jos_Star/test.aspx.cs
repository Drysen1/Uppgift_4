using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Data;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IsPageReloadAllowed"] == null ) 
            {
                Label1.Text = "Första gång";
                Label2.Text = DateTime.Now.ToString();
                Session["StartTime"] = DateTime.Now; //When test is started starttime is saved in session.
                Session["IsPageReloadAllowed"] = false;
            }
            else if (Session["IsPageReloadAllowed"] != null && Session["IsFirstTime"] == null ) 
            {
                Label1.Text = "Andra gång"; //Test purpose.

            }

            string userName = Request.QueryString["userName"];
            string typeOfTest = Request.QueryString["typeOfTest"];
            lblUserName.Text = userName;
            lblTypeOfTest.Text = typeOfTest;

        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Tick event for timer. Intervall at 180000 atm, which is 3 minutes.
        /// If testtime is exceded this event triggers and calls method and redirects to other page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer1_Tick1(object sender, EventArgs e)
        {
            FailUser();
            Response.Redirect("~/start1.aspx");
        }

        /// <summary>
        /// Method sends data to database and fails current user if 
        /// the time for the test has been exceeded. Method is triggered from
        /// timer1_tick which has an intervall for the moment of 180000 which is 
        /// 3 minutes. Do 31 minutes in live scenario.
        /// Code also te be found with slight modification in testmaster.cs
        /// </summary>
        private void FailUser()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Database=kompetensportal;Server=localhost;User Id=postgres;Password=anna;");
            string userName = lblUserName.Text;
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

        /// <summary>
        /// This is pretty much for test scenario to be implemented in testPage.aspx.
        /// Simulates that a user has finished the test but has turned off javascript
        /// and somehow still managed to get thourgh to the testpage.
        /// Retrevies value from session["startTime"] and compares to the time
        /// that the user clicked the submit button. If more than 30 
        /// it should call the failuser method. REMEBER TO IMPLEMENT THIS BEFORE LIVE.
        /// 
        /// Lots of labels also for test purpose.
        /// </summary>
        protected void Button1_Click(object sender, EventArgs e)
        {
            DateTime TheStartTime = Convert.ToDateTime(Session["StartTime"]);
            DateTime endTime = DateTime.Now;
            
            Label5.Text = TheStartTime.ToString(); //TEstpurpose
            Label3.Text = endTime.ToString(); //Test purpose


            TimeSpan elapsedTime = endTime - TheStartTime;
            TimeSpan twoMinutes = TimeSpan.FromMinutes(2); //Change name to thirtyMinutes and FromMInutes(30)
            int result = elapsedTime.CompareTo(twoMinutes);

            if(result == 1)
            {
                Label4.Text = "Mer än 30 minuter";
            }
            else
            {
                Label4.Text = "MIndre än 30 minuter";
            }                     
        }


        //Test purpose.
        private void SetLabel3Time()
        {
            Label3.Text = DateTime.Now.ToLongTimeString();
        }

        //Test purpose.
        //private void LoggedInUserInfo()
        //{
        //    NpgsqlConnection conn = new NpgsqlConnection("Database=kompetensportal;Server=localhost;User Id=postgres;Password=anna;"); 
        //    int userId = 2;
        //    int userId2 = 2;//Change id here to get the user you want to find.
        //    //Assume id or something has been passed from log in page in order to retrieve correct info.
        //    //This is only for simulations purpose for this iteration.
        //    try
        //    {
        //        conn.Open();
        //        NpgsqlCommand cmdGetUserInfo = new NpgsqlCommand("SELECT * FROM person, testoccasion WHERE id_user = @id AND person.id = @id2; ", conn);
        //        cmdGetUserInfo.Parameters.AddWithValue("@id", userId);
        //        cmdGetUserInfo.Parameters.AddWithValue("@id2", userId2);
        //        NpgsqlDataAdapter nda = new NpgsqlDataAdapter();
        //        nda.SelectCommand = cmdGetUserInfo;
        //        DataSet ds = new DataSet();
        //        nda.Fill(ds);
        //        DataTable dt = ds.Tables[0];

        //        Label1.Text = dt.Rows[0]["passed"].ToString();

        //        GridView1.DataSource = dt;
        //        GridView1.DataBind();

        //    }
        //    catch (NpgsqlException ex)
        //    {
        //        Response.Write(ex.Message);

        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
    }
}
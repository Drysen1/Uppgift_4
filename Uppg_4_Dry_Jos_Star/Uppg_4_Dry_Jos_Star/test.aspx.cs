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
        //DateTime startTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            //SetStartTime();

            if (Session["IsPageReloadAllowed"] == null ) //gets instantiated first time
            {
                Label1.Text = "Första gång";
                Label2.Text = DateTime.Now.ToString();
                Session["StartTime"] = DateTime.Now;
                Session["IsPageReloadAllowed"] = false;
            }
            else if (Session["IsPageReloadAllowed"] != null && Session["IsFirstTime"] == null ) //page reload before button has been clicked
            {
                Label1.Text = "Andra gång";

            }
            //string userName = Request.QueryString["userName"];
            ////Response.Write(userName);
            //Label1.Text = userName;
            ////LoggedInUserInfo();
        }

        //private void SetStartTime()
        //{
        //    startTime = DateTime.Now;
        //    Label2.Text = startTime.ToString();
        //}

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        private void TestTimerStuff()
        {
            
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //Label2.Text = DateTime.Now.ToLongTimeString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Session["StartTime"] = Label2.Text;
            //DateTime endTime = DateTime.Now;

            //TimeSpan elapsedTime = endTime - startTime;
            //TimeSpan oneMinute = TimeSpan.FromMinutes(2);


            DateTime TheStartTime = Convert.ToDateTime(Session["StartTime"]);
            Label5.Text = TheStartTime.ToString();
            DateTime endTime = DateTime.Now;
            Label3.Text = endTime.ToString();
            TimeSpan elapsedTime = endTime - TheStartTime;
            TimeSpan tenSec = TimeSpan.FromMinutes(2);
            int result = elapsedTime.CompareTo(tenSec);

            if(result == 1)
            {
                Label4.Text = "Mer än 30 minuter";
            }
            else
            {
                Label4.Text = "MIndre än 30 minuter";
            }
            




            //Label2.Text = startTime.ToString();
            //Label3.Text = endTime.ToString();
            //Label4.Text = Session["StartTime"].ToString();
            //int result = elapsedTime.CompareTo(oneMinute);

            //if (result == 1)
            //{
            //    Label2.Text = "Mer än 10 sekunder ";
            //}
            //else
            //{
            //    Label2.Text = "Mindre än 10 sekduner";
            //}     

            //Label2.Text = oneMinute.ToString();
            //Label3.Text = result.ToString();
            //if (elapsedTime.Minutes > oneMinute.Seconds)
            //{
            //    Label2.Text = "Mer än 10 sekunder ";
            //}
            //else
            //{
            //    Label2.Text = "Mindre än 10 sekduner";
            //}            
        }


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
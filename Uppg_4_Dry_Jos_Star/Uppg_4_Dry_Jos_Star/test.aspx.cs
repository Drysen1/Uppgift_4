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
            string userName = Request.QueryString["userName"];
            //Response.Write(userName);
            Label1.Text = userName;
            //LoggedInUserInfo();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        private void LoggedInUserInfo()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Database=kompetensportal;Server=localhost;User Id=postgres;Password=anna;"); 
            int userId = 2;
            int userId2 = 2;//Change id here to get the user you want to find.
            //Assume id or something has been passed from log in page in order to retrieve correct info.
            //This is only for simulations purpose for this iteration.
            try
            {
                conn.Open();
                NpgsqlCommand cmdGetUserInfo = new NpgsqlCommand("SELECT * FROM person, testoccasion WHERE id_user = @id AND person.id = @id2; ", conn);
                cmdGetUserInfo.Parameters.AddWithValue("@id", userId);
                cmdGetUserInfo.Parameters.AddWithValue("@id2", userId2);
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter();
                nda.SelectCommand = cmdGetUserInfo;
                DataSet ds = new DataSet();
                nda.Fill(ds);
                DataTable dt = ds.Tables[0];

                Label1.Text = dt.Rows[0]["passed"].ToString();

                GridView1.DataSource = dt;
                GridView1.DataBind();

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
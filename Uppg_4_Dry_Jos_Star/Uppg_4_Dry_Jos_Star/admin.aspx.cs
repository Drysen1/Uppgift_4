using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls; 

namespace Uppg_4_Dry_Jos_Star
{
	public partial class admin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if(!IsPostBack)
            {
                SetUpPage();
            }
		}

        private void SetUpPage()  //request.querystring[username]
        {
            string userName = Request.QueryString["userName"];
            lblUserName.Text = userName;
            PopulatePieChart(userName);
        }

        private void PopulatePieChart(string userName)
        {
            DatabaseConnection dr = new DatabaseConnection();
            string query = "SELECT firstname, lastname, testtype, date, score, passed, username " +
                            "FROM person p LEFT JOIN testoccasion t ON p.id = t.id_user " +
                            "WHERE id_testadmin = @userName ";
            
            dr.GetTeamMembers(query, userName);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string userName = Request.QueryString["userName"];
            Response.Redirect("~/licencieringstest.aspx"); 

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string userName = Request.QueryString["userName"];
            Response.Redirect("~/adminStats.aspx?userName=" + userName);
        }
	}
}
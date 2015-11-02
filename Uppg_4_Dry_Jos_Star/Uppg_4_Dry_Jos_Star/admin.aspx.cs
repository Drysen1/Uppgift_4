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
            string userName = Request.QueryString["userName"];
            lblUserName.Text = userName;

            //if (userType != "Admin")
            //{
            //    HtmlAnchor menuButton = (HtmlAnchor)Page.Master.FindControl("adminButton");
            //    HtmlAnchor menuButton1 = (HtmlAnchor)Page.Master.FindControl("a1");
            //    menuButton.Visible = false;
            //    menuButton1.Visible = false;
            //    // Response.Redirect("test.aspx");
            //}
            //else
            //{

            //}
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
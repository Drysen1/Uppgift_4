using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string authorisation = "admin";
            //if (authorisation == "admin")
            //{
            //    adminButton.Visible = true;
            //    a1.Visible = true;
            //}
            //else
            //{
            //    adminButton.Visible = false;
            //    a1.Visible = false;
            //}
        }

        protected void adminButton_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("admin.aspx");
            
        }
    }
}
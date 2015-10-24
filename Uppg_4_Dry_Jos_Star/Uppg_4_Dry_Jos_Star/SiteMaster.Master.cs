using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string authorisation = "admin";
            if (authorisation == "admin")
            {
                admin.Visible = true;
                a1.Visible = true;
            }
            else
            {
                admin.Visible = false;
                a1.Visible = false;
            }
        }

        void btnGoToAdmin_OnClick(object Source, EventArgs e)
        {
            
        }
    }
}
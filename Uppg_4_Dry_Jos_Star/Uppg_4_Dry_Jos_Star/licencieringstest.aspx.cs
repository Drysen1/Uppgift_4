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
    public partial class licencieringstest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // metoden ligger i DatabaseConnections 
            DatabaseConnection db = new DatabaseConnection(); 
            List<Person> personer = db.GetNoTestPersons();
           
            FillGrid(personer);
            //FillGrid(GetNoTestPersons()); alt 2
        }

        private void FillGrid(List<Person> people)
        {
            GridView1.DataSource = people;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Attributes["width"] = "150px";
            e.Row.Cells[1].Attributes["width"] = "150px";
            e.Row.Cells[2].Attributes["width"] = "150px";
            e.Row.Cells[3].Attributes["width"] = "150px";
            e.Row.Cells[4].Attributes["width"] = "150px";
            e.Row.Cells[5].Attributes["width"] = "150px";

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Förnamn";
                e.Row.Cells[1].Text = "Efternamn";
                e.Row.Cells[2].Text = "Datum";
                e.Row.Cells[3].Text = "Provpoäng";
                e.Row.Cells[4].Text = "Provresultat";
                e.Row.Cells[5].Text = "Användarnamn";
            }
        }
    }
}
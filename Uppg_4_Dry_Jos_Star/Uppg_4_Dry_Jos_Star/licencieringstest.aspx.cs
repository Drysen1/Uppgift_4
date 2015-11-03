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
        protected void Page_Load(object sender, EventArgs e) //request.querystring [username]
        {
            DatabaseConnection db = new DatabaseConnection();

            //string userName = Request.QueryString["userName"];
            string userName = "tomKar";
            string query = "SELECT firstname, lastname, testtype, date, score, passed, username " +
                            "FROM person p LEFT JOIN testoccasion t ON p.id = t.id_user " +
                            "WHERE id_testadmin = (SELECT id FROM person WHERE username = @userName) ";

            List<Person> personer = db.GetTeamMembers(query, userName);
           
            FillGrid(personer);


            if (!IsPostBack)
            {
                DropDownList();
                //Initialize();
            }

        }

        private void Initialize()
        {
            if (DropDownList1.Text == "")
            {
                // Alla i teamet 
            }
            else if (DropDownList1.Text == "")
            {
                //De som har ett test att genomföra
            }
            else if (DropDownList1.Text == "")
            {
                //De som har ett LST att genomföra
            }
            else if (DropDownList1.Text == "")
            {
                //De som har ett ÅKU att genomföra
            }
        }

        private void DropDownList()
        {
            DropDownList1.Items.Add("Alla");
            DropDownList1.Items.Add("Prov att göra");
            DropDownList1.Items.Add("LST");
            DropDownList1.Items.Add("ÅKU");
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
            e.Row.Cells[6].Attributes["width"] = "150px";

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Förnamn";
                e.Row.Cells[1].Text = "Efternamn";
                e.Row.Cells[2].Text = "Användarnamn";
                e.Row.Cells[3].Text = "Provtyp";
                e.Row.Cells[4].Text = "Datum";
                e.Row.Cells[5].Text = "Provresultat";
                e.Row.Cells[6].Text = "Betyg";
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           Initialize();
        }
    }
}
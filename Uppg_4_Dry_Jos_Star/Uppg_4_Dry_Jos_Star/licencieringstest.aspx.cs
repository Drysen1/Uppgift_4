﻿using System;
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


            if (!IsPostBack)
            {
                DropDownList();
                //Initialize();
            }

        }

        private void Initialize()
        {
            if (DropDownList1.SelectedValue == "")
            {
                //kod för att göra om värdet är sannt 
                // Alla i teamet 
            }
            else if (DropDownList1.SelectedValue == "")
            {
                //kod för att göra om värdet är sannt 
                //De som har ett test att genomföra
            }
        }

        private void DropDownList()
        {
            DropDownList1.Items.Add("Alla");
            DropDownList1.Items.Add("Prov att göra");
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
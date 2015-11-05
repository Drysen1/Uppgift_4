using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Data;
using System.Drawing; 

namespace Uppg_4_Dry_Jos_Star
{
    public partial class licencieringstest : System.Web.UI.Page
    {
        bool visibleTestWaitingColumn = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = GetUserName();
                PopulateDropDownList();

                List<Person> team = GetTeamMembers(userName);
                List<Person> personsWithUndoneTests = GetPersonsWithTestToDo(team);
                FillGrid(personsWithUndoneTests);
            }
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
            e.Row.Cells[7].Attributes["width"] = "150px";
            
            if (e.Row.RowType != DataControlRowType.Header && visibleTestWaitingColumn)
            {
                e.Row.Cells[7].BackColor = Color.MistyRose;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.BackColor = Color.BlanchedAlmond;
                e.Row.Cells[0].Text = "Förnamn";
                e.Row.Cells[1].Text = "Efternamn";
                e.Row.Cells[2].Text = "Användarnamn";
                e.Row.Cells[3].Text = "Senaste prov";
                e.Row.Cells[4].Text = "Datum";
                e.Row.Cells[5].Text = "Provresultat";
                e.Row.Cells[6].Text = "Betyg";
                e.Row.Cells[7].Text = "Väntande prov";
            }
            if(!visibleTestWaitingColumn)
            {
                e.Row.Cells[7].Visible = false;
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGridFromDropDown();
        }

        //---------------------------------------------------------------------------------------

        private string GetUserName()
        {
            string userName = Session["userName"].ToString();
            return userName;
        } //Session[userName]

        private void PopulateDropDownList()
        {
            DropDownList1.Items.Add("Prov väntar");
            DropDownList1.Items.Add("LST");
            DropDownList1.Items.Add("ÅKU");
            DropDownList1.Items.Add("Alla prov");
        }

        private List<Person> GetTeamMembers(string userName)
        {
            DatabaseConnection dr = new DatabaseConnection();
            string query = "SELECT firstname, lastname, testtype, date, score, passed, username " +
                            "FROM person p LEFT JOIN testoccasion t ON p.id = t.id_user " +
                            "WHERE id_testadmin = (SELECT id FROM person WHERE username = @userName) "+
                            "ORDER BY lastname, firstname, username, date DESC ";

            List<Person> teamMembers = dr.GetTeamMembers(query, userName);
            return teamMembers;
        }

        private List<Person> GetPersonsWithTestToDo(List<Person> team)
        {
            List<Person> personsWithUndoneTests = new List<Person>();
            List<Person> temp = new List<Person>();

            var queryResult = from p in team
                              orderby p.TestDate
                              group p by p.UserName;

            foreach (IGrouping<string, Person> pair in queryResult)
            {
                Person toDoTest = new Person();

                var ordered = pair.OrderByDescending(x => x.TestDate); //we want the last test, regardless how many there are in total
                Person lastTest = ordered.ElementAt(0);

                if (lastTest.TestDate == String.Empty)
                {
                    toDoTest = lastTest;
                    toDoTest.TestWaiting = "LST";
                    personsWithUndoneTests.Add(toDoTest);
                }
                else if (lastTest.TestType == "LST" && lastTest.TestGrade == "Underkänd")
                {
                    toDoTest = lastTest;
                    toDoTest.TestWaiting = "LST";
                    personsWithUndoneTests.Add(toDoTest);
                }
                else if (lastTest.TestType == "ÅKU" && lastTest.TestGrade == "Underkänd")
                {
                    toDoTest = lastTest;
                    toDoTest.TestWaiting = "ÅKU";
                    personsWithUndoneTests.Add(toDoTest);
                }
                else //is date due for ÅKU
                {
                    DateTime passedTestDate = DateTime.Parse(lastTest.TestDate);
                    if (passedTestDate.AddYears(1) < DateTime.Now)
                    {
                        toDoTest = lastTest;
                        toDoTest.TestWaiting = "ÅKU";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                }
            }
            personsWithUndoneTests = personsWithUndoneTests.OrderBy(x => x.LastName).ToList();
            return personsWithUndoneTests;
        }

        private void FillGrid(List<Person> people)
        {
            GridView1.DataSource = people;
            GridView1.DataBind();
        }

        private void PopulateGridFromDropDown()
        {
            if (DropDownList1.Text == "Prov väntar")
            {
                string userName = GetUserName();
                List<Person> team = GetTeamMembers(userName);
                List<Person> personsWithUndoneTests = GetPersonsWithTestToDo(team);
                FillGrid(personsWithUndoneTests);
            }
            else if (DropDownList1.Text == "LST")
            {
                string userName = GetUserName();
                List<Person> result = GetPersonsWithUndoneTestType(userName, "LST");
                FillGrid(result);
            }
            else if (DropDownList1.Text == "ÅKU")
            {
                string userName = GetUserName();
                List<Person> result = GetPersonsWithUndoneTestType(userName, "ÅKU");
                FillGrid(result);
            }
            else if (DropDownList1.Text == "Alla prov")
            {
                string userName = GetUserName();
                List<Person> team = GetTeamMembers(userName);
                team = team.Where(x => x.TestGrade != null).ToList();
                visibleTestWaitingColumn = false;
                FillGrid(team);
            }
        }

        private List<Person> GetPersonsWithUndoneTestType(string userName, string testType)
        {
            List<Person> team = GetTeamMembers(userName);
            List<Person> personsWithUndoneTests = GetPersonsWithTestToDo(team);
            List<Person> persons = personsWithUndoneTests.Where(x => x.TestWaiting == testType).ToList();
            return persons;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
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

        private void SetUpPage()  //session[username]
        {
            string userName = Session["userName"].ToString();
            lblUserName.Text = userName;
            GetTeamMembers(userName);
        }

        private void GetTeamMembers(string userName)
        {
            DatabaseConnection dr = new DatabaseConnection();
            string query = "SELECT firstname, lastname, testtype, date, score, passed, username " +
                            "FROM person p LEFT JOIN testoccasion t ON p.id = t.id_user " +
                            "WHERE id_testadmin = (SELECT id FROM person WHERE username = @userName) ";
            
            List<Person> teamMembers = dr.GetTeamMembers(query, userName);

            List<Person> personsWithUndoneTests = new List<Person>();
            personsWithUndoneTests = GetPersonsWithTestToDo(teamMembers);
            PopulateChart(personsWithUndoneTests, teamMembers);
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
            return personsWithUndoneTests;
        }
       
        private void PopulateChart(List<Person> personsWithUndoneTests, List<Person> allMembers)
        {
            var queryResult = allMembers.GroupBy(x => x.UserName);
            int allTeamMembers = queryResult.Count();
            int toDoTestCount = personsWithUndoneTests.Count;
            int notToDoTest = allTeamMembers - toDoTestCount;

            Chart1.Series["series"].Points.AddXY(toDoTestCount.ToString() + " st", toDoTestCount);
            
            if(notToDoTest != 0)
                Chart1.Series["series"].Points.AddXY(notToDoTest.ToString() +" st", notToDoTest);
            else
                Chart1.Series["series"].Points.AddY(notToDoTest);

            Chart1.Series[0].Points[0].Color = Color.Red;
            Chart1.Series[0].Points[1].Color = Color.LightGreen;

            Chart1.Titles["title"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/licencieringstest.aspx"); 
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/adminStats.aspx");
        }
	}
}
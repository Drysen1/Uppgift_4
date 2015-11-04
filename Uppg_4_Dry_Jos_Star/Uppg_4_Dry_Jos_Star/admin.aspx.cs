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

        private void SetUpPage()  //request.querystring[username]
        {
            string userName = Request.QueryString["userName"];
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

                if(pair.Count(x =>x.TestType == "LST") > 1) //if there are more than 1 LST test we need to check if at least one is passed
                {
                    toDoTest = CheckPersonMultipleTest(pair, toDoTest, "LST");
                    if (toDoTest.FirstName != null)
                    {
                        toDoTest.TestWaiting = "ÅKU";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                    else if (toDoTest.TestGrade != "Godkänd") //person has failed all attempts
                    {
                        var query = pair.OrderByDescending(x => x.TestDate); //I want the failed attempt closest in time
                        toDoTest = query.ElementAt(0);
                        toDoTest.TestWaiting = "LST";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                }
                else if (pair.Count(x => x.TestType == "ÅKU") > 1) //if there are more than 1 ÅKU test we need to check if at least one is passed
                {
                    toDoTest = CheckPersonMultipleTest(pair, toDoTest, "ÅKU");
                    if (toDoTest.FirstName != null)
                    {
                        toDoTest.TestWaiting = "ÅKU";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                    else if(toDoTest.TestGrade != "Godkänd") //person has failed all attempts
                    {
                        var query = pair.OrderByDescending(x => x.TestDate); //I want the failed attempt closest in time
                        toDoTest = query.ElementAt(0);
                        toDoTest.TestWaiting = "ÅKU";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                }
                else if (pair.Count() == 2) // if there are one of each type, LST will always be "Godkänd"
                {
                    foreach (Person p in pair)
                    {
                        if (p.TestType == "ÅKU" && p.TestGrade == "Underkänd")
                        {
                            toDoTest = p;
                            toDoTest.TestWaiting = "ÅKU";
                            personsWithUndoneTests.Add(toDoTest);
                        }
                        else if (p.TestType == "ÅKU" && p.TestGrade == "Godkänd") 
                        {
                            DateTime passedTestDate = DateTime.Parse(p.TestDate);
                            if (passedTestDate.AddYears(1) < DateTime.Now) //due for a new ÅKU?
                            {
                                toDoTest = p;
                                toDoTest.TestWaiting = "ÅKU";
                                personsWithUndoneTests.Add(toDoTest);
                            }
                        }
                    }
                }
                else  //will always be one
                {
                    Person p = pair.ElementAt(0);

                    if (p.TestDate == String.Empty)
                    {
                        toDoTest = p;
                        toDoTest.TestWaiting = "LST";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                    else if (p.TestType == "LST" && p.TestGrade == "Underkänd")
                    {
                        toDoTest = p;
                        toDoTest.TestWaiting = "LST";
                        personsWithUndoneTests.Add(toDoTest);
                    }
                    else if (p.TestType == "LST" && p.TestGrade == "Godkänd") //is date due for ÅKU
                    {
                        DateTime passedTestDate = DateTime.Parse(p.TestDate);
                        if (passedTestDate.AddYears(1) < DateTime.Now)
                        {
                            toDoTest = p;
                            toDoTest.TestWaiting = "ÅKU";
                            personsWithUndoneTests.Add(toDoTest);
                        }
                    }
                }
            }   
            return personsWithUndoneTests;
        }

        private Person CheckPersonMultipleTest(IGrouping<string, Person> pair, Person toDoTest, string testType)
        {
            var result = pair.Where(x => x.TestType == testType && x.TestGrade == "Godkänd").OrderByDescending(x => x.TestDate);

            if (result.Any()) // same as using if (Count!=0), count is however not availible with linq queryresult
            {
                toDoTest.TestGrade = "Godkänd";

                Person p = result.ElementAt(0); //always want the element closest in time
                DateTime testDate = DateTime.Parse(p.TestDate);
                
                if (testDate.AddYears(1) < DateTime.Now) //check if date is due for ÅKU test
                {
                    toDoTest = p;
                }
            }
            return toDoTest;
        }

        private void PopulateChart(List<Person> personsWithUndoneTests, List<Person> allMembers)
        {
            var queryResult = allMembers.GroupBy(x => x.UserName);
            int allTeamMembers = queryResult.Count();
            int toDoTestCount = personsWithUndoneTests.Count;
            int notToDoTest = allTeamMembers - toDoTestCount;

            Chart1.Series["series"].Points.AddXY(toDoTestCount.ToString(), toDoTestCount);
            Chart1.Series["series"].Points.AddY(notToDoTest);
            Chart1.Series[0].Points[0].Color = Color.Red;
            Chart1.Series[0].Points[1].Color = Color.LightGreen;
            Chart1.Series[0].Points[1]["Exploded"] = "true";

            Chart1.Titles["title"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
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
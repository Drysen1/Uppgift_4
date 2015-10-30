//Start1.aspx.cs code behind for page start1.aspx.
//Erik Drysén 2015-10-22.
//Revised 2015-10-29 by Erik Drysén.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls; 
using Npgsql;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class start1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CheckPrivilegeHideNavButton();
                UpdateWebsiteGUI();
            }
        }

        /// <summary>
        /// Method assumes that that a user has been redirected from a login page
        /// calls method GetLoggedinuserinfo to retrieve what privielge user have
        /// to see if admin or not. Hides navbar button to get to adminpanel.
        /// Should maybe put this in a class togheter with GeLoggedInUserInfo
        /// to be able to use it on every page? Good enough for the demo,
        /// but will have to be reworked, preferably with a proper nuget or built in
        /// system to restrict access to webpages.
        /// </summary>
        private void CheckPrivilegeHideNavButton()
        {
            DataTable dt = GetPersonInfo();
            string userType = dt.Rows[0]["privilege"].ToString();

            if (userType != "Admin")
            {
                //Goes through HTML to find buttons to hide.
                HtmlAnchor menuButton = (HtmlAnchor)Page.Master.FindControl("adminButton");
                HtmlAnchor menuButton1 = (HtmlAnchor)Page.Master.FindControl("a1");
                //Hides buttons.
                menuButton.Visible = false;
                menuButton1.Visible = false;
            }
        }

        /// <summary>
        /// Method assumes that a user has been redirected from a loginpage. 
        /// Retrieves all information in person table and test occasion table
        /// and stores in a datatable.
        /// </summary>
        /// <returns>Returns a datatable with all information regarding user</returns>
        private DataTable GetPersonAndTestInfo()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Database=kompetensportal;Server=localhost;User Id=postgres;Password=anna;");
            string userName = lblUserName.Text;
            try
            {
                conn.Open();
                NpgsqlCommand cmdGetUserInfo = new NpgsqlCommand("SELECT * FROM person " +
                                                                 "INNER JOIN testoccasion ON person.id = id_user " +
                                                                 "WHERE person.id = (SELECT id FROM person WHERE username = @userName) " + 
                                                                 "ORDER BY date DESC LIMIT 1; ", conn);
                cmdGetUserInfo.Parameters.AddWithValue("@userName", userName);
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter();
                nda.SelectCommand = cmdGetUserInfo;
                DataSet ds = new DataSet();
                nda.Fill(ds);
                DataTable dt = ds.Tables[0];
                
                return dt;
            }
            catch (NpgsqlException ex)
            {
                Response.Write(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        private DataTable GetPersonInfo()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Database=kompetensportal;Server=localhost;User Id=postgres;Password=anna;");
            int userId = 2;//Change id here to get the user you want to find.
            //Assume id or something has been passed from log in page in order to retrieve correct info.
            //This is only for simulations purpose for this iteration.
            try
            {
                conn.Open();
                NpgsqlCommand cmdGetUserInfo = new NpgsqlCommand("SELECT * FROM person " +
                                                                 "WHERE person.id = @id; ", conn);
                cmdGetUserInfo.Parameters.AddWithValue("@id", userId);
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter();
                nda.SelectCommand = cmdGetUserInfo;
                DataSet ds = new DataSet();
                nda.Fill(ds);
                DataTable dt = ds.Tables[0];             

                return dt;
            }
            catch (NpgsqlException ex)
            {
                Response.Write(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Method updates labels on start1.aspx with relevant values.
        /// Uses method GetLoggedInUserInfo to retrieve said information,
        /// </summary>
        private void UpdateWebsiteGUI()
        {
            DataTable dt = new DataTable();
            DataTable dtAllInfo = new DataTable();

            dt = GetPersonInfo();
            string userName = dt.Rows[0]["username"].ToString();
            lblUserName.Text = userName;

            dtAllInfo = GetPersonAndTestInfo();

            //lblNextTestDate.Text = dtAllInfo.Rows.Count.ToString();
            //lblresult.Text = dt.Rows.Count.ToString();
            if (dtAllInfo.Rows.Count <= 0)
            {
                lblresult.Text = "Inget test hittat";
                lbldate.Text = "Inget datum";
                lbltestToDo.Text = "1";
                DateTime today = DateTime.Now;
                lblNextTestDate.Text = today.ToString("yyyy-MM-dd");
                btnGoToOldTest.Enabled = false;
            }
            else
            {
                string passTest = dtAllInfo.Rows[0]["passed"].ToString();
                DateTime date = DateTime.Parse(dtAllInfo.Rows[0]["date"].ToString());
                lblresult.Text = SetPassOrFail(passTest);
                lbltestToDo.Text = CheckDateOfLastTest(date, passTest);
                lbldate.Text = date.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Method recevis a string, which was a bool in the database
        /// and sets a new string value to make it readable for the user
        /// so he or she can understand. New value depends on if the value 
        /// from db is true or false.
        /// </summary>
        /// <param name="passTest">A value from the database which is True or False</param>
        /// <returns>Returns string value to make sense for the user.</returns>
        private string SetPassOrFail(string passTest)
        {
            if (passTest == "False")
            {
                passTest = "Inte godkänd";
            }
            else
            {
                passTest = "Godkänd";
            }
            return passTest;
        }

        /// <summary>
        /// This method should probably be broken up into smaller methods.
        /// Method takes param date and param passTest.
        /// Evaluates if passtest is false, if false evaluates timespan is more or less
        /// than 7 days to see if user is allowed to retake the test.
        /// If passTest is true, evaluates timespan between date of test and date of
        /// today. If more than 1year ago user can retake test.
        /// </summary>
        /// <param name="date">DateTime value from database</param>
        /// <param name="passTest">bool/string value from database</param>
        /// <returns>Returns new string value</returns>
        private string CheckDateOfLastTest(DateTime date, string passTest)
        {
            string toDoTest = string.Empty;
            DateTime today = DateTime.Now;
            TimeSpan test = today - date;
            int totalDays = test.Days;
            int year = 365;
            int week = 7;
            DateTime nextTestDate;
            if(passTest == "False")
            {
                if(totalDays > week)
                {
                    nextTestDate = date.AddDays(7);
                    lblNextTestDate.Text = nextTestDate.ToString("yyyy-MM-dd");
                    toDoTest = "1";
                    btnStartTest.Enabled = true;
                    btnGoToOldTest.Enabled = false;
                    return toDoTest;
                }
                else
                {
                    nextTestDate = date.AddDays(7);
                    lblNextTestDate.Text = nextTestDate.ToString("yyyy-MM-dd");
                    toDoTest = "1";
                    btnStartTest.Enabled = false;
                    btnGoToOldTest.Enabled = false;
                    return toDoTest;
                }
            }
            else
            {
                if (totalDays > year)
                {
                    nextTestDate = date.AddYears(1);
                    lblNextTestDate.Text = nextTestDate.ToString("yyyy-MM-dd");
                    toDoTest = "1";
                    return toDoTest;
                }
                else
                {
                    nextTestDate = date.AddYears(1);
                    lblNextTestDate.Text = nextTestDate.ToString("yyyy-MM-dd");
                    toDoTest = "0";
                    btnStartTest.Enabled = false;
                    return toDoTest;
                }
            }
        }

        /// <summary>
        /// Method evaluates what type of test user should do, either ÅKU or LST.
        /// </summary>
        /// <returns>Returns new string value.</returns>
        private string SetTypeOfTest()
        {
            DataTable dt = GetPersonAndTestInfo();
            string typeOfTest = string.Empty;
            string passTest = string.Empty;

            if (dt.Rows.Count <= 0)
            {
                typeOfTest = "LST";
                return typeOfTest;
            }
            else
            {
                typeOfTest = dt.Rows[0]["testtype"].ToString();
                passTest = dt.Rows[0]["passed"].ToString();

                if (typeOfTest == "LST" && passTest == "False")
                {
                    typeOfTest = "LST";
                    return typeOfTest;
                }
                else
                {
                    typeOfTest = "ÅKU";
                    return typeOfTest;
                }
            }
        }

        /// <summary>
        /// btnStartTest event click, redirects user to testpage so he or she can
        /// start the test. Sends value to testPage with a querystring.
        /// </summary>
        protected void Button1_Click(object sender, EventArgs e)
        {

            string userName = lblUserName.Text;
            string typeOfTest = SetTypeOfTest();

            Response.Redirect("~/testPage.aspx?userName=" + userName + "&typeOfTest=" + typeOfTest); //Currently sending data to test.aspx for test purpose. Change this to testPage to receieve data in testpage.
        }

        /// <summary>
        /// btnGoToOldTest event click.
        /// </summary>
        protected void Button2_Click(object sender, EventArgs e)
        {
            string userName = lblUserName.Text;

            Response.Redirect("~/UserOldTest.aspx?userName=" + userName);
        }

        // TEST PURPOSES CODE BELOW ***************************************************************************************************
        private DataTable CreateFakeDB()
        {
            //ASSUME that there is a query to database which retrieves one row and stores in datatable.
            DataTable dt = new DataTable();
            dt.Columns.Add("employee_id", typeof(int));
            dt.Columns.Add("userName", typeof(string));
            dt.Columns.Add("latest_date", typeof(DateTime));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("secLevel", typeof(string));
            //SHould be a column with the test itself saved as an xml I think?

            DateTime myDate = new DateTime(2014, 10, 05);
            dt.Rows.Add(1, "FakeUserNo1", DateTime.Now.Date, "PASS", "Admin"); //Fake user no1
            dt.Rows.Add(2, "FakeUserNo2", myDate, "FAIL", "NotAdmin");         //Fake user no2

            return dt;
        }

        private void GetDbInfo()
        {
            DataTable dt = CreateFakeDB();

            string passTest = dt.Rows[0]["status"].ToString();
            DateTime date = DateTime.Parse(dt.Rows[0]["latest_date"].ToString());

            DateTime today = DateTime.Now;
            TimeSpan test = today - date;
            int totalDays = test.Days;
            int year = 365;
            string toDoTest = string.Empty;

            if (totalDays > year)
            {
                toDoTest = "1";
            }
            else
            {
                toDoTest = "0";
                //Disable button
            }

            lbltestToDo.Text = toDoTest.ToString();
            lblresult.Text = passTest.ToString();
            lbldate.Text = date.ToString("yyyy-MM-dd");
            lblUserName.Text = dt.Rows[0]["userName"].ToString();

        }


    }
}
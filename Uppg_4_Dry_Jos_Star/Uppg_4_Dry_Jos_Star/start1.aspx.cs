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
    public partial class start1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = CreateFakeDB();

            string userType = dt.Rows[1]["status"].ToString(); //Simulates that site retrieves userseclevel.
            if (userType != "Admin")
            {
                HtmlAnchor menuButton = (HtmlAnchor)Page.Master.FindControl("adminButton");
                HtmlAnchor menuButton1 = (HtmlAnchor)Page.Master.FindControl("a1");
                menuButton.Visible = false;
                menuButton1.Visible = false;
            }
            else
            {
                lblUserName.Text = userType;
            }

            GetDbInfo();
        }

        private DataTable CreateFakeDB()
        {
            //ASSUME that there is a query to database which retrieves one row and stores in datatable.
            DataTable dt = new DataTable();
            dt.Columns.Add("employee_id", typeof(int));
            dt.Columns.Add("latest_date", typeof(DateTime));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("secLevel", typeof(string));
            //SHould be a column with the test itself saved as an xml I think?

            DateTime myDate = new DateTime(2014, 10, 05);
            dt.Rows.Add(1, DateTime.Now.Date, "PASS", "Admin"); //Fake user no1
            dt.Rows.Add(2, myDate, "FAIL", "NotAdmin");         //Fake user no2

            return dt;
        }

        private void GetDbInfo()
        {
            DataTable dt = CreateFakeDB();

            string passTest = dt.Rows[1]["status"].ToString();
            DateTime date = DateTime.Parse(dt.Rows[1]["latest_date"].ToString());

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

            testToDo.Text = toDoTest.ToString();
            result.Text = passTest.ToString();
            lbldate.Text = date.ToString("yyyy-MM-dd");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("test.aspx"); //Test prupose to make sure redirect works properly. 
                                            //SHould propably send user info to next page so test can load properly.
                                            //
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
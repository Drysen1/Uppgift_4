using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class start : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            GetDbInfo();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("test.aspx");
        }

        private void Start()
        {
            //GetDbInfo();

        }

        private void GetDbInfo()
        {
            //ASSUME that there is a query to database which retrieves one row and stores in datatable.
            DataTable dt = new DataTable();
            dt.Columns.Add("employee_id", typeof(int));
            dt.Columns.Add("latest_date", typeof(DateTime));
            dt.Columns.Add("status", typeof(string));
            //SHould be a column with the test itself saved as an xml.

            DateTime myDate = new DateTime(2014, 10, 05);
            dt.Rows.Add(1, DateTime.Now.Date, "PASS");
            dt.Rows.Add(2, myDate, "FAIL");


            string passTest = dt.Rows[1]["status"].ToString();
            DateTime date = DateTime.Parse(dt.Rows[1]["latest_date"].ToString());

            DateTime today = DateTime.Now;
            TimeSpan test = today - date;
            int totalDays = test.Days;
            int year = 365;
            string toDoTest = string.Empty;

            if(totalDays > year)
            {
                toDoTest = "1";
            }
            else
            {
                toDoTest = "0";
                //Disalbe button
            }

            testToDo.Text = toDoTest.ToString();
            result.Text = passTest.ToString();
            lbldate.Text = date.ToString("yyyy-MM-dd");

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Xml.Linq;
using Npgsql;

namespace Uppg_4_Dry_Jos_Star
{
    public class DatabaseConnection
    {
        private string myConnection = "Server=localhost;Port=5432;Database=kompetensportal;User Id=postgres;Password=anna;";
                                        
        public string NpgsqlException { get; set; }

        public string GetUserId(string userName)
        {
            string id = "";
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "SELECT id FROM person WHERE username = @userName ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("userName", userName);

                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            while(dr.Read())
                            {
                                id = dr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
            return id;
        }

        public void SaveUserXml (string userId, XDocument userXml, DateTime todayDate)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
            {
                userXml.Save(sw);
            }
            try
            {
                using(NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "INSERT INTO testoccasion (date, id_user, xmlstring) VALUES(@date, @userId, @xml)";

                    using(NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("date", todayDate);
                        cmd.Parameters.AddWithValue("userId", int.Parse(userId));
                        cmd.Parameters.AddWithValue("xml", sb.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
        }

        public List<string> RetrieveXmlDocument(string userId, DateTime todayDate)
        {
            List<string> xmlList = new List<string>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "SELECT xmlstring FROM testoccasion "+
                                    "WHERE id_user = @userId AND date = @date ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", int.Parse(userId));
                        cmd.Parameters.AddWithValue("date", todayDate);

                        using(NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            while(dr.Read())
                            {
                                xmlList.Add(dr[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
            return xmlList;
        }

        /// <summary>
        /// Method get latest test that user have done.
        /// </summary>
        /// <returns>returns list with xml.</returns>
        public List<string> GetUserLatestTest(string userId)
        {
            List<string> xmlList = new List<string>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "SELECT xmlstring FROM testoccasion " +
                                    "WHERE id_user = @userId ORDER BY date DESC LIMIT 1";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", int.Parse(userId));

                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                xmlList.Add(dr[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
            return xmlList;
        }

        public List<Person> RetrieveAllXmlDocuments(string testType, string userName)
        {
            List<Person> personWithXmlTest = new List<Person>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "SELECT firstname, lastname, username, date, score, xmlstring " +
                                    "FROM testoccasion t JOIN person p ON t.id_user = p.id "+
                                    "WHERE testtype = @testType AND id_testadmin = "+
                                    "(SELECT id FROM person WHERE username = @userName) " +
                                    "ORDER BY date DESC ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("testType", testType);
                        cmd.Parameters.AddWithValue("userName", userName);

                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                personWithXmlTest.Add(
                                new Person
                                {
                                    FirstName = dr[0].ToString(),
                                    LastName = dr[1].ToString(),
                                    UserName = dr[2].ToString(),
                                    TestDate = dr[3].ToString().Substring(0,10),
                                    TestScore = dr[4].ToString(),
                                    xmlTest = XDocument.Parse(dr[5].ToString())
                                });
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
            return personWithXmlTest;
        }

        public void UpdateAfterTestIsComplete(string userId, DateTime todayDate, string score, bool passed, string typeOfTest)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "UPDATE testoccasion SET score = @score, passed = @passed, testtype = @typeOfTest "+
                                    "WHERE id_user = @userName AND date = @date ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("score", score);
                        cmd.Parameters.AddWithValue("passed", passed);
                        cmd.Parameters.AddWithValue("userName", int.Parse(userId));
                        cmd.Parameters.AddWithValue("date", todayDate);
                        cmd.Parameters.AddWithValue("typeOfTest", typeOfTest);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
        }
        
        public List<Person> GetNoTestPersons()
        {
            //deffinerar koppling mot postgres
            List<Person> listOfPersons = new List<Person>();
            bool isPassed;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    //öpnar koppling mot db
                    conn.Open();

                    //kod för vad man vill göra mot databasen
                    string query = "SELECT firstname, lastname, testtype, date, score, passed, username " +
                                    "FROM testoccasion t " +
                                    "RIGHT JOIN person p ON t.id_user = p.id " +
                                    "WHERE id_user IS NULL OR (testtype ='LST' AND passed = false OR testtype ='ÅKU' AND passed = false) " +
                                    "ORDER BY lastname";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                    {
                        //command.Parameters.AddWithValue("name", userName); 
                        using (NpgsqlDataReader dr = command.ExecuteReader())
                        {

                            while (dr.Read())
                            {

                                Person p = new Person();
                                p.FirstName = dr[0].ToString();
                                p.LastName = dr[1].ToString();

                                p.TestType =dr[2].ToString();

                                p.TestDate = dr[3].ToString();
                                if (p.TestDate != "")
                                {
                                    p.TestDate = p.TestDate.Substring(0, 10);
                                }

                                p.TestScore = dr[4].ToString();

                                if(bool.TryParse(dr[5].ToString(), out isPassed ))
                                {
                                    if (isPassed)
                                        p.TestGrade = "Godkänd";
                                    else
                                        p.TestGrade = "Underkänd";
                                }

                                p.UserName = dr[6].ToString();

                                listOfPersons.Add(p);
                            }
                        }
                    }
                }
            }

            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;

            }
            //returnerar listan med personer som inte skrivit LST och de som inte blivit godkännda 
            return listOfPersons;
        } //Personer som inte gjort ett LST/ÅKU eller ej blivit godkända. 

    }
}
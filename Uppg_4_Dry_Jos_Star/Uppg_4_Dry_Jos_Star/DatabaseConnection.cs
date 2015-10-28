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
        private string myConnection = "Server=localhost;Port=5432;Database=kompetensportalen;User Id=postgres;Password=anna;";
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

        public void UpdateAfterTestIsComplete(string userId, DateTime todayDate, string score, bool passed)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "UPDATE testoccasion SET score = @score, passed = @passed "+
                                    "WHERE id_user = @userName AND date = @date ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("score", score);
                        cmd.Parameters.AddWithValue("passed", passed);
                        cmd.Parameters.AddWithValue("userName", int.Parse(userId));
                        cmd.Parameters.AddWithValue("date", todayDate);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
        }
    }
}
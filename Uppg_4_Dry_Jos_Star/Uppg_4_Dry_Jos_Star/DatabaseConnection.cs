﻿using System;
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

        public void SaveUserXml (XDocument userXml)
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
                    string query = "INSERT INTO testoccasion (xmlstring) VALUES(@xml) ";

                    using(NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
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

        public XDocument RetrieveXmlDocument(int userId)
        {
            XDocument xDoc = new XDocument();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnection))
                {
                    conn.Open();
                    string query = "SELECT xmlstring FROM testoccasion WHERE id = @userId";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);

                        using(NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            while(dr.Read())
                            {
                                string result = dr[0].ToString();
                                xDoc = XDocument.Parse(result);
                            }
                            XDeclaration xDec = xDoc.Declaration; //for some reason postgres changes encoding to utf-16, changing it back!
                            xDec.Encoding = "utf-8";
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                NpgsqlException = ex.Message;
            }
            return xDoc;
        }
    }
}
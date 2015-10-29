using System;
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
        string myConnectionString = "Server=localhost;Port=5432;Database=kompetensportal;User Id=postgres;Password=anna;"; 

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Person> personer = GetNoTestPersons(); 

        }

        private List<Person> GetNoTestPersons()
        {
            //deffinerar koppling mot postgres
             List<Person> listOfPersons = new List<Person>(); 

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(myConnectionString))
                {
                    //öpnar koppling mot db
                    conn.Open();

                    //kod för vad man vill göra mot databasen
                    string query = "SELECT firstname, lastname " +
                                    "FROM testoccasion t " +
                                    "RIGHT JOIN person p ON t.id_user = p.id " +
                                    "WHERE id_user IS NULL";

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

                                listOfPersons.Add(p);
                            }

                        }
                    }
                }
            }
        
            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
            //returnerar listan med personer som inte skrivit LST
            return listOfPersons;
          
        }

        
    }
}
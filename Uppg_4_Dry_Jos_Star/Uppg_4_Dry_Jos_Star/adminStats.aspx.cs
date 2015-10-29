using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class adminStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                List<Person> personsWithTest = GetAlltests();
                Dictionary<string, List<Question>> dictPersonsWithQuestions = CreateQuestions(personsWithTest);
            }
        }

        private List<Person> GetAlltests()
        {
            DatabaseConnection dr = new DatabaseConnection();
            return dr.RetrieveAllXmlDocuments("LST");
        }

        private Dictionary<string, List<Question>> CreateQuestions(List<Person> persons)
        {
            Dictionary<string, List<Question>> dictAllTests = new Dictionary<string, List<Question>>();
            foreach (Person p in persons)
            {
                var xmlResult = 
                        from q in p.xmlTest.Descendants("question")
                        select new Question
                        {
                            Id = q.Attribute("id").Value,
                            Text = q.Element("text").Value,
                            Category = q.Parent.Attribute("type").Value,
                            Answers = q.Elements("answer").Select(x => x.Value).ToList(),
                            CorrectAnswer = q.Elements("answer").Where(x => x.Attribute("correct").Value == "yes")
                                                                .Select(x => x.Value).ToList(),
                            NumOfCorrect = String.Format("Antal korrekta svar: ({0})", q.Elements("answer").Where(x => x.Attribute("correct").Value == "yes")
                                                                .Select(x => x.Value).ToList().Count.ToString()),
                            UserInput = q.Elements("answer").Where(x => x.Attribute("input").Value == "yes")
                                                            .Select(x=> x.Value).ToList(),
                        };

                List<Question> questions = new List<Question>();
                foreach (Question q in xmlResult)
                {
                    questions.Add(q);
                }
                dictAllTests.Add(p.UserName, questions);
            }
            return dictAllTests;
        }
    }
}
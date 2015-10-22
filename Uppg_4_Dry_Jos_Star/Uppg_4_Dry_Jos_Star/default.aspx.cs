using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                List<Question> questions = GetXmlContent();
                Repeater1.DataSource = questions;
                Repeater1.DataBind();
            }
        }

        private List<Question> GetXmlContent()
        {
            
            XDocument xDoc = XDocument.Load(Server.MapPath("~/xml/questions.xml"));
            var xmlResult = from q in xDoc.Descendants("question")
                            select new Question
                            {
                                QuestionNumber = q.Attribute("id").Value,
                                Text = q.Element("text").Value,
                                Category = q.Parent.Attribute("type").Value,
                                Answers = q.Elements("answer").Select(x => x.Value).ToList(),
                                CorrectAnswer = q.Elements("answer").Where(x => x.Attribute("correct").Value == "yes").Select(x => x.Value).ToList(),
                                NumOfCorrect = q.Elements("answer").Where(x => x.Attribute("correct").Value == "yes").Select(x => x.Value).ToList().Count.ToString()
                            };

            List<Question> questions = new List<Question>();
            foreach (Question q in xmlResult)
            {
                questions.Add(q);
            }

            return questions;
        }
        
    }
}
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
        private readonly object syncLock = new object();
        private readonly Random rand = new Random();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Question> questions = GetXmlContent();
                List<List<Question>> categoryLists = GetCategoryLists(questions); //parameter takes List<Question> .Count that corresponds to which test; 25 or 15 items at this moment
                CreateUserXml(categoryLists);
                if (categoryLists.Count > 0)
                {
                    Repeater1.DataSource = categoryLists[0];
                    Repeater1.DataBind();
                }
                if(categoryLists.Count > 1)
                {
                    Repeater2.DataSource = categoryLists[1];
                    Repeater2.DataBind();
                }
                if (categoryLists.Count > 2)
                {
                    Repeater3.DataSource = categoryLists[2];
                    Repeater3.DataBind();
                }
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SetLabelInRepeaterHead(e, Repeater1);
        }
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SetLabelInRepeaterHead(e, Repeater2);
        }
        protected void Repeater3_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SetLabelInRepeaterHead(e, Repeater3);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in Repeater1.Items)
            {
                Label lbl = (Label)item.FindControl("question");
                CheckBox chBox1 = (CheckBox)item.FindControl("cBox1");
                CheckBox chBox2 = (CheckBox)item.FindControl("cBox2");
                CheckBox chBox3 = (CheckBox)item.FindControl("cBox3");
                CheckBox[] cBoxes = { chBox1, chBox2, chBox3 };

                AddXmlAttribute(lbl.Text, cBoxes);
            }
            foreach (RepeaterItem item in Repeater2.Items)
            {
                Label lbl = (Label)item.FindControl("question");
                CheckBox chBox1 = (CheckBox)item.FindControl("cBox1");
                CheckBox chBox2 = (CheckBox)item.FindControl("cBox2");
                CheckBox chBox3 = (CheckBox)item.FindControl("cBox3");
                CheckBox[] cBoxes = { chBox1, chBox2, chBox3 };

                AddXmlAttribute(lbl.Text, cBoxes);
            }
            foreach (RepeaterItem item in Repeater3.Items)
            {
                Label lbl = (Label)item.FindControl("question");
                CheckBox chBox1 = (CheckBox)item.FindControl("cBox1");
                CheckBox chBox2 = (CheckBox)item.FindControl("cBox2");
                CheckBox chBox3 = (CheckBox)item.FindControl("cBox3");
                CheckBox[] cBoxes = { chBox1, chBox2, chBox3 };

                AddXmlAttribute(lbl.Text, cBoxes);
            }
        }
        //-------------------------------------------------------------------------------------
        private List<Question> GetXmlContent()
        {
            XDocument xDoc = XDocument.Load(Server.MapPath("~/xml/questions.xml"));
            var xmlResult = from q in xDoc.Descendants("question")
                            select new Question
                            {
                                Id = q.Attribute("id").Value,
                                Text = q.Element("text").Value,
                                Category = q.Parent.Attribute("type").Value,
                                Answers = q.Elements("answer").Select(x => x.Value).ToList(),
                                CorrectAnswer = q.Elements("answer").Where(x => x.Attribute("correct").Value == "yes")
                                                                    .Select(x => x.Value).ToList(),
                                NumOfCorrect = String.Format("Antal korrekta svar: ({0})", q.Elements("answer").Where(x => x.Attribute("correct").Value == "yes")
                                                                    .Select(x => x.Value).ToList().Count.ToString())
                            };

            List<Question> questions = new List<Question>();
            foreach (Question q in xmlResult)
            {
                questions.Add(q);
            }

            List<int> questionOrder = GetRandomOrder(questions.Count);

            return questions;
        }

        private List<List<Question>> GetCategoryLists(List<Question> questionList)
        {
            List<List<Question>> allCategoryQuestions = new List<List<Question>>();
            int count = 0;

            List<int> numsToReOrderWith = GetRandomOrder(questionList.Count); //create random numbers according to amount of questions
            questionList = GetRandomizedList(questionList, numsToReOrderWith);
            List<string> categories = GetCategoryNames(questionList);

            foreach (string c in categories)
            {
                var result = from q in questionList
                             where q.Category == c
                             select q;

                List<Question> categoryQuestions = new List<Question>();
                
                foreach(Question quest in result)
                {
                    count++;
                    quest.AnswerOrder = count.ToString() + ". ";
                    categoryQuestions.Add(quest);
                }
                allCategoryQuestions.Add(categoryQuestions);
            }
            return allCategoryQuestions;
        }

        private List<Question> GetRandomizedList(List<Question> questions, List<int> numsToReOrderWith)
        {
            List<Question> randomizedList = new List<Question>();
            foreach(int number in numsToReOrderWith)
            {
                randomizedList.Add(questions.Where(x=>x.Id == number.ToString()).FirstOrDefault());
            }
            randomizedList = ShuffleAnswerOrder(randomizedList);
            return randomizedList;
        }

        private List<Question> ShuffleAnswerOrder(List<Question> questions)
        {
            if(questions.Count > 0 && questions[0].Answers != null)
            {
                foreach(Question q in questions)
                {
                    List<int> numbers = GetRandomOrder(q.Answers.Count);
                    List<string> newAnswerOrder = new List<string>();
                    for(int i = 0; i < numbers.Count; i++)
                    {
                        int index = numbers[i] - 1;
                        string answer = q.Answers[index];
                        newAnswerOrder.Add(answer);
                    }
                    q.Answers = newAnswerOrder;
                }
            }
            return questions;
        }

        private List<string> GetCategoryNames(List<Question> questionList)
        {
            List<string> categories = new List<string>();
            var grouped = questionList.GroupBy(x => x.Category);
            foreach (var pair in grouped)
            {
                categories.Add(pair.Key);
            }
            return categories;
        }

        private List<int> GetRandomOrder(int amountOfNums)
        {
            lock (syncLock)
            {
                List<int> numberList = new List<int>();

                while (numberList.Count < amountOfNums)
                {
                    int number = rand.Next(1, amountOfNums + 1); //gives one less than specified
                    if (!numberList.Contains(number))
                    {
                        numberList.Add(number);
                    }
                }
                return numberList;
            }
        }

        private void CreateUserXml(List<List<Question>> categoryLists)
        {
            List<Question> randomizedList = new List<Question>(); //take out all lists under categories and make one long list to get categories below
            foreach (List<Question> list in categoryLists)
            {
                foreach (Question q in list)
                {
                    randomizedList.Add(q);
                }
            }
            List<string> categories = GetCategoryNames(randomizedList);

            XDocument xDoc = new XDocument(
                   new XDeclaration("1.0", "utf-8", "yes"),
                   new XElement("test",
                       from c in categories //loops categories and creates rest accordingly
                       select new XElement("category",
                            new XAttribute("type", c))));

            foreach (List<Question> list in categoryLists) //add elements to xml from each list one by one
            {
                string category = list.ElementAt(0).Category;

                var categoryElement = from el in xDoc.Root.Elements("category")
                                      where (string)el.Attribute("type") == category
                                      select el;

                xDoc.Element("test").Elements("category")
                    .Where(x => x.Attribute("type").Value == category).FirstOrDefault()
                    .Add(
                    from q in list
                    select new XElement("question",
                    new XAttribute("id", q.Id),
                    new XElement("text", q.Text),

                    from a in q.Answers
                    select new XElement("answer", a)));
                
                foreach(Question q in list) //to set correct="yes"/"no" as attributes
                {
                    foreach(string answer in q.CorrectAnswer)
                    {
                        var element = from e in xDoc.Descendants("question")
                                      where e.Element("text").Value == q.Text
                                      select e.Elements("answer");
                        foreach(var a in element)
                        {
                            foreach(XElement el in a)
                            {
                                if (el.Value == answer)
                                {
                                    el.Add(new XAttribute("correct", "yes"));
                                }
                            }
                        }
                    }
                }
                var answers = from e in xDoc.Descendants("answer")
                              where e.Attribute("correct") == null
                              select e;
                foreach (XElement x in answers)
                {
                    x.Add(new XAttribute("correct", "no"));
                }
            }
            xDoc.Save(Server.MapPath("~/xml/userXml.xml"));
        }

        private void SetLabelInRepeaterHead(RepeaterItemEventArgs e, Repeater rep)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Label lbl = (Label)e.Item.FindControl("categoryText");
                if (Repeater1.DataSource != null)
                {
                    List<Question> ds = (List<Question>)rep.DataSource;
                    lbl.Text = ds[0].Category;
                }
            }
        }

        private void AddXmlAttribute(string questionText, CheckBox[] userInput)
        {
            XDocument xDoc = XDocument.Load(Server.MapPath("~/xml/userXml.xml"));
            var answers = from a in xDoc.Descendants("question")
                          where a.Element("text").Value == questionText
                          select a.Elements("answer");
            
            if(answers.Any()) //check for zero result
            {
                IEnumerable<XElement> searchResult = answers.ElementAt(0);

                XElement e1 = searchResult.ElementAt(0);
                string result = MatchElementWithUserInput(e1, userInput);
                WriteToXml(e1, result, xDoc);

                XElement e2 = searchResult.ElementAt(1);
                string result2 = MatchElementWithUserInput(e2, userInput);
                WriteToXml(e2, result2, xDoc);

                XElement e3 = searchResult.ElementAt(2);
                string result3 = MatchElementWithUserInput(e3, userInput);
                WriteToXml(e3, result3, xDoc);
            }
        }

        private string MatchElementWithUserInput(XElement element, CheckBox[] userInput)
        {
            string toReturn = "";
            CheckBox cBox1 = userInput[0];
            CheckBox cBox2 = userInput[1];
            CheckBox cBox3 = userInput[2];

            if (element.Value == cBox1.Text)
            {
                toReturn = IsCheckBoxChecked(cBox1) ? "yes" : "no";
            }
            else if (element.Value == cBox2.Text)
            {
                toReturn = IsCheckBoxChecked(cBox2) ? "yes" : "no";
            }
            else if(element.Value == cBox3.Text)
            {
                toReturn = IsCheckBoxChecked(cBox3) ? "yes" : "no";
            }
            return toReturn;
        }

        private bool IsCheckBoxChecked(CheckBox cBox)
        {
            return cBox.Checked;
        }

        private bool IsAnswerCorrect(XElement element, CheckBox cBox)
        {
            string correct = (string)element.Attribute("correct");
            if(cBox.Checked && correct == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void WriteToXml(XElement element, string inputResult, XDocument xDoc)
        {
            element.SetAttributeValue("input", inputResult);
            xDoc.Save(Server.MapPath("~/xml/userXml.xml"));
        }

    }
}
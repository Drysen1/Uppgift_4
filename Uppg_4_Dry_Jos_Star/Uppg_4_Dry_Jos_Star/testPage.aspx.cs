using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class _default : System.Web.UI.Page
    {
        private readonly object syncLock = new object();
        private readonly Random rand = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["IsPageReloadAllowed"] == null) //gets instantiated first time
            {
                List<Question> questions = GetXmlContent("~/xml/questions.xml");
                List<List<Question>> categoryLists = GetCategoryLists(questions);
                CreateUserXml(categoryLists);
                PopulateRepeaters(categoryLists);
                finalResult.Visible = false;
                Session["IsPageReloadAllowed"] = false;
            }
            else if (Session["IsPageReloadAllowed"] != null && Session["IsFirstTime"] == null) //page reload before button has been clicked
            {
                if(!IsAnythingChecked()) //when button is clicked something will be checked
                {
                    string fileName = GetUserXmlFileName();
                    List<Question> questions = GetXmlContent(fileName);
                    List<List<Question>> categoryLists = GetCategoryListsNoRandomize(questions);

                    PopulateRepeaters(categoryLists);
                    finalResult.Visible = false;
                }
            }
            else if (Session["IsPageReloadAllowed"] != null && Session["IsFirstTime"] != null) //if user hits back in browser after button has been clicked
            {
                finalResult.Visible = false;
                btnSend.Enabled = false;
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
            if(Session["IsFirstTime"] != null) //after buttonclick page reload will trigger onClick-event
            {
                CorrectTest();
            }
            else
            {
                string fileName = GetUserXmlFileName();
                XDocument xDoc = XDocument.Load(Server.MapPath(fileName));

                List<Repeater> reps = new List<Repeater>();
                foreach (Repeater rep in repeaters.Controls.OfType<Repeater>())
                {
                    reps.Add(rep);
                }

                foreach (Repeater rep in reps)
                {
                    foreach (RepeaterItem item in rep.Items)
                    {
                        Label lbl = (Label)item.FindControl("question");
                        CheckBox chBox1 = (CheckBox)item.FindControl("cBox1");
                        CheckBox chBox2 = (CheckBox)item.FindControl("cBox2");
                        CheckBox chBox3 = (CheckBox)item.FindControl("cBox3");
                        CheckBox[] cBoxes = { chBox1, chBox2, chBox3 };
                        AddXmlAttribute(lbl.Text, cBoxes, xDoc);
                    }
                }
                CorrectTest();
            }
        }
        //-------------------------------------------------------------------------------------
        
        private List<Question> GetXmlContent(string xmlVirtualPath) 
        {
            XDocument xDoc = XDocument.Load(Server.MapPath(xmlVirtualPath));
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
                                                                    .Select(x => x.Value).ToList().Count.ToString()),
                            };

            List<Question> questions = new List<Question>();
            foreach (Question q in xmlResult)
            {
                questions.Add(q);
            }
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

                foreach (Question quest in result)
                {
                    count++;
                    quest.AnswerOrder = count.ToString() + ". ";
                    categoryQuestions.Add(quest);
                }
                allCategoryQuestions.Add(categoryQuestions);
            }
            return allCategoryQuestions;
        }

        private List<List<Question>> GetCategoryListsNoRandomize(List<Question> questionList)
        {
            List<List<Question>> allCategoryQuestions = new List<List<Question>>();
            int count = 0;
            List<string> categories = GetCategoryNames(questionList);

            foreach (string c in categories)
            {
                var result = from q in questionList
                             where q.Category == c
                             select q;

                List<Question> categoryQuestions = new List<Question>();

                foreach (Question quest in result)
                {
                    count++;
                    quest.AnswerOrder = count.ToString() + ". ";
                    categoryQuestions.Add(quest);
                }
                allCategoryQuestions.Add(categoryQuestions);
            }
            return allCategoryQuestions;
        }

        private void PopulateRepeaters(List<List<Question>> categoryLists)
        {
            List<Repeater> reps = new List<Repeater>();
            foreach (Repeater rep in repeaters.Controls.OfType<Repeater>())
            {
                reps.Add(rep);
            }
            for (int i = 0; i < categoryLists.Count; i++)
            {
                reps[i].DataSource = categoryLists[i];
                reps[i].DataBind();
            }
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

        private void CreateUserXml(List<List<Question>> categoryLists) //userXml gets sent to DB here
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
                var answers = from e in xDoc.Descendants("answer") //is needed otherwise attribute will be duplicated by else above
                              where e.Attribute("correct") == null
                              select e;
                foreach (XElement x in answers)
                {
                    x.Add(new XAttribute("correct", "no"));
                }
            }
            string fileName = GetUserXmlFileName();
            xDoc.Save(Server.MapPath(fileName));
            //SendUserXmlToDb();
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

        private void AddXmlAttribute(string questionText, CheckBox[] userInput, XDocument xDoc)
        {
            var answers = from a in xDoc.Descendants("question")
                          where a.Element("text").Value == questionText
                          select a.Elements("answer");
            
            if(answers.Any()) //check for zero result
            {
                List<XElement> searchResult = answers.ElementAt(0).ToList();

                for(int i = 0; i< searchResult.Count; i++)
                {
                    XElement xe = searchResult.ElementAt(i);
                    string result = IsCheckBoxChecked(xe, userInput);
                    WriteToXml(xe, result, xDoc);
                }
            }
        }

        private string IsCheckBoxChecked(XElement element, CheckBox[] userInput)
        {
            string toReturn = "";
            CheckBox cBox1 = userInput[0];
            CheckBox cBox2 = userInput[1];
            CheckBox cBox3 = userInput[2];

            if (element.Value == cBox1.Text)
            {
                toReturn = cBox1.Checked ? "yes" : "no";
            }
            else if (element.Value == cBox2.Text)
            {
                toReturn = cBox2.Checked ? "yes" : "no";
            }
            else if(element.Value == cBox3.Text)
            {
                toReturn = cBox3.Checked ? "yes" : "no";
            }
            return toReturn;
        }

        private bool IsAnythingChecked()
        {
            bool result = false;
            List<Repeater> reps = new List<Repeater>();
            foreach (Repeater rep in repeaters.Controls.OfType<Repeater>())
            {
                reps.Add(rep);
            }

            foreach (Repeater rep in reps)
            {
                foreach (RepeaterItem item in rep.Items)
                {
                    Label lbl = (Label)item.FindControl("question");
                    CheckBox chBox1 = (CheckBox)item.FindControl("cBox1");
                    CheckBox chBox2 = (CheckBox)item.FindControl("cBox2");
                    CheckBox chBox3 = (CheckBox)item.FindControl("cBox3");
                    if (chBox1.Checked || chBox2.Checked || chBox3.Checked)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private void WriteToXml(XElement element, string inputResult, XDocument xDoc)
        {
            element.SetAttributeValue("input", inputResult);
            string fileName = GetUserXmlFileName();
            xDoc.Save(Server.MapPath(fileName));
        }

        private void CorrectTest()
        {
            string fileName = GetUserXmlFileName();
            List<Question> questions = GetXmlContent(fileName);
            List<List<Question>> categoryLists = GetCategoryListsNoRandomize(questions);

            List<Repeater> reps = new List<Repeater>();
            foreach (Repeater rep in repeaters.Controls.OfType<Repeater>())
            {
                reps.Add(rep);
            }

            for (int i = 0; i < categoryLists.Count; i++ )
            {
                SetUserInput(categoryLists[i], reps[i]);
            }

            PopulateRepeaters(categoryLists);
            for (int i = 0; i < categoryLists.Count; i++)
            {
                SetBackcBoxToChecked(categoryLists[i], reps[i]);
            }
            CalculateScore(questions, categoryLists);
            btnSend.Enabled = false;
            finalResult.Visible = true;
            KeepInSession();
        }
      
        private void SetBackcBoxToChecked(List<Question> questions, Repeater rep)
        {
            foreach (RepeaterItem item in rep.Items)
            {
                CheckBox cBox1 = (CheckBox)item.FindControl("cBox1");
                CheckBox cBox2 = (CheckBox)item.FindControl("cBox2");
                CheckBox cBox3 = (CheckBox)item.FindControl("cBox3");

                var queryResult = from q in questions
                                  where q.Answers[0] == cBox1.Text
                                  select q.UserInput;

                foreach (List<string> userInput in queryResult)
                {
                    foreach (string input in userInput)
                    {
                        if (cBox1.Text == input)
                            cBox1.Checked = true;
                        if (cBox2.Text == input)
                            cBox2.Checked = true;
                        if (cBox3.Text == input)
                            cBox3.Checked = true;
                    }
                }
            }
        }

        private void SetUserInput(List<Question> questions, Repeater rep)
        {
            for (int i = 0; i < questions.Count; i++ )
            {
                RepeaterItem item = rep.Items[i];
                CheckBox cBox1 = (CheckBox)item.FindControl("cbox1");
                CheckBox cBox2 = (CheckBox)item.FindControl("cbox2");
                CheckBox cBox3 = (CheckBox)item.FindControl("cbox3");

                Question q = new Question();
                List<string> list = new List<string>();
                q = questions[i];
                if (cBox1.Checked)
                    list.Add(cBox1.Text);
                
                if (cBox2.Checked)
                    list.Add(cBox2.Text);
                    
                if (cBox3.Checked)
                    list.Add(cBox3.Text);
                
                q.UserInput = list;

                if (IsAnswersCorrect(q))
                {
                    q.IsCorrect = true;
                    SetCssClasses(q);
                }
                else
                {
                    q.IsCorrect = false;
                    SetCssClasses(q);
                }
            }
        }

        private bool IsAnswersCorrect(Question q)
        {
            if(q.CorrectAnswer.Count == q.UserInput.Count)
            {
                if (q.CorrectAnswer.OrderBy(x => x).SequenceEqual(q.UserInput.OrderBy(x => x)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        protected void SetCssClasses(Question q)
        {
            List<string> cssClasses = new List<string> {"", "", ""}; //needs to be same amount as Answers (Question)

            for(int i = 0; i < q.CorrectAnswer.Count; i++)
            {
                string answer = q.CorrectAnswer[i];
                int index = q.Answers.IndexOf(answer);
                cssClasses[index] = "correctanswer";
            }

            for(int i = 0; i < q.UserInput.Count; i++)
            {
                string userInput = q.UserInput[i];
                if(!q.CorrectAnswer.Contains(userInput))
                {
                    int index = q.Answers.IndexOf(userInput);
                    cssClasses[index] = "incorrectanswer";
                }
            }
            q.CssClasses = cssClasses; 
        }

        private void CalculateScore(List<Question> allQuestions, List<List<Question>> categoryLists)
        {
            List<int> totalQuestions = new List<int>();
            totalQuestions.Add(allQuestions.Count);
            
            int score = GetScoreFromList(allQuestions);
            Dictionary<string, int> allScores = new Dictionary<string, int>();
            allScores.Add("Totalt", score);

            foreach (List<Question> list in categoryLists)
            {
                totalQuestions.Add(list.Count);
                score = GetScoreFromList(list);
                allScores.Add(list[0].Category, score);
            }

            Dictionary<string, double> allPercents = new Dictionary<string, double>();

            for(int i = 0; i<allScores.Count; i++)
            {
                KeyValuePair<string, int> pair = allScores.ElementAt(i);
                int totalAmount = totalQuestions.ElementAt(i);
                double percent = GetPercentageScore(totalAmount, pair.Value);
                allPercents.Add(pair.Key, percent);
            }
            SetDataOnCharts(allPercents);
            //result.Text = String.Format("Ditt resultat blev: {0}/{1}", score, allQuestions.Count);
        }

        private int GetScoreFromList(List<Question> questions)
        {
            int score = 0;
            foreach (Question q in questions)
            {
                if (q.IsCorrect)
                    score++;
            }
            return score;
        }

        private double GetPercentageScore(int totalQuestions, int correctAnswers)
        {
            double percent = (double)correctAnswers / (double)totalQuestions * 100;
            return Math.Round(percent, 2);
        }

        private void SetDataOnCharts(Dictionary<string, double> dictAllPercents)
        {
            List<Chart> charts = new List<Chart>();
            foreach(Chart c in finalResult.Controls.OfType<Chart>())
            {
                charts.Add(c);
            }

            for(int i = 0; i<dictAllPercents.Count; i++)
            {
                KeyValuePair<string, double> pair = dictAllPercents.ElementAt(i);
                double percentLeft = 100 - pair.Value;
                string percentText = pair.Value.ToString() + "%";

                Chart c = charts.ElementAt(i);
                c.Titles[0].Text = pair.Key;

                SeriesCollection s = c.Series;
                s[0].Points.AddY(percentLeft);
                s[0].Points.AddXY(percentText, pair.Value);
                s[0].Points[0].Color = Color.Red;
                s[0].Points[1].Color = Color.LightGreen;
            }
        }

        private void KeepInSession()
        {
            Session["IsFirstTime"] = false;
        }

        private void SendUserXmlToDb() //here's one Request.QueryString
        {
            DatabaseConnection db = new DatabaseConnection();
            //string userName = Request.QueryString["userName"];
            string id = db.GetUserId("stare"); //will be userName later, not "stare"
            List<string> userXmls = db.RetrieveXmlDocument(id, DateTime.Today);

            if (userXmls.Count < 1) 
            {
                string fileName = GetUserXmlFileName();
                XDocument xDoc = XDocument.Load(Server.MapPath(fileName));
                db.SaveUserXml(id, xDoc, DateTime.Today);
            }
            else 
            {
                //person has already done a test today
            }
        }

        private XDocument GetUserXmlFromDb()
        {
            DatabaseConnection db = new DatabaseConnection();
            //string userName = Request.QueryString["userName"];
            string id = db.GetUserId("stare"); //will be userName later, not "stare"
            List<string> userXmls = db.RetrieveXmlDocument(id, DateTime.Today);

            XDocument xDoc = XDocument.Parse(userXmls[0]);
            XDeclaration xDec = xDoc.Declaration; //for some reason postgres changes encoding to utf-16, changing it back!
            xDec.Encoding = "utf-8";

            string fileName = GetUserXmlFileName();
            xDoc.Save(Server.MapPath(fileName));

            return xDoc;
        }

        private string GetUserXmlFileName() //here's one Request.QueryString
        {
            //string userName = Request.QueryString["userName"];
            string userXmlFileName = string.Format("~/xml/{0}.xml", "stare"); //again will be userName later, not "stare"
            return userXmlFileName;
        }
    }
}
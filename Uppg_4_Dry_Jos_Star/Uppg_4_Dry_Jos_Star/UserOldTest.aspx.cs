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
    public partial class UserOldTest : System.Web.UI.Page
    {
        private readonly object syncLock = new object();
        private readonly Random rand = new Random();

        protected void Page_Load(object sender, EventArgs e) //Session[userName]
        {
            string userName = Session["userName"].ToString();
            lblUserName.Text = userName;

            //string fileName = GetUserXmlFileName();
            List<Question> questions = GetXmlContent();
            List<List<Question>> categoryLists = GetCategoryListsNoRandomize(questions);
            PopulateRepeaters(categoryLists);
            CorrectTest();
            
            //finalResult.Visible = true;
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

        //----------------------------------------------------------------------------------------------------------

        private List<Question> GetXmlContent()
        {
            //XDocument xDoc = XDocument.Load(Server.MapPath(xmlVirtualPath));
            XDocument xDoc = GetUserXmlFromDb(); //Uses method to get latest test from DB and read it to htmlpage.
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
                                UserInput = q.Elements("answer").Where(x => x.Attribute("input").Value == "yes")
                                                                .Select(x => x.Value).ToList(),
                            };

            List<Question> questions = new List<Question>();
            foreach (Question q in xmlResult)
            {
                questions.Add(q);
            }
            var queryResult = from e in xDoc.Descendants("question")
                              where e.Attribute("questionPictureUrl") != null
                              select e;
            foreach (XElement xe in queryResult)
            {
                Question q = questions.FirstOrDefault(x => x.Text == xe.Element("text").Value);
                q.QuestionPictureUrl = xe.Attribute("questionPictureUrl").Value;
            }
            return questions;
        }

        private List<List<Question>> GetCategoryListsNoRandomize(List<Question> questionList)
        {
            List<List<Question>> allCategoryQuestions = new List<List<Question>>();
            int count = 0;
            Dictionary<string, List<Question>> dictCategoryQuestions = GetCategoriesWithQuestions(questionList);
            List<string> categories = dictCategoryQuestions.Keys.ToList();

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

        private Dictionary<string, List<Question>> GetCategoriesWithQuestions(List<Question> questionList)
        {
            Dictionary<string, List<Question>> dictQuestionsCategory = new Dictionary<string, List<Question>>();
            var grouped = questionList.GroupBy(x => x.Category);
            foreach (var pair in grouped)
            {
                List<Question> questions = new List<Question>();
                foreach (Question q in pair)
                {
                    questions.Add(q);
                }
                dictQuestionsCategory.Add(pair.Key, questions);
            }
            return dictQuestionsCategory;
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

        private void CorrectTest()
        {
            //string fileName = GetUserXmlFileName();
            List<Question> questions = GetXmlContent();
            List<List<Question>> categoryLists = GetCategoryListsNoRandomize(questions);

            List<Repeater> reps = new List<Repeater>();
            foreach (Repeater rep in repeaters.Controls.OfType<Repeater>())
            {
                reps.Add(rep);
            }

            for (int i = 0; i < categoryLists.Count; i++)
            {
                SetUserInput(categoryLists[i], reps[i]);
            }

            PopulateRepeaters(categoryLists);
            for (int i = 0; i < categoryLists.Count; i++)
            {
                SetBackcBoxToChecked(categoryLists[i], reps[i]);
            }
            CalculateScore(questions, categoryLists);
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
            for (int i = 0; i < questions.Count; i++)
            {
                RepeaterItem item = rep.Items[i];
                CheckBox cBox1 = (CheckBox)item.FindControl("cbox1");
                CheckBox cBox2 = (CheckBox)item.FindControl("cbox2");
                CheckBox cBox3 = (CheckBox)item.FindControl("cbox3");

                Question q = questions[i];
                
                System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)item.FindControl("questionImage");

                if (IsAnswersCorrect(q))
                {
                    q.IsCorrect = true;
                    q.AnswerImageUrl = "~/img/btn_correct.png";
                    SetCssClasses(q);
                }
                else
                {
                    q.IsCorrect = false;
                    q.AnswerImageUrl = "~/img/btn_incorrect.png";
                    SetCssClasses(q);
                }
            }
        }

        private bool IsAnswersCorrect(Question q)
        {
            if (q.CorrectAnswer.Count == q.UserInput.Count)
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
            List<string> cssClasses = new List<string> { "", "", "" }; //needs to be same amount as Answers (Question)

            for (int i = 0; i < q.CorrectAnswer.Count; i++)
            {
                string answer = q.CorrectAnswer[i];
                int index = q.Answers.IndexOf(answer);
                cssClasses[index] = "correctanswer";
            }

            for (int i = 0; i < q.UserInput.Count; i++)
            {
                string userInput = q.UserInput[i];
                if (!q.CorrectAnswer.Contains(userInput))
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

            for (int i = 0; i < allScores.Count; i++)
            {
                KeyValuePair<string, int> pair = allScores.ElementAt(i);
                int totalAmount = totalQuestions.ElementAt(i);
                double percent = GetPercentageScore(totalAmount, pair.Value);
                allPercents.Add(pair.Key, percent);
            }
            SetDataOnCharts(allPercents, allScores, totalQuestions);
            SetTestResultImageUrl(allPercents);
            //UpdateDbWithResult(allScores, totalQuestions, allPercents);
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

        private void SetDataOnCharts(Dictionary<string, double> dictAllPercents, Dictionary<string, int> dictAllScores, List<int> totalQuestions)
        {
            List<Chart> charts = new List<Chart>();
            foreach (Chart c in finalResult.Controls.OfType<Chart>())
            {
                charts.Add(c);
            }
            List<Label> labels = new List<Label>();
            foreach (Label lbl in finalResult.Controls.OfType<Label>())
            {
                if (lbl.Text != "Provresultat:")
                    lbl.Visible = false;

                labels.Add(lbl);
            }

            for (int i = 0; i < dictAllPercents.Count; i++)
            {
                KeyValuePair<string, double> pair = dictAllPercents.ElementAt(i);
                double percentLeft = 100 - pair.Value;
                string percentText = pair.Value.ToString() + "%";

                Chart c = charts.ElementAt(i);
                c.Titles[0].Text = pair.Key;
                if (c.Titles[0].Text == "Totalt")
                {
                    c.Titles[0].Font = new Font("Lato-light", 12, FontStyle.Bold);
                    c.Titles[0].Text += "\n";
                }
                else if (c.Titles[0].Text == "Etik och regelverk")
                {
                    c.Titles[0].Font = new Font("Lato-light", 12, FontStyle.Italic);
                    c.Titles[0].Text += "\n";
                }
                else
                    c.Titles[0].Font = new Font("Lato-light", 12, FontStyle.Italic);

                SeriesCollection s = c.Series;
                s[0].Points.AddY(percentLeft);
                s[0].Points.AddXY(percentText, pair.Value);
                s[0].Points[0].Color = Color.Red;
                s[0].Points[1].Color = Color.LightGreen;
                s[0].Points[1]["Exploded"] = "true";

                string result = String.Format("Poäng: {0}/{1}", dictAllScores[pair.Key], totalQuestions[i]);
                Label lbl = (Label)labels[i];
                lbl.Text = result;
                lbl.Visible = true;
            }
        }

        private bool IsTestPassed(Dictionary<string, double> dictAllPercents)
        {
            bool result = true;
            foreach (KeyValuePair<string, double> pair in dictAllPercents)
            {
                if (pair.Key == "Totalt" && pair.Value < 70) //total have to be 70 % or higher to be passed
                {
                    result = false;
                    break;
                }
                if (pair.Key != "Totalt" && pair.Value < 60) //in each category at least 60 % has to be correct
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private void SetTestResultImageUrl(Dictionary<string, double> allPercents)
        {
            if (IsTestPassed(allPercents))
                yesNoImg.ImageUrl = "~/img/btn_correct.png";
            else
                yesNoImg.ImageUrl = "~/img/btn_incorrect.png";
        }

        private void KeepInSession()
        {
            Session["IsFirstTime"] = false;
        }

        /// <summary>
        /// Updated to only get latest test from one specific user. 
        /// </summary>
        /// <returns></returns>
        private XDocument GetUserXmlFromDb() //Session["userName"]
        {
            DatabaseConnection db = new DatabaseConnection();
            string userName = Session["userName"].ToString();
            string id = db.GetUserId(userName); 
            List<string> userXmls = db.GetUserLatestTest(id);

            XDocument xDoc = XDocument.Parse(userXmls[0]);
            XDeclaration xDec = xDoc.Declaration; //for some reason postgres changes encoding to utf-16, changing it back!
            xDec.Encoding = "utf-8";

            string fileName = GetUserXmlFileName();
            xDoc.Save(Server.MapPath(fileName));

            return xDoc;
        }

        private string GetUserXmlFileName()
        {
            string userName = lblUserName.Text;
            string userXmlFileName = string.Format("~/xml/{0}.xml", userName);
            return userXmlFileName;
        }
    }
}
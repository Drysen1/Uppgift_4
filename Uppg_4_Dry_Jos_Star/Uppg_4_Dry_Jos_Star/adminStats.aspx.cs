using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.UI.DataVisualization.Charting;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class adminStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PopulateDropDownLists();
                Initialize();
            }
        }

        protected void pickTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void pickTestCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void gViewStatsCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.BackColor = Color.BlanchedAlmond;
            }
            else
            {
                for (int i = 2; i < e.Row.Cells.Count - 2; i++) //first two columns is already taken, last two columns is not to be populated in loop
                {
                    if (e.Row.Cells[i].Text == "R&#228;tt") // makes ä = &#228;
                        e.Row.Cells[i].BackColor = Color.LightGreen;
                    else if (e.Row.Cells[i].Text.StartsWith("#"))
                        e.Row.Cells[i].BackColor = Color.BlanchedAlmond;
                    else if (e.Row.Cells[i].Text == "Fel")
                        e.Row.Cells[i].BackColor = Color.Tomato;
                    else if (!e.Row.Cells[i].Text.StartsWith("#"))
                        e.Row.Cells[i].Text = "Ingår ej";
                }
            }
        }

        //----------------------------------------------------------------------------------------------

        private void PopulateDropDownLists()
        {
            XDocument xDoc = XDocument.Load(Server.MapPath("~/xml/questions.xml"));

            var queryResult = from c in xDoc.Descendants("category")
                              orderby c.Attribute("type").Value
                              select c.Attribute("type").Value;

            pickTestCategory.DataSource = queryResult;
            pickTestCategory.DataBind();

            pickTestType.Items.Add("LST");
            pickTestType.Items.Add("ÅKU");
        }

        private void Initialize()
        {
            List<Person> personsWithTest = GetAllTests();
            List<List<Question>> allPersonsWithQuestions = CreateQuestions(personsWithTest);

            List<List<CategoryStats>> allCategoryStats = new List<List<CategoryStats>>();
            for (int i = 0; i < personsWithTest.Count; i++)
            {
                List<Question> questions = allPersonsWithQuestions.ElementAt(i);
                List<List<Question>> categoryLists = GetCategoryLists(questions);

                string fullName = personsWithTest[i].FirstName + " " + personsWithTest[i].LastName;
                string testDate = personsWithTest[i].TestDate;
                string testScore = personsWithTest[i].TestScore;
                List<CategoryStats> categoryStats = GetCategoryStats(categoryLists, fullName, testDate, testScore);
                allCategoryStats.Add(categoryStats);
            }
            DataTable dt = CreateDataTable();
            PopulateGridViews(allCategoryStats, dt);
            PopulateChart();
        }

        private List<Person> GetAllTests() //Session["username"]
        {
            DatabaseConnection dr = new DatabaseConnection();
            string userName = Session["userName"].ToString();
            return dr.RetrieveAllXmlDocuments(pickTestType.Text, userName);
        }

        private List<Person> GetAllTestsRegardlessType() //Session["username"]
        {
            DatabaseConnection dr = new DatabaseConnection();
            string userName = Session["userName"].ToString();
            return dr.RetrieveAllXmlDocuments(userName);
        }

        private List<List<Question>> CreateQuestions(List<Person> persons)
        {
            List<List<Question>> allTests = new List<List<Question>>();
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
                List<Question> defaultQuestions = GetXmlContent("~/xml/questions.xml");
                var queryResult = from d in defaultQuestions
                                  join q in questions
                                  on d.Id equals q.Id
                                  select q;

                allTests.Add(queryResult.ToList());
            }
            CorrectTest(allTests);
            return allTests;
        }

        private void CorrectTest(List<List<Question>> allTests)
        {
            foreach(List<Question> list in allTests)
            {
                foreach(Question q in list)
                {
                    if (IsAnswerCorrect(q.CorrectAnswer, q.UserInput))
                        q.IsCorrect = true;
                    else
                        q.IsCorrect = false;
                }
            }
        }

        private bool IsAnswerCorrect(List<string> correctAnswers, List<string> userInput)
        {
            if (correctAnswers.Count == userInput.Count)
            {
                if (correctAnswers.OrderBy(x => x).SequenceEqual(userInput.OrderBy(x => x)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private List<List<Question>> GetCategoryLists(List<Question> questionList)
        {
            List<List<Question>> allCategoryQuestions = new List<List<Question>>();
            Dictionary<string, List<Question>> dictCategoryQuestions = GetCategoriesWithQuestions(questionList);
            List<string> categories = dictCategoryQuestions.Keys.ToList();
            categories.Sort();

            foreach (string c in categories)
            {
                var result = from q in questionList
                             where q.Category == c
                             select q;

                List<Question> categoryQuestions = new List<Question>();

                foreach (Question quest in result)
                {
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

        private List<CategoryStats> GetCategoryStats(List<List<Question>> categoryLists, string fullName, string testDate, string testScore)
        {
            List<Question> defaultQuestions = GetXmlContent("~/xml/questions.xml");
            List<CategoryStats> categoryStats = new List<CategoryStats>();
            
            Dictionary<string, bool> dictCorrectAnswers = new Dictionary<string, bool>();

            for(int i = 0; i<categoryLists.Count; i++)
            {
                List<Question> questions = categoryLists[i];
                
                if(questions[0].Category == "Etik och regelverk")
                {
                    categoryStats = GetChoosenCategoryStats(categoryStats, questions, fullName, testDate, testScore, defaultQuestions);
                }
                else if (questions[0].Category == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                {
                    categoryStats = GetChoosenCategoryStats(categoryStats, questions, fullName, testDate, testScore, defaultQuestions);
                }
                else if (questions[0].Category == "Produkter och hantering av kundens affärer")
                {
                    categoryStats = GetChoosenCategoryStats(categoryStats, questions, fullName, testDate, testScore, defaultQuestions);
                }
            }
            return categoryStats;
        }

        private List<CategoryStats> GetChoosenCategoryStats(List<CategoryStats> categoryStats, List<Question> questions, string fullName, string testDate, string testScore, List<Question> defaultQuestions)
        {
            Dictionary<string, bool> correctedAnswers = GetCategoryAnswers(defaultQuestions, questions);

            categoryStats.Add(
                    new CategoryStats
                    {
                        CategoryName = questions[0].Category,
                        FullName = fullName,
                        TestDate = testDate,
                        QuestionsResult = correctedAnswers,
                        CategoryScore = GetCategoryScore(correctedAnswers),
                        TotalScore = testScore,
                    });
            return categoryStats;
        }

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

        private Dictionary<string, bool> GetCategoryAnswers(List<Question> defaultQuestions, List<Question> questions)
        {
            Dictionary<string, bool> dictCorrectAnswers = new Dictionary<string, bool>();
            bool isAnswerCorrect;

            for (int j = 0; j < defaultQuestions.Count; j++)
            {
                var resultQuery = from q in questions
                                  join dq in defaultQuestions
                                  on q.Id equals dq.Id
                                  where dq.Id == (j + 1).ToString()
                                  select q;
                foreach (Question q in resultQuery)
                {
                    isAnswerCorrect = q.IsCorrect;
                    dictCorrectAnswers.Add(q.Text, isAnswerCorrect);
                }
            }
            return dictCorrectAnswers;
        }

        private string GetCategoryScore(Dictionary<string, bool> correctedAnswers)
        {
            string totalQuestions = correctedAnswers.Count.ToString();
            string numCorrect = correctedAnswers.Where(x => x.Value == true).Count().ToString();
            return String.Format("{0}/{1}", numCorrect, totalQuestions);
        }

        private DataTable CreateDataTable() 
        {
            List<Question> defaultQuestions = GetXmlContent("~/xml/questions.xml");

            var queryResult = defaultQuestions.Where(x => x.Category == pickTestCategory.Text);
            
            DataTable dt = new DataTable();
            DataColumn clmName = new DataColumn("Namn");
            DataColumn clmTestDate = new DataColumn("Provdatum");
            dt.Columns.Add(clmName);
            dt.Columns.Add(clmTestDate);

            List<string> idNumbers = new List<string>();
            
            foreach(Question q in queryResult)
            {
                DataColumn clmQuestion = new DataColumn(q.Text);
                dt.Columns.Add(clmQuestion);
                idNumbers.Add("#" + q.Id);
            }

            DataColumn clmCategoryScore = new DataColumn("Poäng kategori");
            dt.Columns.Add(clmCategoryScore);
            DataColumn clmTotalScore = new DataColumn("Totalpoäng");
            dt.Columns.Add(clmTotalScore);

            DataRow dr = dt.NewRow();
            for (int i = 2; i < dt.Columns.Count - 2; i++)
            {
                string columnName = dt.Columns[i].ColumnName;
                dr[columnName] = idNumbers[i - 2];
            }
            dt.Rows.Add(dr);
            return dt;
        }

        private void PopulateGridViews(List<List<CategoryStats>> allCategoryStats, DataTable dt)
        {
            bool isAnswerCorrect;
            foreach (List<CategoryStats> csList in allCategoryStats)
            {
                DataRow dr = dt.NewRow();
                var queryResult = csList.Where(x => x.CategoryName == pickTestCategory.Text);
                foreach (CategoryStats cs in queryResult)
                {
                    dr["Namn"] = cs.FullName;
                    dr["Provdatum"] = cs.TestDate;

                    for(int i = 2; i<dt.Columns.Count -2; i++) //first two columns is already taken, last two columns is not to be populated in loop
                    {
                        string columnName = dt.Columns[i].ColumnName;
                        if(cs.QuestionsResult.TryGetValue(columnName, out isAnswerCorrect))
                        {
                            if (isAnswerCorrect)
                                dr[columnName] = "Rätt";
                            else
                                dr[columnName] = "Fel";
                        }
                    }
                    dr["Poäng kategori"] = cs.CategoryScore;
                    dr["Totalpoäng"] = cs.TotalScore;
                }
                dt.Rows.Add(dr);
            }
            gViewStatsCategory.DataSource = dt;
            gViewStatsCategory.DataBind();
        }

        private void PopulateChart()
        {
            List<Person> personsWithTest = GetAllTestsRegardlessType();
            List<List<Question>> allPersonsWithQuestions = CreateQuestions(personsWithTest);
            BindChart(allPersonsWithQuestions);
        }

        private void BindChart(List<List<Question>> allPersonsWithQuestions)
        {
            List<Question> defaultQuestions = GetXmlContent("~/xml/questions.xml");

            List<int> listOfCorrectAnswers = new List<int>();
            List<int> listOfTotalAnswers = new List<int>();
            
            for (int i = 0; i < defaultQuestions.Count; i++)
            {
                int correctAnswers = 0;
                int totalAnswers = 0;

                foreach (List<Question> list in allPersonsWithQuestions)
                {
                    Tuple <bool, Question> returnedItems = DoesAnswerExist(defaultQuestions[i], list);
                    if (returnedItems.Item1 == true)
                    {
                        if(returnedItems.Item2.IsCorrect)
                        {
                            correctAnswers++;
                            totalAnswers++;
                        }
                        else
                        {
                            totalAnswers++;
                        }
                    }
                }
                listOfCorrectAnswers.Add(correctAnswers);
                listOfTotalAnswers.Add(totalAnswers);
            }
            SetChart(listOfCorrectAnswers, listOfTotalAnswers);
        }

        private Tuple<bool, Question> DoesAnswerExist(Question checkQuestion, List<Question> personsWithQuestions)
        {
            
            Question q = personsWithQuestions.FirstOrDefault(x => x.Id == checkQuestion.Id);
            if (q != null)
            {
                Tuple<bool, Question> tuple = new Tuple<bool, Question>(true,q);
                return tuple;
            }
                
            else
            {
                Tuple<bool, Question> tuple2 = new Tuple<bool, Question>(false, personsWithQuestions[0]);
                return tuple2;
            }
        }

        private void SetChart(List<int> listOfCorrectAnswers, List<int> listOfTotalAnswers)
        {
            chartTotalStats.Series["correctAnswers"].Points.DataBindY(listOfCorrectAnswers);
            chartTotalStats.Series["correctAnswers"].Color = Color.LightGreen;
            chartTotalStats.Series["totalAnswers"].Points.DataBindY(listOfTotalAnswers);
            chartTotalStats.Series["totalAnswers"].Color = Color.Gray;

            chartTotalStats.Legends["legend"].Docking = Docking.Bottom;
            chartTotalStats.Series["correctAnswers"].LegendText = "Antal rätt";
            chartTotalStats.Series["totalAnswers"].LegendText = "Totalantal svar";

            chartTotalStats.Titles["title"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
            chartTotalStats.ChartAreas["chartArea"].AxisX.TitleFont = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
            chartTotalStats.ChartAreas["chartArea"].AxisY.TitleFont = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
        }
    }
}
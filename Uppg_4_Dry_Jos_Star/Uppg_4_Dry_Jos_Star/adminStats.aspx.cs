using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Uppg_4_Dry_Jos_Star
{
    public partial class adminStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PopulateDropDownLists();
                List<Person> personsWithTest = GetAlltests();
                Dictionary<string, List<Question>> dictPersonsWithQuestions = CreateQuestions(personsWithTest);
                
                List<List<CategoryStats>> allCategoryStats = new List<List<CategoryStats>>();
                for(int i = 0; i<personsWithTest.Count; i++)
                {
                    KeyValuePair<string, List<Question>> pair = dictPersonsWithQuestions.ElementAt(i);

                    List<List<Question>> categoryLists = GetCategoryLists(pair.Value);
                    List<CategoryStats> categoryStats = GetCategoryStats(categoryLists, pair.Key, personsWithTest[i].TestScore);
                    allCategoryStats.Add(categoryStats);
                }
                PopulateGridViews(allCategoryStats);
            }
        }

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

        private List<Person> GetAlltests()
        {
            DatabaseConnection dr = new DatabaseConnection();
            return dr.RetrieveAllXmlDocuments(pickTestType.Text);
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
                dictAllTests.Add(p.FirstName + " " + p.LastName, questions);
            }
            CorrectTest(dictAllTests);
            return dictAllTests;
        }

        private void CorrectTest(Dictionary<string, List<Question>> dictAllTests)
        {
            foreach(KeyValuePair<string, List<Question>> pair in dictAllTests)
            {
                foreach (Question q in pair.Value)
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

        private List<CategoryStats> GetCategoryStats(List<List<Question>> categoryLists, string fullName, string testScore)
        {
            List<Question> defaultQuestions = GetXmlContent("~/xml/questions.xml");
            List<CategoryStats> categoryStats = new List<CategoryStats>();
            
            Dictionary<string, bool> dictCorrectAnswers = new Dictionary<string, bool>();

            for(int i = 0; i<categoryLists.Count; i++)
            {
                List<Question> questions = categoryLists[i];
                
                if(questions[0].Category == "Etik och regelverk")
                {
                    categoryStats = GetChoosenCategoryStats(categoryStats, questions, fullName, testScore, defaultQuestions);
                }
                else if (questions[0].Category == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                {
                    categoryStats = GetChoosenCategoryStats(categoryStats, questions, fullName, testScore, defaultQuestions);
                }
                else if (questions[0].Category == "Produkter och hantering av kundens affärer")
                {
                    categoryStats = GetChoosenCategoryStats(categoryStats, questions, fullName, testScore, defaultQuestions);
                }
            }
            return categoryStats;
        }

        private List<CategoryStats> GetChoosenCategoryStats(List<CategoryStats> categoryStats, List<Question> questions, string fullName, string testScore, List<Question> defaultQuestions)
        {
            Dictionary<string, bool> correctedAnswers = GetCategoryAnswers(defaultQuestions, questions);

            categoryStats.Add(
                    new CategoryStats
                    {
                        CategoryName = questions[0].Category,
                        FullName = fullName,
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

        private void PopulateGridViews(List<List<CategoryStats>> allCategoryStats) //hardcoded category right now, will get value from dropDownlist later
        {
            foreach (List<CategoryStats> csList in allCategoryStats)
            {
                var queryResult = csList.Where(x => x.CategoryName == pickTestCategory.Text);
                gViewStatsCategory.DataSource = "";
                gViewStatsCategory.DataBind();
            }
        }

        protected void pickTestType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void pickTestCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
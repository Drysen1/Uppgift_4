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

                Repeater1.DataSource = categoryLists[0];
                Repeater1.DataBind();
                Repeater2.DataSource = categoryLists[1];
                Repeater2.DataBind();
                Repeater3.DataSource = categoryLists[2];
                Repeater3.DataBind();
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

            List<int> numbers = GetRandomOrder(questionList.Count); //create random numbers according to amount of questions
            questionList = GetRandomizedList(questionList, numbers);
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

        private List<Question> GetRandomizedList(List<Question> questions, List<int> order)
        {
            List<Question> randomizedList = new List<Question>();
            foreach(int number in order)
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

        private List<int> GetRandomOrder(int amountOfNums)
        {
            lock(syncLock)
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
    }
}
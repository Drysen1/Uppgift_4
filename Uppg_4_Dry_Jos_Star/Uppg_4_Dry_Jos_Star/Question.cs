using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uppg_4_Dry_Jos_Star
{
    public class Question
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public List<string> Answers { get; set; }
        public string AnswerOrder { get; set; }
        public List<string> CorrectAnswer { get; set; }
        public string NumOfCorrect { get; set; }
        public List<string> UserInput { get; set; }
        public bool IsCorrect { get; set; }
        public List<string> CssClasses { get; set; }
        public string AnswerImageUrl { get; set; }
        public string QuestionPictureUrl { get; set; }
    }
}
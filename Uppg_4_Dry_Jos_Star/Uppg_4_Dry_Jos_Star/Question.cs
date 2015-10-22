using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uppg_4_Dry_Jos_Star
{
    public class Question
    {
        public string QuestionNumber { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string NumOfCorrect { get; set; }
        public List<string> Answers { get; set; }
        public List<string> CorrectAnswer { get; set; }
        public List<string> UserInput { get; set; }
    }
}
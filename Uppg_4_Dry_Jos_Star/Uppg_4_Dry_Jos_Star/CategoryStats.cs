using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uppg_4_Dry_Jos_Star
{
    public class CategoryStats
    {
        public string CategoryName { get; set; }
        public string FullName { get; set; }
        public Dictionary<string, bool> QuestionsResult { get; set; }
        public string CategoryScore { get; set; }
        public string TotalScore { get; set; }
    }
}
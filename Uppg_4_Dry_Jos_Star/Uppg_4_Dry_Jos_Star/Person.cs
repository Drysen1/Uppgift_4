﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Uppg_4_Dry_Jos_Star
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string TestType { get; set; }
        public string TestDate { get; set; }
        public string TestScore { get; set; }
        public string TestGrade { get; set; }
        public string TestWaiting { get; set; }
        public XDocument xmlTest { get; set; }
    }
}
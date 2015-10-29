using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uppg_4_Dry_Jos_Star
{
    public class Person
    {
        private string firstName;
        private string lastName;

        //metod som inte har en returtyp, property
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value;  }
        }
    }
}
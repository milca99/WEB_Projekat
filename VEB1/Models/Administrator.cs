using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Administrator
    {
        
        public string UserName { get; set; }                                                     // (jedinstveno)
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public Administrator(string userName, string password, string name, string lastName, DateTime birthday)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            Birthday = birthday;
        }

        public Administrator() { }
    }
}
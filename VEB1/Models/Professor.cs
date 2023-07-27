using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Professor
    {
        public string UserName { get; set; }                                                     // (jedinstveno)
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public List<string> Subjects { get; set; }
        public List<Exam> Exams { get; set; }

        public Professor() { }

        public Professor(string userName, string password, string name, string lastName, string email, DateTime birthday)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            Email = email;
            Birthday = birthday;
            Subjects = new List<string>();
            Exams = new List<Exam>();
        }
    }
}
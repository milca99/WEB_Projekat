using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Student
    {
        public string UserName { get; set; }                                                     // (jedinstveno)
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string IndexNumber { get; set; }                                                 // (jedinstveno)
        public bool Deleted { get; set; }
        public List<Exam> RegisterdExam { get; set; }
        public List<Exam> PassedExam { get; set; }
        public List<Exam> FailedExam { get; set; }

        public Student(string userName, string password, string name, string lastName, string email, DateTime birthday, string indexNumber, bool deleted)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            Email = email;
            Birthday = birthday;
            IndexNumber = indexNumber;
            Deleted = deleted;
            RegisterdExam = new List<Exam>();
            PassedExam = new List<Exam>();
            FailedExam = new List<Exam>();
        }

        public Student() { }
    }
}
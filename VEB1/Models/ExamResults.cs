using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class ExamResults
    {
        public int Exam { get; set; }                                                     // (jedinstveno)
        public string Student { get; set; }
        public int Grade { get; set; }

        public ExamResults(int exam, string student, int grade)
        {
            Exam = exam;
            Student = student;
            Grade = grade;
        }

        public ExamResults() { }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Exam
    {

        public int ExamId { get; set; }
        public string Professor { get; set; }
        public string Subject { get; set; }
        public string Classroom { get; set; }
        public string ExamPeriod { get; set; }
        public DateTime DateOfExam { get; set; }

        public Exam(int examId, string professor, string subject, string classroom, string examPeriod, DateTime dateOfExam)
        {
            ExamId = examId;
            Professor = professor;
            Subject = subject;
            Classroom = classroom;
            ExamPeriod = examPeriod;
            DateOfExam = dateOfExam;
        }

        public Exam() { }

        public override bool Equals(object obj)
        {
            Exam e = (Exam)obj;
            return this.ExamId == e.ExamId;
        }
    }
}
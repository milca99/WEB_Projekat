using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class DataLists
    {
        public static List<Administrator> AdministratorList { get; set; }
        public static List<Exam> ExamList { get; set; }
        public static List<ExamResults> ExamResultList { get; set; }
        public static List<Professor> ProfessorList { get; set; }
        public static List<Student> StudentList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;

namespace Projekat.Models
{
    public class Data
    {
        public static List<Student> ReadStudents(string path)
        {
            List<Student> students = new List<Student>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('[');
                string[] tokens = parts[0].Split('|');
                Student student = new Student(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], DateTime.Parse(tokens[5]), tokens[6], Boolean.Parse(tokens[7]));
                

                string[] registered = parts[1].Split('|');
                string[] passed = parts[2].Split('|');
                string[] failed = parts[3].Split('|');

                if(registered.Count()!=0 || passed.Count() != 0 || failed.Count() != 0)
                {
                    List<Exam> exams = ReadExams("~/App_Data/ExamsData.dsv");

                    foreach (var item in registered)
                    {
                        if (item.Equals(""))
                            continue;
                        int id = int.Parse(item);
                        Exam e = exams.FirstOrDefault(x => x.ExamId == id);
                        if (e != null)
                            student.RegisterdExam.Add(e);
                        
                    }

                    foreach (var item in passed)
                    {
                        if (item.Equals(""))
                            continue;
                        int id = int.Parse(item);
                        Exam e = exams.FirstOrDefault(x => x.ExamId == id);
                        if (e != null)
                            student.PassedExam.Add(e);

                    }

                    foreach (var item in failed)
                    {
                        if (item.Equals(""))
                            continue;
                        int id = int.Parse(item);
                        Exam e = exams.FirstOrDefault(x => x.ExamId == id);
                        if (e != null)
                            student.FailedExam.Add(e);

                    }

                    students.Add(student);
                }
            }
            sr.Close();
            stream.Close();

            return students;
        }


        public static void SaveStudent(Student student)
        {
            string userToString = "";
            userToString = student.UserName + "|" + student.Password + "|" + student.Name + "|" + student.LastName + "|" + student.Email + "|" + student.Birthday.ToString("dd/MM/yyyy") + "|" + student.IndexNumber + "|" + student.Deleted + "[";

            if (student.RegisterdExam != null)
            {
                for (int i = 0; i < student.RegisterdExam.Count(); i++)
                {
                    userToString += student.RegisterdExam[i].ExamId + "|";
                }
            }
            userToString += "[";
            if (student.PassedExam != null)
            {
                for (int i = 0; i < student.PassedExam.Count(); i++)
                {
                    userToString += student.PassedExam[i].ExamId + "|";
                }
            }
            userToString += "[";
            if (student.FailedExam != null)
            {
                for (int i = 0; i < student.FailedExam.Count(); i++)
                {
                    userToString += student.FailedExam[i].ExamId + "|";
                }
            }
            // save student in file users.txt
            string path = HostingEnvironment.MapPath("~/App_Data/StudentsData.dsv");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(userToString);
            }
        }

        public static void SaveStudents(List<Student> students)
        {
            var path = HostingEnvironment.MapPath("~/App_Data/StudentsData.dsv");
            FileStream stream = new FileStream(path, FileMode.Truncate, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (var student in students)
                {
                    string userToString = "";
                    userToString = student.UserName + "|" + student.Password + "|" + student.Name + "|" + student.LastName + "|" + student.Email + "|" + student.Birthday.ToString("dd/MM/yyyy") + "|" + student.IndexNumber + "|" + student.Deleted + "[";
                    if (student.RegisterdExam != null)
                    {
                        for (int i = 0; i < student.RegisterdExam.Count(); i++)
                        {
                            userToString += student.RegisterdExam[i].ExamId + "|";
                        }
                    }
                    userToString += "[";
                    if (student.PassedExam != null)
                    {
                        for (int i = 0; i < student.PassedExam.Count(); i++)
                        {
                            userToString += student.PassedExam[i].ExamId + "|";
                        }
                    }
                    userToString += "[";
                    if (student.FailedExam != null)
                    {
                        for (int i = 0; i < student.FailedExam.Count(); i++)
                        {
                            userToString += student.FailedExam[i].ExamId + "|";
                        }
                    }
                    sw.WriteLine(userToString);
                }
            }
            
        }

        public static List<Exam> ReadExams(string path)
        {
            List<Exam> exams = new List<Exam>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');
                Exam exam = new Exam(int.Parse(tokens[0]), tokens[1], tokens[2], tokens[3], tokens[4], DateTime.Parse(tokens[5]));
                exams.Add(exam);
            }
            sr.Close();
            stream.Close();

            return exams;
        }

        public static void SaveExam(Exam exam)
        {
            string examString = exam.ExamId + "|" + exam.Professor + "|" + exam.Subject + "|" + exam.Classroom + "|" + exam.ExamPeriod + "|" + exam.DateOfExam;
            string path = HostingEnvironment.MapPath("~/App_Data/ExamsData.dsv");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(examString);
            }
        }

        public static List<Administrator> ReadAdministrators(string path)
        {
            List<Administrator> administrators = new List<Administrator>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');
                Administrator administrator = new Administrator(tokens[0], tokens[1], tokens[2], tokens[3], DateTime.Parse(tokens[4]));
                administrators.Add(administrator);
            }
            sr.Close();
            stream.Close();

            return administrators;
        }

        public static void SaveAdministrator(Administrator administrator)
        {
            string adminString = administrator.UserName + "|" + administrator.Password + "|" + administrator.Name + "|" + administrator.LastName + "|" + administrator.Birthday.ToString("dd/MM/yyyy");
            string path = HostingEnvironment.MapPath("~/App_Data/AdministratorsData.dsv");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(adminString);
            }
        }

        public static List<ExamResults> ReadExamResults(string path)
        {
            List<ExamResults> examResults = new List<ExamResults>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');
                ExamResults examResult = new ExamResults(int.Parse(tokens[0]), tokens[1], int.Parse(tokens[2]));
                examResults.Add(examResult);
            }
            sr.Close();
            stream.Close();

            return examResults;
        }

        public static void SaveExamResult(ExamResults examResult)
        {
            string examString = examResult.Exam + "|" + examResult.Student + "|" + examResult.Grade;
            string path = HostingEnvironment.MapPath("~/App_Data/ExamResultsData.dsv");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(examString);
            }
        }

        public static List<Professor> ReadProfessors(string path)
        {
            List<Professor> professors = new List<Professor>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('[');
                string[] tokens = parts[0].Split('|');
                Professor professor = new Professor(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], DateTime.Parse(tokens[5]));
                

                string[] subjects = parts[1].Split('|');
                string[] profExams = parts[2].Split('|');

                foreach (var item in subjects)
                {
                    if(!item.Equals(""))
                        professor.Subjects.Add(item);
                }

                if (profExams.Count() !=0)
                {
                    List<Exam> exams = ReadExams("~/App_Data/ExamsData.dsv");

                    foreach (var item in profExams)
                    {
                        int id = -1;
                        if (item!="")
                            id = int.Parse(item);
                        Exam e = exams.FirstOrDefault(x => x.ExamId == id);
                        if (e != null)
                            professor.Exams.Add(e);

                    }

                    
                }

                professors.Add(professor);
            }
            sr.Close();
            stream.Close();

            return professors;
        }


        public static void SaveProfessor(Professor professor)
        {
            string userToString = "";
            userToString = professor.UserName + "|" + professor.Password + "|" + professor.Name + "|" + professor.LastName + "|" + professor.Email + "|" + professor.Birthday.ToString("dd/MM/yyyy")  + "[";

            for (int i = 0; i < professor.Subjects.Count(); i++)
            {
                userToString += professor.Subjects[i] + "|";
            }
            userToString += "[";

            for (int i = 0; i < professor.Exams.Count(); i++)
            {
                userToString += professor.Exams[i].ExamId + "|";
            }

            string path = HostingEnvironment.MapPath("~/App_Data/ProfessorsData.dsv");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(userToString);
            }
        }

        public static void SaveProfessors(List<Professor> professors)
        {
            var path = HostingEnvironment.MapPath("~/App_Data/ProfessorsData.dsv");
            FileStream stream = new FileStream(path, FileMode.Truncate, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (var professor in professors)
                {
                    string userToString = "";
                    userToString = professor.UserName + "|" + professor.Password + "|" + professor.Name + "|" + professor.LastName + "|" + professor.Email + "|" + professor.Birthday.ToString("dd/MM/yyyy") + "[";

                    for (int i = 0; i < professor.Subjects.Count(); i++)
                    {
                        userToString += professor.Subjects[i] + "|";
                    }
                    userToString += "[";

                    for (int i = 0; i < professor.Exams.Count(); i++)
                    {
                        userToString += professor.Exams[i].ExamId + "|";
                    }
                    sw.WriteLine(userToString);
                }
            }

        }

    }
}
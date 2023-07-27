using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Projekat.Filters;
using System.Security.Claims;
using Projekat.Models;

namespace Projekat.Controllers
{
    public class ProfessorsController : ApiController
    {
        [JwtAuthentication]
        [HttpGet]
        [Route("api/professors/GetExamResults")]
        public IHttpActionResult GetExamResults()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "professor")
                {
                    List<ExamResults> profExamRes = new List<ExamResults>();
                    List<Exam> exs = DataLists.ExamList.FindAll(x => x.Professor.Equals(u.UserName));
                    List<Exam> examList = new List<Exam>();
                    foreach (var item in exs)
                    {
                        List<ExamResults> ex = DataLists.ExamResultList.FindAll(x => x.Exam == item.ExamId);
                        if(ex!=null && ex.Count() != 0) {
                            profExamRes.AddRange(ex);
                            for (int i = 0; i < profExamRes.Count(); i++)
                            {
                                examList.Add(new Exam(item.ExamId, item.Professor, item.Subject, item.Classroom, item.ExamPeriod, item.DateOfExam));

                            }
                        }
                            
                    }
                    return Ok(new Tuple<List<ExamResults>, List<Exam>>(profExamRes, examList));
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/professors")]
        public IHttpActionResult Post(Exam exam)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "professor")
                {
                    if (exam == null)
                    {
                        return BadRequest("Sva polja moraju biti popunjena");
                    }
                    if (exam.Classroom == null || exam.Classroom.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biti popunjena");
                    }
                    if (exam.ExamPeriod == null || exam.ExamPeriod.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biti popunjena");
                    }
                    if (exam.Subject == null || exam.Subject.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biti popunjena");
                    }
                    if (exam.DateOfExam == null || exam.DateOfExam < DateTime.Now)
                    {
                        return BadRequest("Datum mora biti stariji od danasnjeg.");
                    }

                    Professor professor = DataLists.ProfessorList.Find(x => x.UserName.Equals(u.UserName));
                    foreach (var item in professor.Subjects)
                    {
                        if (item.Equals(exam.Subject.ToUpper()))
                        {
                            if (DataLists.ExamList.Find(x => x.ExamPeriod.ToLower().Equals(exam.ExamPeriod.ToLower()) && x.Professor.Equals(professor.UserName) && x.Subject.ToLower().Equals(exam.Subject.ToLower())) == null)
                            {
                                exam.Professor = professor.UserName;
                                exam.Subject = exam.Subject.ToUpper();
                                exam.ExamPeriod = exam.ExamPeriod.ToUpper();
                                exam.ExamId = DataLists.ExamList.Count;
                                Data.SaveExam(exam);
                                DataLists.ExamList.Add(exam);
                                professor.Exams.Add(exam);
                                Data.SaveProfessors(DataLists.ProfessorList);
                                return Ok("Uspesno ste kreirali ispit");
                            }
                            else
                                return BadRequest("Ispit za dati rok je vec kreiran");
                        }
                    }
                    return Conflict();
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/professors/GetExamsToGrade")]
        public IHttpActionResult GetExamsToGrade()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "professor")
                {
                    List<Exam> registeredExams = new List<Exam>();
                    List<string> registeredStudents = new List<string>();

                    foreach (var student in DataLists.StudentList)
                    {
                        if (student.RegisterdExam != null && !student.Deleted)
                        {
                            foreach (var exam in student.RegisterdExam)
                            {
                                registeredExams.Add(exam);
                                registeredStudents.Add(student.UserName);
                            }
                        }
                    }

                    Professor prof = DataLists.ProfessorList.Find(x => x.UserName.Equals(u.UserName));
                    List<Exam> retExams = new List<Exam>();
                    List<string> retStudents = new List<string>();

                    foreach (var exam in prof.Exams)
                    {
                        for (int i = 0; i < registeredExams.Count(); i++)
                        {
                            if(exam.ExamId == registeredExams[i].ExamId)
                            {
                                retExams.Add(exam);
                                retStudents.Add(registeredStudents[i]);
                            }
                        }
                    }

                    
                    return Ok(new Tuple<List<string>, List<Exam>>(retStudents, retExams));
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/professors/GradeExam")]
        public IHttpActionResult GradeExam(ExamResults results)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "professor")
                {
                    if (results == null)
                    {
                        return BadRequest("Sva polja moraju biti popunjena");
                    }
                    if (results.Student == null || results.Student.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biti popunjena!");
                    }
                    if (results.Grade <5 || results.Grade>10)
                    {
                        return BadRequest("Ocena mora biti u opsegu 5 i 10!");
                    }
                    
                    Professor professor = DataLists.ProfessorList.Find(x => x.UserName.Equals(u.UserName));
                    ExamResults examRes = DataLists.ExamResultList.Find(x => x.Exam == results.Exam && x.Student.Equals(results.Student));

                    Exam prof = professor.Exams.Find(x => x.ExamId == results.Exam);
                    if (prof == null)
                        return BadRequest("Niste nadlezni za dati predmet");
                    if (examRes != null)
                        return Conflict();

                    
                    DataLists.ExamResultList.Add(results);
                    Data.SaveExamResult(results);
                    Student student = DataLists.StudentList.Find(x => x.UserName.Equals(results.Student));
                    student.RegisterdExam.Remove(prof);
                    if (results.Grade == 5)
                        student.FailedExam.Add(prof);
                    else
                        student.PassedExam.Add(prof);
                    Data.SaveStudents(DataLists.StudentList);
                    return Ok("Uspesno ste ocenili ispit!");
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }
    }
}

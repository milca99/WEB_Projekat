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
    public class StudentsController : ApiController
    {
        [JwtAuthentication]
        [Route("api/students/")]
        public IHttpActionResult Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "student")
                {
                    List<Exam> fc = DataLists.ExamList.FindAll(x => x.DateOfExam > DateTime.Now);
                    return Ok(fc);
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpPut]
        [Route("api/students/RegisterExam/{id}")]
        public IHttpActionResult RegisterExam([FromUri] int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "student")
                {
                    Exam regEx = DataLists.ExamList.Find(x => x.ExamId == id);
                    if (regEx.DateOfExam < DateTime.Now)
                        return BadRequest("Ispit je vec zavrsen!");
                    Student st = DataLists.StudentList.Find(x => x.UserName == u.UserName);
                    Exam ex = new Exam();
                    Exam passed = new Exam();
                    Exam failed = new Exam();
                    if (st.RegisterdExam!=null)
                        ex = st.RegisterdExam.Find(x => x.ExamId == id);
                    if (st.PassedExam != null)
                        passed = st.PassedExam.Find(x => x.ExamId == id);
                    if (st.FailedExam != null)
                        failed = st.FailedExam.Find(x => x.ExamId == id);
                    if (ex == null && passed==null && failed==null)
                        st.RegisterdExam.Add(regEx);
                    else
                        return Conflict();
                    Data.SaveStudents(DataLists.StudentList);

                    return Ok("Uspesno prijavljen ispit");
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/students/GetExamResults")]
        public IHttpActionResult GetExamResults()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "student")
                {
                    List<ExamResults> fc = DataLists.ExamResultList.FindAll(x => x.Student.Equals(u.UserName));
                    List<Exam> exs = new List<Exam>();
                    foreach (var item in fc)
                    {
                        Exam ex = DataLists.ExamList.Find(x => x.ExamId == item.Exam);
                        if(ex!=null)
                            exs.Add(ex);
                    }
                    return Ok(new Tuple<List<ExamResults>, List<Exam>>(fc,exs));
                }
            }
            return Unauthorized();
        }
    }
}

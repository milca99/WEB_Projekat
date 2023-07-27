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
    public class AdminsController : ApiController
    {
        [JwtAuthentication]
        [HttpGet]
        [Route("api/admins/GetStudents")]
        public IHttpActionResult GetStudents()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "admin")
                {
                    List<Student> students = DataLists.StudentList.FindAll(x => x.Deleted.Equals(false));
                    
                    return Ok(students);
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpDelete]
        [Route("api/admins/DeleteStudent/{username}")]
        public IHttpActionResult DeleteStudent(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "admin")
                {
                    Student student = DataLists.StudentList.Find(x => x.UserName.Equals(username));
                    if (student == null)
                        return BadRequest("Student ne postoji.");
                    if (student.Deleted)
                        return BadRequest("Student je vec obrisan");
                    student.Deleted = true;
                    Data.SaveStudents(DataLists.StudentList);

                    return Ok("uspesno izbrisan student");
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpPut]
        [Route("api/admins/ModifyStudent")]
        public IHttpActionResult ModifyStudent(Student student)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "admin")
                {
                    if (student == null)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.UserName == null || student.UserName.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Password == null || student.Password.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Name == null || student.Name.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.LastName == null || student.LastName.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Email == null || student.Email.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Birthday == null || student.Birthday > DateTime.Now)
                    {
                        return BadRequest("Datum rodjenja mora biti stariji.");
                    }

                    Student stu = DataLists.StudentList.Find(x => x.UserName.Equals(student.UserName));
                    Student s = DataLists.StudentList.Find(x => x.Email.Equals(student.Email) && !(x.UserName.Equals(stu.UserName)));
                    Professor pr = DataLists.ProfessorList.Find(x => x.Email.Equals(student.Email));
                    if (s != null || pr != null)
                        return Conflict();

                    stu.Password = student.Password;
                    stu.Name = student.Name;
                    stu.LastName = student.LastName;
                    stu.Birthday = student.Birthday;
                    stu.Email = student.Email;

                    Data.SaveStudents(DataLists.StudentList);
                    return Ok("Uspesno ste modifikovali studenta");
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/admins/SaveStudent")]
        public IHttpActionResult SaveStudent(Student student)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                User u = JwtManager.GetUserFromIdentity(identity);
                if (u.Role == "admin")
                {
                    if (student == null)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.UserName == null || student.UserName.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Password == null || student.Password.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Name == null || student.Name.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.LastName == null || student.LastName.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Email == null || student.Email.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }
                    if (student.Birthday == null || student.Birthday > DateTime.Now)
                    {
                        return BadRequest("Datum rodjenja mora biti stariji.");
                    }
                    if (student.IndexNumber == null || student.IndexNumber.Length == 0)
                    {
                        return BadRequest("Sva polja moraju biiti popunjena!");
                    }

                    Student username = DataLists.StudentList.Find(x => x.UserName.Equals(student.UserName));
                    if (username != null)
                        return BadRequest("Korisnicko ime vec postoji!");
                    Student email = DataLists.StudentList.Find(x => x.Email.Equals(student.Email));
                    if (email != null)
                        return BadRequest("Email vec postoji!");
                    Student index = DataLists.StudentList.Find(x => x.IndexNumber.Equals(student.IndexNumber));
                    if (index != null)
                        return BadRequest("Index vec postoji!");

                    Professor userProf = DataLists.ProfessorList.Find(x => x.UserName.Equals(student.UserName));
                    if (userProf != null)
                        return BadRequest("Korisnicko ime vec postoji!");
                    Professor emailProf = DataLists.ProfessorList.Find(x => x.Email.Equals(student.Email));
                    if (emailProf != null)
                        return BadRequest("Email vec postoji!");

                    Administrator userAdm = DataLists.AdministratorList.Find(x => x.UserName.Equals(student.UserName));
                    if (userAdm != null)
                        return BadRequest("Korisnicko ime vec postoji!");

                    DataLists.StudentList.Add(student);
                    Data.SaveStudent(student);
                    return Ok("Uspesno ste dodali studenta");
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

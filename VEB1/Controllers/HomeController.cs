using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Projekat.Models.Parameters;
using Projekat.Models;

namespace Projekat.Controllers
{
    public class HomeController : ApiController
    {
        [AllowAnonymous]
        [HttpGet, Route("")]
        public RedirectResult Index()
        {
            var requestUri = Request.RequestUri;
            return Redirect(requestUri.AbsoluteUri + "index.html");
        }

        [AllowAnonymous]
        [Route("api/home/LogUser")]
        [HttpPost]
        public IHttpActionResult LogUser(LogParameters logParam)
        {
            Student student = DataLists.StudentList.Find(x => (x.UserName.Equals(logParam.Username) && x.Password.Equals(logParam.Password) && x.Deleted.Equals(false)));
            Professor professor = new Professor();
            Administrator admin = new Administrator();

            if(student==null)
                professor = DataLists.ProfessorList.Find(x => (x.UserName.Equals(logParam.Username) && x.Password.Equals(logParam.Password)));
            if(professor==null)
                admin = DataLists.AdministratorList.Find(x => (x.UserName.Equals(logParam.Username) && x.Password.Equals(logParam.Password)));
            if (student != null)
            {
                User user = new User(student.UserName, student.Password, student.Name, student.LastName, student.Email, student.Birthday, "student");
                return Ok(JwtManager.GenerateToken(user));
            }
            else if(professor != null)
            {
                User user = new User(professor.UserName, professor.Password, professor.Name, professor.LastName, professor.Email, professor.Birthday, "professor");
                return Ok(JwtManager.GenerateToken(user));
            }
            else if(admin != null)
            {
                User user = new User(admin.UserName, admin.Password, admin.Name, admin.LastName, "", admin.Birthday, "admin");
                return Ok(JwtManager.GenerateToken(user));
            }
            else
            {
                return NotFound();
            }
        }
    }
}

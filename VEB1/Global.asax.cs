using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Projekat.Models;

namespace Projekat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataLists.AdministratorList = Data.ReadAdministrators("~/App_Data/AdministratorsData.dsv");
            DataLists.ExamList = Data.ReadExams("~/App_Data/ExamsData.dsv");
            DataLists.ExamResultList = Data.ReadExamResults("~/App_Data/ExamResultsData.dsv");
            DataLists.ProfessorList = Data.ReadProfessors("~/App_Data/ProfessorsData.dsv");
            DataLists.StudentList = Data.ReadStudents("~/App_Data/StudentsData.dsv");
        }
    }
}

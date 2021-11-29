using CyberSecurity3WebApplication.DataModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberSecurity3WebApplication.Controllers
{
    public class SchoolController : Controller
    {
        private SchoolContext schoolContext;

        public SchoolController(SchoolContext schoolContext)
        {
            this.schoolContext = schoolContext;
        }

        public IActionResult Index()
        {
            var students = schoolContext.Students.ToList();

            // schoolContext.Students.Add(new Student { LastName = "x", FirstMidName = "y" });
            // int cnt = schoolContext.SaveChanges();

            return View();
        }
    }
}

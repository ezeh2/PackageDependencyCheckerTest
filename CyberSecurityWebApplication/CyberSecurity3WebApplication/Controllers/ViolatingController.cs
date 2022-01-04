using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberSecurity3WebApplication.Controllers
{
    /// <summary>
    /// Returns pages to browser, which violates for demo purposes the CSP defined in Startup.cs
    /// </summary>
    public class ViolatingController : Controller
    {
        public IActionResult InlineJavaScript()
        {
            return View();
        }

        public IActionResult JavaScriptFromInternet()
        {
            return View();
        }

        public IActionResult InlineCss()
        {
            return View();
        }

        public IActionResult ImageFromInternet()
        {
            return View();
        }

        public IActionResult FormSubmitToInternet()
        {
            return View();
        }

        public IActionResult BaseTagToInternet()
        {
            return View();
        }
    }
}

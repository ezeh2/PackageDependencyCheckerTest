using CyberSecurity3WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurity3WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// https://localhost:44365/home/example10?command=cmd.exe&arguments=/c%20type%20c:\temp\pal_module_tester_series2_assert_exceptions.log
        /// </summary>
        /// <returns></returns>
        public IActionResult Example10()
        {
            var context = base.HttpContext;

            HttpRequest req = context.Request;
            string queryString = req.QueryString.Value;
            string command = req.Query["command"].FirstOrDefault();
            string arguments = req.Query["arguments"].FirstOrDefault();
            if (command != null)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    StreamReader reader = process.StandardOutput;
                    string output = reader.ReadToEnd();
                    StreamReader errorReader = process.StandardError;
                    string error = errorReader.ReadToEnd();

                    process.WaitForExit();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(output);
                    sb.AppendLine(error);

                    return Content(sb.ToString());
                }
            }
            else
            {
                return Content("command parameter is missing");
            }

            return View();
        }

        /// <summary>
        /// use with https://localhost:44365/home/example20?path=c:\temp\pal_module_tester_series2_assert_exceptions.log%20%26%26%20c:\windows\notepad.exe
        /// </summary>
        /// <returns></returns>
        public IActionResult Example20()
        {
            var context = base.HttpContext;

            HttpRequest req = context.Request;
            string queryString = req.QueryString.Value;
            string path = req.Query["path"].FirstOrDefault();
            if (path != null)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";

                    StringBuilder argumentsSb = new StringBuilder();
                    argumentsSb.Append(" /c type ");
                    argumentsSb.Append(path);
                    process.StartInfo.Arguments = argumentsSb.ToString();


                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    StreamReader reader = process.StandardOutput;
                    string output = reader.ReadToEnd();
                    StreamReader errorReader = process.StandardError;
                    string error = errorReader.ReadToEnd();

                    process.WaitForExit();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(output);
                    sb.AppendLine(error);

                    return Content(sb.ToString());
                }
            }
            else
            {
                return Content("command parameter is missing");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

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
using System.Text.RegularExpressions;
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
            HttpRequest req = base.HttpContext.Request;
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
            HttpRequest req = base.HttpContext.Request;
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

        /// <summary>
        /// shows content of file "c:\temp\pal_module_tester_series2_assert_exceptions.log" in browser
        /// https://localhost:44365/home/example40?path=c:\temp\pal_module_tester_series2_assert_exceptions.log
        /// 
        /// path contains invalid characters:
        /// https://localhost:44365/home/example40?path=c:\temp\pal_module_tester_series2_assert_exceptions.log%20%26%26%20c:\windows\notepad.exe
        /// 
        /// path points to a forbidden location
        /// https://localhost:44365/home/example40?path=c:\tempxy\bla.txt
        /// 
        /// path does not represent a valid file
        /// https://localhost:44365/home/example40?path=c:\temp\pal_module_tester_series2_assert_exceptions_2_.log
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Example40()
        {
            HttpRequest req = base.HttpContext.Request;
            string path = req.Query["path"].FirstOrDefault();

            if (path != null)
            {
                path = SanitizePath(path);

                if (!IsValidPath(path))
                {
                    return Problem(detail: "path contains invalid characters", statusCode: 400);
                }
                else if (!path.StartsWith("c:\\temp"))
                {
                    return Problem(detail: "path points to a forbidden location", statusCode: 403);
                }
                else if (!System.IO.File.Exists(path))
                {
                    return Problem(detail: "path does not represent a valid file", statusCode: 404);
                }

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
        }

        /// <summary>
        /// https://exceptionshub.com/c-sanitize-file-name.html
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool IsValidPath(string path)
        {
            Match match = Regex.Match(path, "([0-9\\-A-Za-z:\\\\_\\.]+)");
            bool isValid = match.Success && match.Value == path;
            return isValid;
        }

        private static string SanitizePath(string path)
        {
            StringBuilder sb = new StringBuilder();
            bool stop = false;
            for(int i=0;i<path.Length && !stop;i++)
            {
                string substring = path.Substring(i, 1);
                // valid char ?
                if (Regex.IsMatch(substring, "[0-9\\-A-Za-z:\\\\_\\.]"))
                {
                    // yes, append
                    sb.Append(substring);
                }
                else
                {
                    // stop loop
                    stop = true;
                }
            }

            return sb.ToString();
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

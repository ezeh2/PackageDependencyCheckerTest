using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CyberSecurity3WebApplication.Controllers
{
    public class SearchController : Controller
    {
        /// <summary>
        /// Reflected XSS
        /// 
        /// https://localhost:44365/search/Example210?searchterm=mueller
        /// https://localhost:44365/search/Example210?searchterm=%3Cscript%3Ealert(%27x%27);%3C/script%3E
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IActionResult Example210(string searchTerm)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<HTML><BODY>");
            sb.Append("Search Results for: ");
            sb.Append(searchTerm);
            sb.Append(searchTerm);
            sb.Append("</BODY></HTML>");

            ContentResult cr = new ContentResult();
            cr.ContentType = "text/html; charset=utf-8";
            cr.Content = sb.ToString();

            return cr;
        }

        /// <summary>
        /// mitigate reflected XSS: encode html
        /// 
        /// https://localhost:44365/search/Example220?searchterm=mueller
        /// https://localhost:44365/search/Example220?searchterm=%3Cscript%3Ealert(%27x%27);%3C/script%3E
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IActionResult Example220(string searchTerm)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<HTML><BODY>");
            sb.Append("Search Results for: ");
            sb.Append(HttpUtility.HtmlEncode(searchTerm));
            sb.Append("</BODY></HTML>");

            ContentResult cr = new ContentResult();
            cr.ContentType = "text/html; charset=utf-8";
            cr.Content = sb.ToString();

            return cr;
        }

        /// <summary>
        /// mitigate reflected XSS: sanitize
        /// 
        /// https://localhost:44365/search/Example230?searchterm=mueller
        /// https://localhost:44365/search/Example230?searchterm=mueller%3Cscript%3Ealert(%27x%27);%3C/script%3E
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IActionResult Example230(string searchTerm)
        {
            HtmlSanitizer htmlSanitizer = new HtmlSanitizer();
            searchTerm = htmlSanitizer.Sanitize(searchTerm);

            StringBuilder sb = new StringBuilder();
            sb.Append("<HTML><BODY>");
            sb.Append("Search Results for: ");
            sb.Append(searchTerm);
            sb.Append("</BODY></HTML>");

            ContentResult cr = new ContentResult();
            cr.ContentType = "text/html; charset=utf-8";
            cr.Content = sb.ToString();

            return cr;
        }

        /// <summary>
        /// mitigate reflected XSS: use razor
        /// 
        /// https://localhost:44365/search/Example240?searchterm=mueller%3Cscript%3Ealert(%27x%27);%3C/script%3E
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IActionResult Example240(string searchTerm)
        {
            base.ViewData["searchTerm"] = searchTerm;
            return View();
        }
    }
}

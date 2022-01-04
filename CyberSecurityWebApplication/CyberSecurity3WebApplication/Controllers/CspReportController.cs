using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CyberSecurity3WebApplication.Controllers
{
    /// <summary>
    /// This Controller is 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CspReportController : ControllerBase
    {
        private readonly ILogger _logger;

        public CspReportController(ILogger<CspReportController> _logger)
        {
            this._logger = _logger;
        }

        public async void Post()
        {
            string message = null;

            using (StreamReader sr = new StreamReader(this.Request.Body))
            {
                message = await sr.ReadToEndAsync();

                string indentedMessage = IndentMessage(message);

                this._logger.LogError("###");
                this._logger.LogError(indentedMessage);
                this._logger.LogError("###");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string IndentMessage(string message)
        {
            JObject jobject = JObject.Parse(message);

            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.Formatting = Formatting.Indented;
            JsonSerializer js = JsonSerializer.Create(jss);
            StringWriter sw = new StringWriter();
            js.Serialize(sw, jobject);

            sw.Flush();
            string indentedMessage = sw.ToString();
            return indentedMessage;
        }
    }
}

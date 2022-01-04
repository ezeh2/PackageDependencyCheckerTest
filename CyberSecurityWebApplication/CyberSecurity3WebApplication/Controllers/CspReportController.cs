using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                this._logger.LogError(message);
            }
        }
    }
}

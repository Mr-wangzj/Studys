using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIfirst.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;
        public LogsController(ILogger<LogsController> logger)
        {
            _logger= logger;
        }
        [HttpPost]
        public string infologs(string msg) {
            _logger.LogInformation(msg);
            return msg;
        }
        [HttpPost]
        public string errorlogs(string msg)
        {
            _logger.LogError(msg);
            return msg;
        }
        [HttpPost]
        public string debuglogs(string msg)
        {
            _logger.LogDebug(msg);
            return msg;
        }
        [HttpPost]
        public string warnlogs(string msg)
        {
            _logger.LogWarning(msg);
            return msg;
        }
    }
}

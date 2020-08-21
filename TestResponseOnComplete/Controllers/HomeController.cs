using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestResponseOnComplete.Models;

namespace TestResponseOnComplete.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation($"Index called");
                _logger.LogInformation($"in try before sleep{DateTimeOffset.Now.ToString()}");
                Thread.Sleep(2000);
                _logger.LogInformation($"in try after sleep{DateTimeOffset.Now.ToString()}");

                return View();
            }
            finally
            {
                _logger.LogInformation($"Return done, first line in finally");
                Response.OnCompleted(async () =>
                {
                    _logger.LogInformation($"In complete callback, going to sleep");
                    await Task.Run(() => Thread.Sleep(7000));
                    _logger.LogInformation($"sleep done");
                });
            }
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

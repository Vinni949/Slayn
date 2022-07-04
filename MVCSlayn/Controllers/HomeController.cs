using Microsoft.AspNetCore.Mvc;
using MVCSlayn.Models;
using System.Diagnostics;

namespace MVCSlayn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CounterPartuClassContext context;
        public HomeController(ILogger<HomeController> logger, CounterPartuClassContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            List<OrderClass> orders = new List<OrderClass>();
            orders = context.OrderClass.ToList();
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
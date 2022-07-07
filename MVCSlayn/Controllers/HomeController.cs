using Microsoft.AspNetCore.Mvc;
using MVCSlayn.Models;
using System.Diagnostics;
using Models1.Model;

namespace MVCSlayn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBSlaynTest dBSlaynTest;
        public HomeController(ILogger<HomeController> logger, DBSlaynTest dBSlaynTest)
        {
            _logger = logger;
            this.dBSlaynTest = dBSlaynTest;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string login)
        {
            var counterParties = dBSlaynTest.counterPartyClass.SingleOrDefault(p=>p.Name==login);
            //foreach(var counter in dBSlaynTest.counterPartyClass)
            //{
            //    if(counter.Name==login)
            //    {
            //       var counterParties = counter;
            //        return Content(counterParties.Name+counterParties.PriceType);
            //    }
            //}
            if (counterParties != null)
                return View(counterParties);
            else
                return View("Введены не верные данные.");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Orders()
        {
            List<OrderClass> orders = new List<OrderClass>();
            orders = dBSlaynTest.orderClass.ToList();
            return View(orders);
        }
        public IActionResult Privacy()
        {
            List<PositionClass> positions = new List<PositionClass>();
            positions = dBSlaynTest.positionClass.ToList();
            return View(positions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
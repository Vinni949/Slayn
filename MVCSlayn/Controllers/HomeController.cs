using Microsoft.AspNetCore.Mvc;
using MVCSlayn.Models;
using System.Diagnostics;
using Models1.Model;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string login,string password)
        {
            var counterParty = dBSlaynTest.counterPartyClass.Include(p=>p.counterPartyOrders).SingleOrDefault(p=>p.Name==login);
            //foreach(var counter in dBSlaynTest.counterPartyClass)
            //{
            //    if(counter.Name==login)
            //    {
            //       var counterParties = counter;
            //        return Content(counterParties.Name+counterParties.PriceType);
            //    }
            //}
            if (counterParty != null)
                return RedirectToAction(nameof(Index),counterParty);
            else
                return View("Введены не верные данные.");
        }
        public IActionResult Index(CounterPartyClass counterParty)
        {
            ViewData["ordersCount"] = counterParty.counterPartyOrders.Count;

            return View(counterParty);
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
        public void AddToBasket(string id)
        {
            if (ViewData.ContainsKey("Basket"))
                ((List<string>)ViewData["Basket"]).Add(id);
            else
            {
                ViewData["Basket"] = new List<String>();
                ((List<string>)ViewData["Basket"]).Add(id);
            }
        }
        public IActionResult Basket()
        {
            return View(ViewData["Basket"]);
        }

        [ResponseCache(Duration = 0 , Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
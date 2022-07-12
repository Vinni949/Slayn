using Microsoft.AspNetCore.Mvc;
using MVCSlayn.Models;
using System.Diagnostics;
using Models1.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            
            var counterParty = dBSlaynTest.counterPartyClass.Include(p=>p.counterPartyOrders).SingleOrDefault(p=>p.LoginOfAccessToTheLC==loginViewModel.Login
            &&p.LoginOfPsswordToTheLC==loginViewModel.Password);

            if (counterParty != null)
            {
                await AuthAsync(loginViewModel.Login);
                return RedirectToAction(nameof(Index), counterParty);
            }
            else
                return View(loginViewModel);
        }
        [Authorize]
        public IActionResult Index(CounterPartyClass counterParty)
        {
            ViewData["ordersCount"] = counterParty.counterPartyOrders.Count;

            return View(counterParty);
        }
        [Authorize]
        public IActionResult Orders(int? page)
        {
            int pageSize = 5;
            page = page ?? 0;
            List<OrderClass> orders = new List<OrderClass>();
            orders = dBSlaynTest.orderClass.Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<OrderClass>(page.Value,dBSlaynTest.orderClass.Count(),orders,pageSize));
        }
        [Authorize]
        public IActionResult Privacy()
        {
            List<PositionClass> positions = new List<PositionClass>();
            positions = dBSlaynTest.positionClass.ToList();
            return View(positions);
        }
        [Authorize]
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

        private async Task AuthAsync(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }
    }
}
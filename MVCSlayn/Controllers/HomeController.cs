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
                await AuthAsync(counterParty.Id);
                return RedirectToAction(nameof(Index));
            }
            else
                return View(loginViewModel);
        }
        [Authorize]
        public IActionResult Index()
        {
<<<<<<< HEAD
            string id = User.Identity.Name;
            return View(dBSlaynTest.counterPartyClass.SingleOrDefault(p => p.Id == id));
=======
            ViewData["ordersCount"] = counterParty.counterPartyOrders.Count;

            return View(counterParty);
>>>>>>> parent of a7de78b (Add PositionToOrders)
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
        [HttpPost]
        public IActionResult AddToBasket(string id,int page)
        {
            string counterPartyId = User.Identity.Name;
            var basketUser = dBSlaynTest.userBaskets.SingleOrDefault(p => p.CounterPartyId == counterPartyId && p.PositionId == id);
            if (basketUser != null)
            {
                basketUser.Count++;
            }
            else
            {
                UserBasket product = new UserBasket();
                product.PositionId = id;
                product.CounterPartyId = counterPartyId;
                product.Count = 1;
                dBSlaynTest.userBaskets.Add(product);
            }
            dBSlaynTest.SaveChanges();

            return RedirectToAction(nameof(Privacy),page);
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

        private async Task AuthAsync(string userId)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }
    }
}
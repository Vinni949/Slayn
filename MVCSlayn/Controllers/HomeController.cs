﻿using Microsoft.AspNetCore.Mvc;
using MVCSlayn.Models;
using System.Diagnostics;
using Models1.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Confiti.MoySklad.Remap.Entities;
using Confiti.MoySklad.Remap.Client;
using Confiti.MoySklad.Remap.Api;
using Confiti.MoySklad.Remap.Models;
using System.Net.Mime;

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
            string id = User.Identity.Name;
            var count = dBSlaynTest.counterPartyClass.Include(p => p.counterPartyOrders).SingleOrDefault(p => p.Id == User.Identity.Name);
            int counorder = count.counterPartyOrders.Count;
            return View(dBSlaynTest.counterPartyClass.SingleOrDefault(p => p.Id == id));
        }
        [Authorize]
        public IActionResult Orders(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<OrderClass> orders = new List<OrderClass>();
            orders = dBSlaynTest.orderClass.Include(p=>p.positions).Where(p=>p.CounterPartyClassId==User.Identity.Name).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<OrderClass>(page.Value,dBSlaynTest.orderClass.Count(),orders,pageSize));
        }
        [Authorize]
        public IActionResult Privacy(int? page, string searchString = null)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<AssortmentClass> assortments = new List<AssortmentClass>();
            var conter = dBSlaynTest.counterPartyClass.SingleOrDefault(p => p.Id == User.Identity.Name);
            assortments = dBSlaynTest.assortmentClass.Include(p=>p.PriceTypes).Skip(pageSize * page.Value).Take(pageSize).ToList();
            for(int a=0; a<assortments.Count;a++)
            {
                var price= assortments[a].PriceTypes.SingleOrDefault(p => p.name == conter.PriceType);
                if (price != null && price.price != 0)
                {

                    assortments[a].price = price.price;
                    dBSlaynTest.SaveChanges();
                }
                else
                    assortments[a].price = assortments[a].PriceTypes[0].price;
                dBSlaynTest.SaveChanges();
            }
            return View(new PagedList<AssortmentClass>(page.Value, dBSlaynTest.assortmentClass.Count(), assortments, pageSize));
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddToBasket(string id,int currentPage)
        {
            string counterPartyId = User.Identity.Name;
            var basketUser = dBSlaynTest.userBaskets.SingleOrDefault(p => p.CounterPartyId == counterPartyId && p.AssortmentId == id);
            if (basketUser != null)
            {
                basketUser.Count++;
            }
            else
            {
                UserBasket product = new UserBasket();
                product.AssortmentId = id;
                product.CounterPartyId = counterPartyId;
                product.Count = 1;
                product.Price = dBSlaynTest.assortmentClass.SingleOrDefault(p => p.Id == id).price;
                dBSlaynTest.userBaskets.Add(product);
            }
            dBSlaynTest.SaveChanges();

            return RedirectToAction(nameof(Privacy), currentPage);
        }
        public IActionResult Basket(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            var basketPositions = from b in dBSlaynTest.userBaskets
                                  join p in dBSlaynTest.assortmentClass
                                  on b.AssortmentId equals p.Id
                                  where b.CounterPartyId == User.Identity.Name
                                  select new BasketViewModel(b.Count, p.Name);
                               
            return View(basketPositions);
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
        /// <summary>
        /// Создание заказа
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var credentials = new MoySkladCredentials()
            {
                Username = "aldef@slayn",
                Password = "12345678",
            };
            var httpClient = new HttpClient();
            var api = new MoySkladApi(credentials, httpClient);
            var sklad = await api.Organization.GetAllAsync();
            var query = new ApiParameterBuilder<CounterpartiesQuery>();
            query.Parameter("id").Should().Be(User.Identity.Name);
            var counter = await api.Counterparty.GetAllAsync(query);
            var basketPositions = dBSlaynTest.userBaskets.Where(p => p.CounterPartyId == User.Identity.Name).ToList();
            var newOrder = new CustomerOrder() { Agent = counter.Payload.Rows[0], Organization = sklad.Payload.Rows[2], 
                Positions = new PagedMetaEntities<CustomerOrderPosition> {
                    Rows=new CustomerOrderPosition[basketPositions.Count()]                    
            } };
            for(int i=0;i< newOrder.Positions.Rows.Length; i++)
            {

                newOrder.Positions.Rows[i] = new CustomerOrderPosition
                {
                    Quantity = basketPositions[i].Count,
                    Price = (long)basketPositions[i].Price*100,
                    Assortment = new Product
                    {
                        Meta = new Meta
                        {
                            Href = "https://online.moysklad.ru/api/remap/1.2/entity/product/" + basketPositions[i].AssortmentId,
                            MetadataHref = "https://online.moysklad.ru/api/remap/1.2/entity/product/metadata",
                            Type = EntityType.Product,
                            MediaType = MediaTypeNames.Application.Json
                        }
                    }
                };
            }
            await api.CustomerOrder.CreateAsync(newOrder);
            foreach(var positions in dBSlaynTest.userBaskets)
            {
                dBSlaynTest.userBaskets.Remove(positions);
                
            }
            dBSlaynTest.SaveChanges();
            return RedirectToAction(nameof(Privacy));
        }
    }
    
}
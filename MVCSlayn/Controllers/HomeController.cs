using Microsoft.AspNetCore.Mvc;
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
        private readonly DBSlayn DBSlayn;
        public HomeController(ILogger<HomeController> logger, DBSlayn DBSlayn)
        {
            _logger = logger;
            this.DBSlayn = DBSlayn;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            
            var counterParty = DBSlayn.counterPartyClass.Include(p=>p.counterPartyOrders).SingleOrDefault(p=>p.LoginOfAccessToTheLC==loginViewModel.Login
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
            var count = DBSlayn.counterPartyClass.Include(p => p.counterPartyOrders).SingleOrDefault(p => p.Id == User.Identity.Name);
            int counorder = count.counterPartyOrders.Count;
            return View(DBSlayn.counterPartyClass.SingleOrDefault(p => p.Id == id));
        }
        [Authorize]
        public IActionResult Orders(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<OrderClass> orders = new List<OrderClass>();
            orders = DBSlayn.orderClass.Include(p=>p.positions).Where(p=>p.CounterPartyClassId==User.Identity.Name).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<OrderClass>(page.Value,DBSlayn.orderClass.Count(),orders,pageSize));
        }
        [Authorize]
        public IActionResult Return(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<SalesReturnClass> salesReturns = new List<SalesReturnClass>();
            salesReturns = DBSlayn.salesReturnClass.Include(p => p.SalesReturnPositions).Where(p => p.CounterPartiesId == User.Identity.Name).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<SalesReturnClass>(page.Value, DBSlayn.orderClass.Count(), salesReturns, pageSize));
        }
        [Authorize]
        public async Task<IActionResult> Privacy(int? page, string searchString)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<AssortmentClass> assortments = new List<AssortmentClass>();
            var conter = DBSlayn.counterPartyClass.SingleOrDefault(p => p.Id == User.Identity.Name);
            assortments = DBSlayn.assortmentClass.Include(p => p.PriceTypes).ToList();
                        
            if (!String.IsNullOrEmpty(searchString))
            {
                assortments = assortments.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            int assortmetCount = assortments.Count();
            assortments=assortments.Skip(pageSize * page.Value).Take(pageSize).ToList();
            for (int a=0; a<assortments.Count;a++)
            {
                var price= assortments[a].PriceTypes.SingleOrDefault(p => p.name == conter.PriceType);
                if (price != null && price.price != 0)
                {
                    assortments[a].price = price.price;
                    DBSlayn.SaveChanges();
                }
                else
                    assortments[a].price = assortments[a].PriceTypes[0].price;
                DBSlayn.SaveChanges();
            }
            ViewBag.Search = searchString;
            return View(new PagedList<AssortmentClass>(page.Value, assortmetCount, assortments, pageSize));
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddToBasket(string id,int currentPage,string searchString)
        {
            string counterPartyId = User.Identity.Name;
            var basketUser = DBSlayn.userBaskets.SingleOrDefault(p => p.CounterPartyId == counterPartyId && p.AssortmentId == id);
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
                product.Price = DBSlayn.assortmentClass.SingleOrDefault(p => p.Id == id).price;
                DBSlayn.userBaskets.Add(product);
            }
            DBSlayn.SaveChanges();

            return RedirectToAction(nameof(Privacy), new { page = currentPage, searchString = searchString });
        }
        [HttpPost]

        public async Task<IActionResult> AddTask(string posId, string id)
        {
            return RedirectToAction(nameof(Task),posId,id);
        }

            [HttpPost]
        public async Task<IActionResult> Task(string posId, string id,string massage,string post)
        {
            var credentials = new MoySkladCredentials()
            {
                Username = "aldef@slayn",
                Password = "12345678",
            };
            var httpClient = new HttpClient();
            var api = new MoySkladApi(credentials, httpClient);
            TaskEntity task = new TaskEntity()
            {
                Assignee = new Employee
                {
                    Meta = new Meta
                    {
                        Href = "https://online.moysklad.ru/api/remap/1.2/entity/employee/cd721ba3-07aa-11eb-0a80-0904000641db",
                        MetadataHref = "https://online.moysklad.ru/api/remap/1.2/entity/employee/metadata",
                        Type = EntityType.Employee,
                        MediaType = MediaTypeNames.Application.Json
                    }
                },

                Description ="заказ: "+posId+", позиция:  "+id +", Суть рекламации: "+massage+", почта для обратной связи: "+post,
            };
            await api.Task.CreateAsync(task);
            return RedirectToAction(nameof(Orders));
        }
        [HttpPost]
        public async Task<IActionResult> ReturnPositionsOrder(string posId, string id)
        {
            var credentials = new MoySkladCredentials()
            {
                Username = "aldef@slayn",
                Password = "12345678",
            };
            var httpClient = new HttpClient();
            var api = new MoySkladApi(credentials, httpClient);
            var quantyDemand = new ApiParameterBuilder<DemandQuery>();
            quantyDemand.Expand().With(p => p.Positions);
            var demandId = DBSlayn.orderClass.Include(p => p.Demands).SingleOrDefault(p => p.Id == posId);
            foreach (var dem in demandId.Demands)
            {
                var demand = await api.Demand.GetAsync(Guid.Parse(dem.Id), quantyDemand);
               
                foreach (var pos in demand.Payload.Positions.Rows)
                {
                    string[] interPosID = pos.Assortment.Meta.Href.Split("/");
                    if (interPosID[interPosID.Count() - 1] == id)
                    {

                        var salesReturn = new SalesReturn()
                        {
                            Agent = demand.Payload.Agent,
                            Organization = demand.Payload.Organization,
                            Store = demand.Payload.Store,
                            Demand = demand.Payload,
                            
                            Applicable=false,
                            Positions = new PagedMetaEntities<SalesReturnPosition>()
                            {

                                Rows = new[]
                                {
                                    new SalesReturnPosition
                                    {
                                        Quantity=pos.Quantity,
                                        Price=pos.Price,
                                        Assortment=pos.Assortment,
                                        Vat=pos.Vat

                                    }
                                }
                            }


                        };
                        var salesReturnResponse = await api.SalesReturn.CreateAsync(salesReturn);
                    }
                }

            }
            return RedirectToAction(nameof(Orders));
        }
        

        public IActionResult Basket(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            var basketPositions = from b in DBSlayn.userBaskets
                                  join p in DBSlayn.assortmentClass
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
            var basketPositions = DBSlayn.userBaskets.Where(p => p.CounterPartyId == User.Identity.Name).ToList();
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
            foreach(var positions in DBSlayn.userBaskets)
            {
                DBSlayn.userBaskets.Remove(positions);
                
            }
            DBSlayn.SaveChanges();
            return RedirectToAction(nameof(Privacy));
        }
    }
    
}
using Confiti.MoySklad.Remap.Api;
using Confiti.MoySklad.Remap.Client;
using Confiti.MoySklad.Remap.Entities;
using Confiti.MoySklad.Remap.Models;
using Microsoft.EntityFrameworkCore;
using Models1.Model;
using System.Net.Mime;

static class Program
{
    static async Task Main(string[] args)
    {


        var api = GetApiCredentials();
        List<CounterPartyClass> counterParties = new List<CounterPartyClass>();
        List<AssortmentClass> assortment = new List<AssortmentClass>();
        counterParties = await GetApiCounterparties(api, counterParties);
        await GetApiCounterpartiesOrders(api, counterParties);
        await GetApiCounterpartiesOrdersPositions(api);
        await GetApiOrdersDemand(api);
        await GetApiSalesReturn(api);
        await GetApiSalesReturnPositions(api);
        //await GetApiPositions(api, assortment, true);
        //await GetApiPositions(api, assortment, false);



        //var quantyDemand = new ApiParameterBuilder<DemandQuery>();
        //quantyDemand.Expand().With(p => p.Positions);
        //var demand = await api.Demand.GetAsync(Guid.Parse("87f0029e-fe93-11ec-0a80-040b00062846"), quantyDemand);
        //var quantityReturn = new ApiParameterBuilder<SalesReturnQuery>();
        //quantityReturn.Expand().With(p => p.Positions);
        //var returnDemand = await api.SalesReturn.GetAsync(Guid.Parse("a4472c90-01e4-11ed-0a80-041100104841"));

        //SalesReturn salesReturn = new SalesReturn()
        //{
        //    Agent = demand.Payload.Agent,
        //    Organization = demand.Payload.Organization,
        //    Store = demand.Payload.Store,

        //    Positions = new PagedMetaEntities<SalesReturnPosition>()
        //    {

        //        Rows = new[]
        //        {
        //            new SalesReturnPosition
        //            {
        //                Quantity=1,
        //                Assortment=new Product
        //                {
        //                    Meta=new Meta
        //                    {
        //                        Href=demand.Payload.Positions.Rows[0].Assortment.Meta.Href,
        //                        MetadataHref =demand.Payload.Positions.Rows[0].Assortment.Meta.MetadataHref,
        //                        Type=EntityType.Product,
        //                        MediaType = MediaTypeNames.Application.Json
        //                    }
        //                }
        //            }
        //        }
        //    },
        //    Demand = demand.Payload

        //};
        //await api.SalesReturn.CreateAsync(salesReturn);

        //TaskEntity task = new TaskEntity()
        //{
        //    Assignee=new Employee
        //    {
        //        Meta=new Meta
        //        {
        //            Href = "https://online.moysklad.ru/api/remap/1.2/entity/employee/cd721ba3-07aa-11eb-0a80-0904000641db",
        //            MetadataHref = "https://online.moysklad.ru/api/remap/1.2/entity/employee/metadata",
        //            Type = EntityType.Employee,
        //            MediaType = MediaTypeNames.Application.Json
        //        }
        //    },

        //    Description = "reclamation",
        //};
        //await api.Task.CreateAsync(task);




        Console.WriteLine("OK!");

    }



    ///<summary>
    ///подключение
    ///</summary>
    ///<returns>
    ///возвращает api-подключение
    ///</returns>
    static MoySkladApi GetApiCredentials()
    {
        var credentials = new MoySkladCredentials()
        {
            Username = "aldef@slayn",
            Password = "12345678",
        };
        var httpClient = new HttpClient();
        var api = new MoySkladApi(credentials, httpClient);
        return api;
    }

    ///<summary>
    ///получение списка товаров 
    ///</summary>
    static async Task GetApiPositions(MoySkladApi api, List<AssortmentClass> positions,bool allStok)
    {
        using (var context = new DBSlayn())
        {
            foreach (var orders in context.priceTypeClass)
            {
                context.priceTypeClass.Remove(orders);
            }
            context.SaveChanges();
        }
        if(allStok==true)
        {
            using (var context = new DBSlayn())
            {
                foreach (var assortment in context.assortmentClass)
                {
                    context.assortmentClass.Remove(assortment);
                }
                context.SaveChanges();
            }
        }
        int offset = 0;
        var query = new AssortmentApiParameterBuilder();
        var sklad = await api.Store.GetAllAsync();
        query.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/product/metadata/attributes/27fa75f7-3626-11ec-0a80-02a60003df33").Should().Be("true");
        query.Expand().With(p => p.Product).And.With(p => p.Product.SalePrices);
        if (allStok == false)
        { 
            query.Parameter(p => p.StockStore).Should().Be(sklad.Payload.Rows[0].Meta); 
        }
        while (true)
        {
            query.Offset(offset);
            var response = await api.Assortment.GetAllAsync(query);
            if (response.Payload.Rows.Length == 0)
            {
                break;
            }
            for (int i = 0; i < response.Payload.Rows.Length; i++)
            {

                AssortmentClass assortment = new AssortmentClass();
                assortment.PriceTypes = new List<PriceTypeClass>();
                assortment.Id = response.Payload.Rows[i].Id.ToString();
                assortment.Name = response.Payload.Rows[i].Name.ToString();
                if (allStok == true)
                {
                    assortment.QuantityAllStok = response.Payload.Rows[i].Quantity;
                }
                else
                {
                    assortment.QuantityStock = response.Payload.Rows[i].Quantity;
                }
                //надо получить комплекты await api.Bundle.Equals();

                if (response.Payload.Rows[i].Product.ToString() == "Confiti.MoySklad.Remap.Entities.Product")
                {
                    var positionAssortment = await api.Product.GetAsync(Guid.Parse(response.Payload.Rows[i].Id.ToString()));

                    for (int a = 0; a < positionAssortment.Payload.SalePrices.Length; a++)
                    {
                       
                            PriceTypeClass priceTypeClass = new PriceTypeClass();
                            priceTypeClass.name = positionAssortment.Payload.SalePrices[a].PriceType.Name.ToString();
                            priceTypeClass.price = Convert.ToDecimal(positionAssortment.Payload.SalePrices[a].Value / 100);
                            assortment.PriceTypes.Add(priceTypeClass);
                        
                    }
                }
                else
                    Console.WriteLine(assortment.Id + "\t" + assortment.Name + "Не подгружен!!!");
                positions.Add(assortment);
                using (var context = new DBSlayn())
                {

                    var param = context.assortmentClass.ToList().FirstOrDefault(p => p.Id == assortment.Id);
                    if (param != null)
                    {
                        param.Name = assortment.Name;
                        param.PriceTypes = assortment.PriceTypes;
                        if (allStok == true)
                        {
                            param.QuantityAllStok = assortment.QuantityAllStok;
                        }
                            param.QuantityStock = assortment.QuantityStock;
                        Console.WriteLine("Изменение");
                    }

                    else
                    {
                        context.assortmentClass.Add(assortment);
                        Console.WriteLine("Запись");
                    }

                    context.SaveChanges();

                }
            }
            offset += 1000;
            Console.WriteLine(offset);

        }
        Console.WriteLine("Готово");
    }

    ///<summary>
    ///Получение списка контрагентов
    ///</summary>
    static async Task<List<CounterPartyClass>> GetApiCounterparties(MoySkladApi api, List<CounterPartyClass> counterParties)
    {
        var queryCountr = new ApiParameterBuilder<CounterpartiesQuery>();
        queryCountr.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/counterparty/metadata/attributes/ffbdbe2e-3254-11ec-0a80-00f8000ad694").Should().NotBe("false"); //Выгрузка по рассылке остатков
        //queryCountr.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/counterparty/metadata/attributes/c3905508-f2bf-11ec-0a80-06270018eda1").Should().NotBe(null); //Вышрузка по логину и паролю
        int offset = 0;
        while (true)
        {
            queryCountr.Offset(offset);
            var contrs = await api.Counterparty.GetAllAsync(queryCountr);
            if (contrs.Payload.Rows.Length == 0)
            {
                break;
            }
            for (int countrPartyCount = 0; countrPartyCount < contrs.Payload.Rows.Length; countrPartyCount++)
            {
                CounterPartyClass counterPartyClass = new CounterPartyClass();
                counterPartyClass.Id = contrs.Payload.Rows[countrPartyCount].Id.ToString();
                counterPartyClass.Name = contrs.Payload.Rows[countrPartyCount].Name.ToString();
                if (contrs.Payload.Rows[countrPartyCount].PriceType != null && contrs.Payload.Rows[countrPartyCount].PriceType.Name != null)
                { counterPartyClass.PriceType = contrs.Payload.Rows[countrPartyCount].PriceType.Name.ToString(); }
                counterPartyClass.LoginOfPsswordToTheLC = contrs.Payload.Rows[countrPartyCount].Attributes[contrs.Payload.Rows[countrPartyCount].Attributes.Length - 1].Value.ToString();
                counterPartyClass.LoginOfAccessToTheLC = contrs.Payload.Rows[countrPartyCount].Attributes[contrs.Payload.Rows[countrPartyCount].Attributes.Length - 2].Value.ToString();
                counterPartyClass.counterPartyOrders = new List<OrderClass>();
                counterPartyClass.Meta = contrs.Payload.Rows[countrPartyCount].Meta.Href;
                counterParties.Add(counterPartyClass);
                using (var context = new DBSlayn())
                {

                    var param = context.counterPartyClass.SingleOrDefault(p => p.Id == counterPartyClass.Id);
                    if (param != null)
                    {
                        param.Name = counterPartyClass.Name;
                        param.Meta = counterPartyClass.Meta;
                        param.LoginOfPsswordToTheLC = counterPartyClass.LoginOfPsswordToTheLC;
                        param.LoginOfAccessToTheLC = counterPartyClass.LoginOfAccessToTheLC;
                        Console.WriteLine("Изменение");
                    }

                    else
                    {
                        context.counterPartyClass.Add(counterPartyClass);
                        Console.WriteLine("Запись");
                    }

                    context.SaveChanges();

                }
                Console.WriteLine(counterParties[countrPartyCount].Name);
            }
            offset += 1000;
        }
        return counterParties;

    }

    ///<summary>
    ///получение списка заказов контрагента
    ///</summary>
    static async Task GetApiCounterpartiesOrders(MoySkladApi api, List<CounterPartyClass> counterParties)
    {
        var order = new OrderClass();
        for (int conterPartiecCount =0; conterPartiecCount < counterParties.Count; conterPartiecCount++)
        {
            int offset = 0; 
            var queryOrders = new ApiParameterBuilder<CustomerOrdersQuery>();
            DateTime date = DateTime.Now.Subtract(new TimeSpan(182, 0, 0, 0));
            queryOrders.Parameter("deliveryPlannedMoment").Should().BeGreaterThan(date.ToString("yyyy-MM-dd hh:mm:ss"));
            queryOrders.Parameter("agent").Should().Be(counterParties[conterPartiecCount].Meta);
            //queryOrders.Parameter("state").Should().Be("https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f84956-db44-11e8-9ff4-34e80016406a");
            while (true)
            {
                queryOrders.Offset(offset);
                var orders = await api.CustomerOrder.GetAllAsync(queryOrders);
                if (orders.Payload.Rows.Length == 0)
                {
                    break;
                }

                for (int i = 0; i < orders.Payload.Rows.Length; i++)
                {
                    order.Id = orders.Payload.Rows[i].Id.ToString();
                    order.Name = orders.Payload.Rows[i].Name.ToString();
                    order.DateСreation = orders.Payload.Rows[i].DeliveryPlannedMoment.ToString();
                   
                    if (orders.Payload.Rows[i].State != null)
                    {
                        order.Status = GetState(orders.Payload.Rows[i].State.Meta.Href);
                    }
                    else
                        order.Status = "Новый";
                    counterParties[conterPartiecCount].counterPartyOrders.Add(order);
                    Console.WriteLine(counterParties[conterPartiecCount].counterPartyOrders[i].Name);
                    using (var context = new DBSlayn())
                    {
                        var param = context.counterPartyClass.Include(p=>p.counterPartyOrders).ToList().FirstOrDefault(p => p.Id == counterParties[conterPartiecCount].Id);
                        if (param != null)
                        {
                            if(param.counterPartyOrders==null||param.counterPartyOrders.SingleOrDefault(p=>p.Id==order.Id)==null)
                            {
                                param.counterPartyOrders.Add(order);
                                Console.WriteLine("Добавление");
                                context.SaveChanges();
                            }
                        }
                        else
                            Console.WriteLine("Нет контрагента!");
                    }

                }
                offset += 1000;
            }
        }
    }

    /// <summary>
    /// получение товаров из заказов
    /// </summary>
    /// <param name="api"></param>
    /// <param name="counterParties"></param>
    /// <returns></returns>
    static async Task GetApiCounterpartiesOrdersPositions(MoySkladApi api)
    {
        var position = new PositionClass();
        int offset = 0;
        var query = new ApiParameterBuilder<CustomerOrderQuery>();
        query.Expand().With(p => p.Positions);
        List<OrderClass> orders = new List<OrderClass>();
        using (var context = new DBSlayn())
        {
            foreach (var order in context.orderClass)
            {
                orders.Add(order);
            }
        }
        foreach (var order in orders)
        {
            if (order.positions == null)
            {
                var positions = await api.CustomerOrder.GetAsync(Guid.Parse(order.Id), query);

                for (var j = 0; j < positions.Payload.Positions.Rows.Count(); j++)
                {
                    string[] positionId = positions.Payload.Positions.Rows[j].Assortment.Meta.Href.Split('/');
                    position.Id = positionId[positionId.Count() - 1].ToString();
                    bool paramPosition = true;
                    using (var context = new DBSlayn())
                    {
                        var posId = context.positionClass.SingleOrDefault(p => p.Id == position.Id);
                        if (posId != null)
                        {
                            position.Name = posId.Name;
                            position.priceOldOrder = posId.priceOldOrder;
                            position.OldQuantity = posId.OldQuantity;
                            paramPosition = false;
                            order.sum += order.sum + position.priceOldOrder.Value;
                        }
                    }
                    if (paramPosition == true)
                    {
                        var queryPositions = new AssortmentApiParameterBuilder();
                        queryPositions.Parameter("id").Should().Be(position.Id);
                        var intermediatePositions = await api.Assortment.GetAllAsync(queryPositions);

                        if (intermediatePositions.Payload.Rows.Count() > 0)
                        {
                            position.Name = intermediatePositions.Payload.Rows[0].Name.ToString();
                            Console.WriteLine(position.Name);
                        }
                        else
                            position.Name = "???????";
                        position.priceOldOrder = positions.Payload.Positions.Rows[j].Price.Value;
                        position.OldQuantity = positions.Payload.Positions.Rows[j].Quantity.Value;
                        order.sum += positions.Payload.Positions.Rows[j].Price.Value;

                        using (var context = new DBSlayn())
                        {
                            var param = context.orderClass.Include(p => p.positions).ToList().FirstOrDefault(p => p.Id == order.Id);
                            param.sum = order.sum;
                            if (param != null)
                            {
                                if (param.positions == null || param.positions.SingleOrDefault(p => p.Id == position.Id) == null)
                                {
                                    param.positions.Add(position);
                                    Console.WriteLine("Добавление");
                                }
                            }
                            else
                                Console.WriteLine("заказа");
                            context.SaveChanges();

                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// получение отгрузки из заказа
    /// </summary>
    /// <param name="api"></param>
    /// <returns></returns>
    static async Task GetApiOrdersDemand(MoySkladApi api)
    {
        DemandClass demand=new DemandClass();
        List<OrderClass> orders = new List<OrderClass>();
        using (var context = new DBSlayn())
        {
            foreach (var order in context.orderClass)
            {
                orders.Add(order);
            }
        }
        foreach (var order in orders)
        {
            if (order.Demands == null)
            {
                var demands = await api.CustomerOrder.GetAsync(Guid.Parse(order.Id));
                if (demands.Payload.Demands != null)
                {
                    
                    for (var j = 0; j < demands.Payload.Demands.Count(); j++)
                    {
                        if (demands.Payload.Demands[j].Meta.MetadataHref == "https://online.moysklad.ru/api/remap/1.2/entity/demand/metadata")
                        {
                            string[] demandId = demands.Payload.Demands[j].Meta.Href.Split('/');
                            demand.Id = demandId[demandId.Count() - 1];
                            var intermediateDemand = await api.Demand.GetAsync(Guid.Parse(demandId[demandId.Count() - 1]));
                            demand.Name = intermediateDemand.Payload.Name;
                            using (var context = new DBSlayn())
                            {
                                var orderDemand = context.orderClass.Include(p => p.Demands).ToList().FirstOrDefault(p => p.Id == order.Id);
                                if (orderDemand != null)
                                {
                                    if (orderDemand.Demands == null || orderDemand.Demands.SingleOrDefault(p => p.Id == demand.Id) == null)
                                    {
                                        orderDemand.Demands.Add(demand);
                                        Console.WriteLine(demand.Name);
                                    }
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
            
        }
    }
    
    /// <summary>
    /// Получение возвратов покупателя
    /// </summary>
    /// <param name="api"></param>
    /// <returns></returns>
    static async Task GetApiSalesReturn(MoySkladApi api)
    {
        SalesReturnClass salesReturn = new SalesReturnClass();
        List<DemandClass> demands = new List<DemandClass>();  
        using(var contex=new DBSlayn())
        {
            foreach(var demand in contex.demand)
            {
                demands.Add(demand);
            }
        }
        foreach(var demand in demands)
        {
            if(demand.SalesReturn==null)
            {
                var returns= await api.Demand.GetAsync(Guid.Parse(demand.Id));
                if(returns.Payload.Returns!=null)
                {
                    for(int i=0;i< returns.Payload.Returns.Count();i++)
                    {
                        string[] returnId = returns.Payload.Returns[i].Meta.Href.Split('/');
                        salesReturn.Id = returnId[returnId.Count() - 1];
                        var intermediateReturn = await api.SalesReturn.GetAsync(Guid.Parse(salesReturn.Id));
                        salesReturn.Name = intermediateReturn.Payload.Name;
                        salesReturn.sum = intermediateReturn.Payload.Sum.Value;
                        string[] CounterPartiesId= intermediateReturn.Payload.Agent.Meta.Href.Split('/');
                        salesReturn.CounterPartiesId = CounterPartiesId[CounterPartiesId.Count() - 1];
                        using (var context = new DBSlayn())
                        {
                            var demandReturn=context.demand.Include(p => p.SalesReturn).ToList().FirstOrDefault(p => p.Id == demand.Id);
                            if(demandReturn!=null)
                            {
                                if (demandReturn.SalesReturn == null || demandReturn.SalesReturn.SingleOrDefault(p => p.Id == salesReturn.Id) == null)
                                {
                                    demandReturn.SalesReturn.Add(salesReturn);
                                    Console.WriteLine(salesReturn.Name);
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// получение позиций из возвратов
    /// </summary>
    /// <param name="api"></param>
    /// <returns></returns>
    static async Task GetApiSalesReturnPositions(MoySkladApi api)
    {

        var salesReturnPositions = new SalesReturnPositionsClass();
        int offset = 0;
        var query = new ApiParameterBuilder<SalesReturnQuery>();
        query.Expand().With(p => p.Positions);
        List<SalesReturnClass> orders = new List<SalesReturnClass>();
        using (var context = new DBSlayn())
        {
            foreach (var order in context.salesReturnClass)
            {
                orders.Add(order);
            }
        }
        foreach (var order in orders)
        {
            if (order.SalesReturnPositions == null)
            {
                var positions = await api.SalesReturn.GetAsync(Guid.Parse(order.Id), query);

                for (var j = 0; j < positions.Payload.Positions.Rows.Count(); j++)
                {
                    string[] positionId = positions.Payload.Positions.Rows[j].Assortment.Meta.Href.Split('/');
                    salesReturnPositions.Id = positionId[positionId.Count() - 1].ToString();
                    bool paramPosition = true;
                    using (var context = new DBSlayn())
                    {
                        var posId = context.salesReturnPositionsClass.SingleOrDefault(p => p.Id == salesReturnPositions.Id);
                        if (posId != null)
                        {
                            salesReturnPositions.Name = posId.Name;
                            salesReturnPositions.priceOldOrder = posId.priceOldOrder;
                            salesReturnPositions.OldQuantity = posId.OldQuantity;
                            paramPosition = false;
                        }
                    }
                    if (paramPosition == true)
                    {
                        var queryPositions = new AssortmentApiParameterBuilder();
                        queryPositions.Parameter("id").Should().Be(salesReturnPositions.Id);
                        var intermediatePositions = await api.Assortment.GetAllAsync(queryPositions);

                        if (intermediatePositions.Payload.Rows.Count() > 0)
                        {
                            salesReturnPositions.Name = intermediatePositions.Payload.Rows[0].Name.ToString();
                            Console.WriteLine(salesReturnPositions.Name);
                        }
                        else
                            salesReturnPositions.Name = "???????";
                        salesReturnPositions.priceOldOrder = positions.Payload.Positions.Rows[j].Price.Value;
                        salesReturnPositions.OldQuantity = positions.Payload.Positions.Rows[j].Quantity.Value;
                        order.sum += positions.Payload.Positions.Rows[j].Price.Value;

                        using (var context = new DBSlayn())
                        {
                            var param = context.salesReturnClass.Include(p => p.SalesReturnPositions).ToList().FirstOrDefault(p => p.Id == order.Id);
                            if (param != null)
                            {
                                if (param.SalesReturnPositions == null || param.SalesReturnPositions.SingleOrDefault(p => p.Id == salesReturnPositions.Id) == null)
                                {
                                    param.SalesReturnPositions.Add(salesReturnPositions);
                                    Console.WriteLine("Добавление");
                                }
                            }
                            else
                                Console.WriteLine("заказа");
                            context.SaveChanges();

                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Получение статуса заказа
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    static string GetState(string href)
    {
        string state="";
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f84956-db44-11e8-9ff4-34e80016406a")
        {
            state = "Выполнен";
        }
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f84c2b-db44-11e8-9ff4-34e80016406b")
        {
            state = "Возврат поставщику";
        }
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f840a2-db44-11e8-9ff4-34e800164066")
        {
            state = "Самовывоз";
        }
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f84e1f-db44-11e8-9ff4-34e80016406c")
        {
            state = "Отмена";
        }
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/693013d8-4938-11e9-9ff4-34e8000cb7f9")
        {
            state = "дубль";
        }
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/174d0743-5ed0-11ea-0a80-000f00150769")
        {
            state = "Отмена заявки";
        }
        if (href == "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/dfb3d04c-1479-11eb-0a80-04dd000f981d")
        {
            state = "Выполнен НА";
        }
        if(href== "https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f85266-db44-11e8-9ff4-34e80016406e")
        {
            state = "Рекламация";
        }
        return state;
    }

}

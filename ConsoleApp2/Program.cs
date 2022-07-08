using Confiti.MoySklad.Remap.Api;
using Confiti.MoySklad.Remap.Client;
using Confiti.MoySklad.Remap.Entities;
using Confiti.MoySklad.Remap.Models;
using ConsoleApp2.Model;
using Microsoft.EntityFrameworkCore;
static class Program
{
    static async Task Main(string[] args)
    {
        var api = GetApiCredentials();
        List<CounterPartyClass> counterParties = new List<CounterPartyClass>();
        List<PositionClass> positions = new List<PositionClass>();
        positions=await GetApiPositions(api, positions);
        //counterParties = await GetApiCounterparties(api, counterParties);
        //GetApiCounterpartiesOrders(api, counterParties);
        //ClearPriceTypeDB();
        ClearPositionsDB();
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
    static async Task<List<PositionClass>> GetApiPositions(MoySkladApi api, List<PositionClass> positions)
    { int offset = 0;
        var query = new AssortmentApiParameterBuilder();
        query.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/product/metadata/attributes/27fa75f7-3626-11ec-0a80-02a60003df33").Should().Be("true");
        //query.Parameter(p => p.Name).Should().Contains("Sheffilton");
        query.Expand().With(p => p.Product).And.With(p => p.Product.SalePrices);
        
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

                PositionClass position = new PositionClass();
                position.PriceTypes = new List<PriceTypeClass>();
                position.Id = response.Payload.Rows[i].Id.ToString();
                position.Name = response.Payload.Rows[i].Name.ToString();

                //надо получить комплекты await api.Bundle.Equals();

                if (response.Payload.Rows[i].Product.ToString() == "Confiti.MoySklad.Remap.Entities.Product")
                {
                    var positionAssortment = await api.Product.GetAsync(Guid.Parse(response.Payload.Rows[i].Id.ToString()));

                    for (int a = 0; a < positionAssortment.Payload.SalePrices.Length; a++)
                    {
                        PriceTypeClass priceTypeClass = new PriceTypeClass();
                        priceTypeClass.name = positionAssortment.Payload.SalePrices[a].PriceType.Name.ToString();
                        priceTypeClass.price = Convert.ToDecimal(positionAssortment.Payload.SalePrices[a].Value / 100);
                        position.PriceTypes.Add(priceTypeClass);
                    }
                }
                    
                
                else
                    Console.WriteLine(position.Id + "\t" + position.Name + "Не подгружен!!!");
                positions.Add(position);
                using (var context = new DBSlaynTest())
                {
                    if (context.positionClass.Count() > 0)
                    {
                        var param = context.positionClass.ToList().FirstOrDefault(position).ToString();
                        if (param != null)
                        {
                            context.positionClass.Update(position);
                            Console.WriteLine("Изменение");

                        }
                    }
                    else
                        context.positionClass.Add(position); Console.WriteLine("Запись");

                    context.SaveChanges();

                }
            }
            offset += 1000;
            Console.WriteLine(offset);
            
        }
        
        
        return positions;
        Console.WriteLine("Готово");
    }

    //остатки по складам

    //var remains=await api.Store.GetAllAsync();


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
                Console.WriteLine(counterParties[countrPartyCount].Name);
            }
            offset += 1000;
        }
        return counterParties;

    }

    ///<summary>
    ///получение списка заказов контрагента
    ///</summary>
    static async void GetApiCounterpartiesOrders(MoySkladApi api, List<CounterPartyClass> counterParties)
    {
        
        for (int conterPartiecCount = 0; conterPartiecCount < counterParties.Count; conterPartiecCount++)
        {
            int offset = 0;
            var queryOrders = new ApiParameterBuilder<CustomerOrdersQuery>();
            DateTime date = DateTime.Now.Subtract(new TimeSpan(182, 0, 0, 0));
            queryOrders.Parameter("deliveryPlannedMoment").Should().BeGreaterThan(date.ToString("yyyy-MM-dd hh:mm:ss"));
            queryOrders.Parameter("agent").Should().Be(counterParties[conterPartiecCount].Meta);
            queryOrders.Parameter("state").Should().Be("https://online.moysklad.ru/api/remap/1.2/entity/customerorder/metadata/states/f2f84956-db44-11e8-9ff4-34e80016406a");
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
                    var order = new OrderClass();
                    order.Id = orders.Payload.Rows[i].Id.ToString();
                    order.Name = orders.Payload.Rows[i].Name.ToString();
                    order.Status = "Выполнен";
                    order.DateСreation = orders.Payload.Rows[i].DeliveryPlannedMoment.ToString();
                    counterParties[conterPartiecCount].counterPartyOrders.Add(order);
                    Console.WriteLine(counterParties[conterPartiecCount].counterPartyOrders[i].Name);

                }
                offset += 1000;

            }
        }
        using (var context = new DBSlaynTest())
        {
            context.counterPartyClass.AddRange(counterParties);
            context.SaveChanges();
        }
    }

    static void ClearPriceTypeDB()
    {
        using(var context = new DBSlaynTest())
        {
            foreach(var deleted in context.priceTypeClass)
            {
                context.priceTypeClass.Remove(deleted);
                Console.WriteLine(deleted.id);
                
            }
            context.SaveChanges();
        }
    }
    static void ClearPositionsDB()
    {
        using (var context = new DBSlaynTest())
        {
            foreach (var deleted in context.positionClass)
            {
                context.positionClass.Remove(deleted);
                Console.WriteLine(deleted.Name);

            }
            context.SaveChanges();
        }
    }



    //var order = "6678550908";
    //var queryPositionsOrder = new AssortmentApiParameterBuilder();

    //queryPositionsOrder.Parameter(p => p.Name).Should().Be(order);
    //query.Parameter(p => p.Archived).Should().Be(true).Or.Be(false);
    //var orderPosition = await api.Assortment.GetAllAsync(queryPositionsOrder);









    //получение организаций

    //var organization = await api.Organization.GetAllAsync();



    //создание заказа

    //var newOrder = new CustomerOrder() { Agent = contrs.Payload.Rows[0], Organization = organization.Payload.Rows[1] };
    //await api.CustomerOrder.CreateAsync(newOrder);

    //поиск опреденного заказа и выгрузка товаров

    //var queryOrdersTest = new ApiParameterBuilder<CustomerOrdersQuery>;
    //queryOrdersTest.Parameter("A38384711");
    //var orderTestName = api.CustomerOrder.GetAsync(Guid.ParseExact(queryOrdersTest));

}

using Confiti.MoySklad.Remap.Api;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Confiti.MoySklad.Remap.Client;
using Confiti.MoySklad.Remap.Entities;
using Confiti.MoySklad.Remap.Models;
using ConsoleApp2.Model;

class Program
{
    static void Main(string[] args)
    {
        var api = GetApiCredentials();
        List<PositionClass> positions = new List<PositionClass>();
        //GetPositionsAsync(api,positions);
        List<CounterPartyClass> counterParties = new List<CounterPartyClass>();
        counterParties=await GetCountrPartysAsync(api, counterParties);
        for (int i = 0; i < counterParties.Count; i++)
        {

            Console.WriteLine(counterParties[i].Name);
            for (int b = 0; b < counterParties[i].orders.Count; b++)
            {
                Console.WriteLine(counterParties[i].orders[b].Name);
            }
        }


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
            AccessToken = "1e3ab41b942b4b8bdf28a6559385247af734aae5"
        };
        var httpClient = new HttpClient();
        var api = new MoySkladApi(credentials, httpClient);
        return api;
    }

    ///<summary>
    ///получение списка товаров 
    ///</summary>
    static async void GetPositionsAsync(MoySkladApi api, List<PositionClass> positions)
    {
        int offset = 0;
        var query = new AssortmentApiParameterBuilder();
        query.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/product/metadata/attributes/27fa75f7-3626-11ec-0a80-02a60003df33").Should().Be("true");
        //query.Parameter(p => p.Name).Should().Contains("Sheffilton");
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
                position.Id = response.Payload.Rows[i].Id.ToString();
                position.Name = response.Payload.Rows[i].Name.ToString();
                positions.Add(position);
                Console.WriteLine(i);
            }

            offset += 1000;
            Console.WriteLine(offset);
        }
        Console.WriteLine("Готово");
    }

    //остатки по складам

    //var remains=await api.Store.GetAllAsync();




    ///<summary>
    ///Получение списка контрагентов
    ///</summary>
    static async Task<List<CounterPartyClass>> GetCountrPartysAsync(MoySkladApi api, List<CounterPartyClass> counterPartyClassesList)
    {
        var queryCountr = new ApiParameterBuilder<CounterpartiesQuery>();
        queryCountr.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/counterparty/metadata/attributes/ffbdbe2e-3254-11ec-0a80-00f8000ad694").Should().Be("true");
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
                counterPartyClass.orders=(await GetOrdersCounterParty(api, contrs.Payload.Rows[countrPartyCount], counterPartyClass));
                counterPartyClassesList.Add(counterPartyClass);
                Console.WriteLine(counterPartyClassesList.Count);
            }
            offset += 1000;
        }
        return counterPartyClassesList;
    }

    ///<summary>
    ///получение списка заказов контрагента
    ///</summary>
    static async Task<List<OrderClass>> GetOrdersCounterParty(MoySkladApi api, Counterparty contrs, CounterPartyClass counterPartyClass)
    {
        var queryOrders = new ApiParameterBuilder<CustomerOrdersQuery>();
        int offset = 0;
        queryOrders.Parameter("agent").Should().Be(contrs.Meta.Href);
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
                string[] stat = orders.Payload.Rows[i].State.Meta.Href.Split("/");
                if (orders.Payload.Rows[i].State != null && stat[stat.Length - 1].ToString() == "f2f84956-db44-11e8-9ff4-34e80016406a")
                {
                    string[] idCounterParty = orders.Payload.Rows[i].Agent.Meta.Href.Split("/");
                    var order = new OrderClass();
                    order.Id = orders.Payload.Rows[i].Id.ToString();
                    order.Name = orders.Payload.Rows[i].Name.ToString();
                    order.OrderCounterParty = idCounterParty[8].ToString();
                    order.Status = "Выполнен";
                    order.DateСreation = orders.Payload.Rows[i].DeliveryPlannedMoment.ToString();
                    counterPartyClass.orders.Add(order);
                }
                Console.ReadKey();
            }
            offset += 1000;
            Console.WriteLine(offset);
        }
        return counterPartyClass.orders;
    }




    //var query1 = new ApiParameterBuilder<CustomerOrderQuery>();

    //query1.Expand()
    //    .With(p => p.Positions);

    //var orderProduct = await api.CustomerOrder.GetAsync(Guid.Parse(id), query1);

    //Состыковка заказов и контрагентов по ID контрагента
    //for (int listCounterParty = 0; listCounterParty < CounterPartyClassesList.Count; listCounterParty++)
    //{

    //    if (CounterPartyClassesList[listCounterParty].Id == idCounterParty.ToString())
    //    {
    //        ;
    //        order.Id = orders.Payload.Rows[0].Id.ToString();
    //        order.Name = orders.Payload.Rows[0].Name.ToString();
    //        CounterPartyClassesList[listCounterParty].orders = order;
    //        Console.WriteLine(order.Name);
    //    }
    //}
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
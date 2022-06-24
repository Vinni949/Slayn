using Confiti.MoySklad.Remap.Api;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Confiti.MoySklad.Remap.Client;
using Confiti.MoySklad.Remap.Entities;
using Confiti.MoySklad.Remap.Models;
using ConsoleApp2.Model;

        //var api = GetApiCredentials();
        //List<PositionClass> positions = new List<PositionClass>();
        //GetPositionsAsync(api,positions);
        List<CounterPartyClass> counterParties = new List<CounterPartyClass>();
       


    ///<summary>
    ///подключение
    ///</summary>
    ///<returns>
    ///возвращает api-подключение
    ///</returns>
        var credentials = new MoySkladCredentials()
        {
            AccessToken = "1e3ab41b942b4b8bdf28a6559385247af734aae5"
        };
        var httpClient = new HttpClient();
        var api = new MoySkladApi(credentials, httpClient);



///<summary>
///получение списка товаров 
///</summary>

int offset = 0;
var query = new AssortmentApiParameterBuilder();
List<PositionClass> positions = new List<PositionClass>();
query.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/product/metadata/attributes/27fa75f7-3626-11ec-0a80-02a60003df33").Should().Be("true");
//query.Parameter(p => p.Name).Should().Contains("Sheffilton");
query.Expand().With(p => p.Product).And.With(p=>p.Product.SalePrices);
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

        if (response.Payload.Rows[i].Product.ToString() != "Confiti.MoySklad.Remap.Entities.Product")
        {
            var positionAssortment = await api.Product.GetAsync(Guid.Parse(response.Payload.Rows[i].Id.ToString()));
            Console.WriteLine(position.Name);

            for (int a = 0; a < positionAssortment.Payload.SalePrices.Length; a++)
            {
                PriceTypeClass priceTypeClass = new PriceTypeClass();
                priceTypeClass.name = positionAssortment.Payload.SalePrices[a].PriceType.Name.ToString();
                priceTypeClass.price = Convert.ToDecimal(positionAssortment.Payload.SalePrices[a].Value / 100);
                position.PriceTypes.Add(priceTypeClass);
            }
        }
        positions.Add(position);
        
    }

    offset += 1000;
    Console.WriteLine(offset);
}
Console.WriteLine("Готово");


//остатки по складам

//var remains=await api.Store.GetAllAsync();




///<summary>
///Получение списка контрагентов
///</summary>

var queryCountr = new ApiParameterBuilder<CounterpartiesQuery>();
    queryCountr.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/counterparty/metadata/attributes/ffbdbe2e-3254-11ec-0a80-00f8000ad694").Should().NotBe("false");
    //queryCountr.Parameter("https://online.moysklad.ru/api/remap/1.2/entity/counterparty/metadata/attributes/c3905508-f2bf-11ec-0a80-06270018eda1").Should().NotBe(null);
offset = 0;
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
        counterParties.Add(counterPartyClass);
        Console.WriteLine(counterParties[countrPartyCount].Name);


        ///<summary>
        ///получение списка заказов контрагента
        ///</summary>

        
        var queryOrders = new ApiParameterBuilder<CustomerOrdersQuery>();
        offset = 0;
        queryOrders.Parameter("agent").Should().Be(contrs.Payload.Rows[countrPartyCount].Meta.Href);
        DateTime date = DateTime.Now.Subtract(new TimeSpan(182, 0, 0, 0));
        queryOrders.Parameter("deliveryPlannedMoment").Should().BeGreaterThan(date.ToString("yyyy-MM-dd hh:mm:ss"));
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
                var order = new OrderClass();
                order.Id = orders.Payload.Rows[i].Id.ToString();
                order.Name = orders.Payload.Rows[i].Name.ToString();
                order.DateСreation = orders.Payload.Rows[i].DeliveryPlannedMoment.ToString();
                counterPartyClass.counterPartyOrders.Add(order);
                Console.WriteLine(counterParties[countrPartyCount].counterPartyOrders[i].Name);
                var queryOrderPositions = new ApiParameterBuilder<CustomerOrderQuery>();

                queryOrderPositions.Expand()
                    .With(p => p.Positions);
                var orderProduct = await api.CustomerOrder.GetAsync(Guid.Parse(order.Id), queryOrderPositions);
                order.positions = new List<PositionClass>();
                for (int a = 0; a < orderProduct.Payload.Positions.Rows.Length; a++)
                {
                    string[] idCounterPartyOrders = orderProduct.Payload.Positions.Rows[a].Assortment.Meta.Href.Split("/");
                    PositionClass position = new PositionClass();
                    position.Id = idCounterPartyOrders[idCounterPartyOrders.Length - 1].ToString();
                    position.priceOldOrder = orderProduct.Payload.Positions.Rows[a].Price.ToString();
                    var queryPositionsOrder = new AssortmentApiParameterBuilder();
                    queryPositionsOrder.Parameter(p => p.Id).Should().Be(Guid.Parse(position.Id));
                    query.Parameter(p => p.Archived).Should().Be(true).Or.Be(false);
                    var orderPosition = await api.Assortment.GetAllAsync(queryPositionsOrder);
                    position.Name = orderPosition.Payload.Rows[0].Name;
                    Console.WriteLine(position.Name);
                    order.positions.Add(position);
                }
            }
            offset += 1000;
            
        }
    }
    offset += 1000;
}

Console.WriteLine("OK!");


   
    
    //получение организаций

    //var organization = await api.Organization.GetAllAsync();



    //создание заказа

    //var newOrder = new CustomerOrder() { Agent = contrs.Payload.Rows[0], Organization = organization.Payload.Rows[1] };
    //await api.CustomerOrder.CreateAsync(newOrder);

    //поиск опреденного заказа и выгрузка товаров

    //var queryOrdersTest = new ApiParameterBuilder<CustomerOrdersQuery>;
    //queryOrdersTest.Parameter("A38384711");
    //var orderTestName = api.CustomerOrder.GetAsync(Guid.ParseExact(queryOrdersTest));



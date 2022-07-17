using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class CounterPartyClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? PriceType { get; set; }
        public List<OrderClass> counterPartyOrders { get; set; }
        public string LoginOfAccessToTheLC { get; set; }
        public string LoginOfPsswordToTheLC { get; set; }
        public string Meta { get; set; }
        public List<UserBasket> Basket { get; set; }

    }
}

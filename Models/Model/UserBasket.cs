using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class UserBasket
    {
        public int Id { get; set; }
        public string CounterPartyId { get; set; }
        public CounterPartyClass CounterParty { get; set; }
        public string AssortmentId { get; set; }
        public AssortmentClass Assortment { get; set; }
        public int Count { get; set; }
        public decimal? Price { get; set; }
    }
}

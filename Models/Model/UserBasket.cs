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
        public string PositionId { get; set; }
        public AssortmentClass Position { get; set; }
        public int Count { get; set; }
    }
}

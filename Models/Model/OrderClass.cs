using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class OrderClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateСreation { get; set; }
        public string? Status { get; set; }
        public List<PositionClass> positions { get; set; }
        public long sum { get; set; }
        public string CounterPartyClassId { get; set; }
        public List<DemandClass>? Demands { get; set; }
    }
}

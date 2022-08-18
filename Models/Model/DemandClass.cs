using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class DemandClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Status { get; set; }
        public long? sum { get; set; }
        public List<SalesReturnClass>? SalesReturn { get;set; }
    }
}

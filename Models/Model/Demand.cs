using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class Demand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Status { get; set; }
        public SalesReturn? SalesReturn { get;set; }
    }
}

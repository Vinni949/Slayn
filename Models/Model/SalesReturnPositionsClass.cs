using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class SalesReturnPositionsClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? priceOldOrder { get; set; }
        public double OldQuantity { get; set; }
    }
}

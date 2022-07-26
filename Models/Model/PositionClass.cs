using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models1.Model
{
    public class PositionClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? priceOldOrder { get; set; }
        public List<PriceTypeClass>? PriceTypes { get; set; }
        public double OldQuantity { get; set; }
        public double? QuantityStock { get; set; }
        public double? QuantityAllStok { get; set; }
    }
}

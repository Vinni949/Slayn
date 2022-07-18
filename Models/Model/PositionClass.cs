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
        public string? priceOldOrder { get; set; }
        public List<PriceTypeClass>? PriceTypes { get; set; }
<<<<<<< HEAD
        public double OldQuantity { get; set; }
=======
        public int quantity { get; set; }
>>>>>>> parent of a7de78b (Add PositionToOrders)
    }
}

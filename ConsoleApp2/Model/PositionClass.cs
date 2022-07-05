﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Model
{
    public class PositionClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string priceOldOrder { get; set; }
        public List<PriceTypeClass> PriceTypes { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Model
{
    internal class PositionClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        List<PriceTypeClass> PriceTypes { get; set; }
    }
}

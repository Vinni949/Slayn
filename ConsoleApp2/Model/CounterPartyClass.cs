﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Model
{
    internal class CounterPartyClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PriceType { get; set; }
        public List<OrderClass> orders { get; set; }

    }
}

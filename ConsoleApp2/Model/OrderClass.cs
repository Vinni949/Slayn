﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Model
{
    internal class OrderClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateСreation { get; set; }
        public string Status { get; set; }
        public string OrderCounterParty { get; set; }
        public List<PositionClass> positions { get; set; }

    }
}

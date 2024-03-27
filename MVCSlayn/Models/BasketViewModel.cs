using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCSlayn.Models
{
    public class BasketViewModel
    {
        public int Count { get; set; }
        public string Name { get; set; }

        public BasketViewModel(int count,string name)
        {
            this.Count = count;
            this.Name = name;
        }
    }
}

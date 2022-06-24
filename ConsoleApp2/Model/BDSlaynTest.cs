using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Model
{
    internal class DBSlaynTest : DbContext
    {
        public DbSet<CounterPartyClass> counterPartyClass { get; set; }
        public DbSet<OrderClass> orderClass { get; set; }
        public DbSet<PositionClass> positionClass { get; set; }
        public DbSet<PriceTypeClass> priceTypeClass { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Filename=BDSlaynTest.sqlite");
        }


    }
}

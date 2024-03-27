using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;


namespace Models1.Model
{
    public class DBSlayn : DbContext
    {
        public DBSlayn()
        {
        }

        public DBSlayn(DbContextOptions<DBSlayn> options) : base(options)
        {

        }
        public DbSet<CounterPartyClass> counterPartyClass { get; set; }
        public DbSet<OrderClass> orderClass { get; set; }
        public DbSet<PositionClass> positionClass { get; set; }
        public DbSet<PriceTypeClass> priceTypeClass { get; set; }
        public DbSet<UserBasket> userBaskets { get; set; }
        public DbSet<AssortmentClass> assortmentClass { get; set; }
        public DbSet<DemandClass> demand { get; set; }
        public DbSet<SalesReturnClass> salesReturnClass { get; set; }
        public DbSet<SalesReturnPositionsClass> salesReturnPositionsClass { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            List<string> s = new List<string>();
            StreamReader f = new StreamReader("test.txt");
            while (!f.EndOfStream)
            {
                s.Add(f.ReadLine());
            }
            optionsBuilder.UseSqlServer(s[2]);
        }


    }
}

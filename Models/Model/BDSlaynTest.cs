using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;


namespace Models1.Model
{
    public class DBSlaynTest : DbContext
    {
        public DBSlaynTest(DbContextOptions<DBSlaynTest> options) : base(options)
        {

        }
        public DbSet<CounterPartyClass> counterPartyClass { get; set; }
        public DbSet<OrderClass> orderClass { get; set; }
        public DbSet<PositionClass> positionClass { get; set; }
        public DbSet<PriceTypeClass> priceTypeClass { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb;Database = BdSlaynTest; Trusted_Connection = True;");
        }


    }
}

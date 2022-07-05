using Microsoft.EntityFrameworkCore;

namespace MVCSlayn.Models
{
    public class CounterPartuClassContext: DbContext
    {
        public CounterPartuClassContext(DbContextOptions<CounterPartuClassContext> options) : base(options)
        {

        }
        public DbSet<OrderClass> OrderClass { get; set; }
        public DbSet<PriceTypeClass> PriceType { get; set; }
        public DbSet<PositionClass> PositionClass { get; set; }

    }
}

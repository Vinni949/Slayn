using Microsoft.EntityFrameworkCore;

namespace MVCSlayn.Models
{
    public class CounterPartuClassContext: DbContext
    {
        public CounterPartuClassContext(DbContextOptions<CounterPartuClassContext> options) : base(options)
        {

        }
        public DbSet<CounterPartuClassContext> OrderClass { get; set; }
        public DbSet<CounterPartuClassContext> PriceType { get; set; }
        public DbSet<CounterPartuClassContext> PositionClass { get; set; }

    }
}

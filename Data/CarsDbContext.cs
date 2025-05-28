using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Data
{
    public class CarsDbContext : DbContext
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> opts) : base(opts)
        { }

        public DbSet<Car> Cars { get; set; }
    }
}

using CarDealership.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CarDealership.Data
{
    public class CarsDbContext : IdentityDbContext<IdentityUser>
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options)
        { }

        public DbSet<Car> Cars { get; set; }
    }
}

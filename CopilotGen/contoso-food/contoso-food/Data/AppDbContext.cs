using Microsoft.EntityFrameworkCore;
using contoso_food.Models;

namespace contoso_food.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MenuItem> MenuItems { get; set; }
    }
}

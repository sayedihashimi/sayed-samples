using Microsoft.EntityFrameworkCore;
using AspireShared;

namespace AspireCliReact01.ApiService;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItems { get; set; } = null!;
}

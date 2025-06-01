using Microsoft.EntityFrameworkCore;
using AureusVitalis.Data.Entities;

namespace AureusVitalis.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts)
            : base(opts) { }

        public DbSet<AppUser> Users { get; set; }
    }
}
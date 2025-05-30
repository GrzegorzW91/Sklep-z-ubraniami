using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SklepZUbraniami.Models;
using Microsoft.EntityFrameworkCore;

namespace SklepZUbraniami.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
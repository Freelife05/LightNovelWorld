using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LightNovelSite.Models;

namespace LightNovelSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LightNovelSite.Models.Novels>? Novels { get; set; }
        public DbSet<LightNovelSite.Models.Chapter>? Chapter { get; set; }
    }
}
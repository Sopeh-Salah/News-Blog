using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsBlogAPI.Models;

namespace NewsBlogAPI.Context
{
    public class newsBlogDatabase : IdentityDbContext<Admin>
    {
        public newsBlogDatabase(DbContextOptions options) : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<New> News { get; set; }
    }
}

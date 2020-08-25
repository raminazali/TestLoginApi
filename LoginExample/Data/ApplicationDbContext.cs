using LoginExample.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginExample.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options): base(Options)
        {
            
        }
        public DbSet<UserLogin> UserLogin { get; set; }
    }
}
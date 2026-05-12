using Identity_Entity.api_06.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity_Entity.api_06.Data
{
    public class AppDbContext :DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}

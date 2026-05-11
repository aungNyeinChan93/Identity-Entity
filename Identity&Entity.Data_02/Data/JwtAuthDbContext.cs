using Identity_Entity.Data_02.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity_Entity.Data_02.Data
{
    public class JwtAuthDbContext :IdentityDbContext<AppUser>
    {
        public JwtAuthDbContext(DbContextOptions<JwtAuthDbContext> options):base(options)
        {
            
        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}

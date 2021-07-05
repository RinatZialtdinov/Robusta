using Microsoft.EntityFrameworkCore;
using System;


namespace ApplicationContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Info> Info { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using Mission.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities
{
    public class MissionDbContext : DbContext
    {
        // Add this constructor for dependency injection
        public MissionDbContext(DbContextOptions<MissionDbContext> options)
            : base(options)
        {
        }

        // Parameterless constructor for design-time operations (migrations)
        protected MissionDbContext()
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Tatva",
                Email = "admin@tatvasoft.com",
                Password = "admin",
                PhoneNumber = "01234567890",
                UserType = "admin"
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
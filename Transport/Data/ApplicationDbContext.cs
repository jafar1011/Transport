using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transport.Data.Tables;
using Microsoft.AspNetCore.Identity;

namespace Transport.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Driver", NormalizedName = "DRIVER" },
                new IdentityRole { Id = "2", Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { Id = "3", Name = "Parent", NormalizedName = "PARENT" }
            );
        }
    }


}


    

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Transport.Data.Tables;

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
        public DbSet<DriverPost> DriverPosts { get; set; }
        public DbSet<DriverPostArea> DriverPostAreas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Driver", NormalizedName = "DRIVER" },
                new IdentityRole { Id = "2", Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { Id = "3", Name = "Parent", NormalizedName = "PARENT" }
            );


            builder.Entity<Student>()
        .HasOne(s => s.IdentityUser)
        .WithMany()
        .HasForeignKey(s => s.IdentityUserId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Car>()
       .HasOne(c => c.IdentityUser)
       .WithMany()
       .HasForeignKey(c => c.IdentityUserId)
       .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Driver>()
                .HasOne(d => d.IdentityUser)
                .WithMany()
                .HasForeignKey(d => d.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Parent>()
    .HasOne(p => p.IdentityUser)
    .WithMany()
    .HasForeignKey(p => p.IdentityUserId)
    .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<DriverPost>()
    .HasOne(p => p.Driver)
    .WithMany() 
    .HasForeignKey(p => p.IdentityUserId)
    .HasPrincipalKey(d => d.IdentityUserId);
        }
    }


}


    

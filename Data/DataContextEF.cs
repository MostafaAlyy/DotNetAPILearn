using DotNetAPILearn.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPILearn.Data
{
    public class DataContextEF(IConfiguration config) : DbContext
    {
        private readonly IConfiguration _config = config;

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSalary> UsersSalary { get; set; }
        public virtual DbSet<UserJobInfo> UsersJobInfo { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<User>()
               .ToTable("Users", "TutorialAppSchema").HasKey(u => u.Id);

            modelBuilder.Entity<UserSalary>()
               .ToTable("UserSalary", "TutorialAppSchema").HasKey(u => u.Id);

            modelBuilder.Entity<UserJobInfo>()
               .ToTable("UserJobInfo", "TutorialAppSchema").HasKey(u => u.Id);
        }



    }
}
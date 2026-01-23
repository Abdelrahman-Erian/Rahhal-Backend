using Microsoft.EntityFrameworkCore;
using Rahhal.Models.UserManagment;
using System.Data;
using System.Diagnostics;

namespace Rahhal.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            #region Seed Admin User    
            var passwordHash = "OwsaNEXpDDAXEx+oo3zeNejRgwsl/ezkljw1uA0Thuw=";
            var passwordSalt = "iCkBAbFby/I7C2G5Hg2CWQ==";



            var superAdminId = new Guid("11111111-1111-1111-1111-111111111111");
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = superAdminId,
                    Fname = "Super Admin",
                    Lname = "Admin",
                    Email = "admin@Rahhal.com",
                    UserName ="Super",
                    HashedPassword = passwordHash,
                    Role = Common.Enum.Role.SuperAdmin,
                    SaltPassword = passwordSalt,
                    CreatedBy = "system",
                    CreatedDate  = new DateTime(2026, 1, 22, 10, 16, 17, DateTimeKind.Utc)
                }
            );
            #endregion

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //here as i can see every quary i can see the sql sentacse about that query in the debug console
            //I made NoTracking prevent the change tracker
            //from tracking the entities  that will be slow the perfomance 
            optionsBuilder
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
}

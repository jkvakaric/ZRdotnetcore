using Microsoft.EntityFrameworkCore;
using ZRdotnetcore.Models;

namespace ZRdotnetcore.Data
{
    public class YoctoDbContext : DbContext
    {
        public YoctoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        }
    }
}
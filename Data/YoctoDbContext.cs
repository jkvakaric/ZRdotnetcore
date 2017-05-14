using System;
using Microsoft.EntityFrameworkCore;
using ZRdotnetcore.Models;

namespace ZRdotnetcore.Data
{
    public sealed class YoctoDbContext : DbContext
    {
        public YoctoDbContext(DbContextOptions<YoctoDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<ReadingType> ReadingTypes { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<ActiveReading> ActiveReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().HasMany(u => u.Devices).WithOne(d => d.User);
            modelBuilder.Entity<User>().HasMany(u => u.Readings).WithOne(r => r.Owner);
            modelBuilder.Entity<User>().HasMany(u => u.ActiveReadings).WithOne(ar => ar.Owner);

            modelBuilder.Entity<Device>().ToTable("Devices").HasKey(d => d.Id);
            modelBuilder.Entity<Device>().Property(d => d.Hostname).IsRequired();
            modelBuilder.Entity<Device>().Property(d => d.AddedOn).IsRequired();
            modelBuilder.Entity<Device>().Property(d => d.UpdatedOn).IsRequired().HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Device>().HasOne(d => d.DeviceType).WithMany(dt => dt.Devices);
            modelBuilder.Entity<Device>().HasMany(d => d.ActiveReadings).WithOne(ar => ar.Device);
            modelBuilder.Entity<Device>().HasMany(d => d.Readings).WithOne(r => r.Device);

            modelBuilder.Entity<Reading>().ToTable("Readings").HasKey(r => r.Id);
            modelBuilder.Entity<Reading>().Property(r => r.Timestamp).IsRequired().HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Reading>().Property(r => r.ReadValue).IsRequired();
            modelBuilder.Entity<Reading>().HasOne(r => r.ActiveReading).WithMany(ar => ar.Readings);
            modelBuilder.Entity<Reading>().HasOne(r => r.ReadingType).WithMany(rt => rt.Readings);

            modelBuilder.Entity<ReadingType>().ToTable("ReadingTypes").HasKey(rt => rt.Id);
            modelBuilder.Entity<ReadingType>().Property(rt => rt.Name).IsRequired();

            modelBuilder.Entity<DeviceType>().ToTable("DeviceTypes").HasKey(dt => dt.Id);
            modelBuilder.Entity<DeviceType>().Property(dt => dt.Name).IsRequired();

            modelBuilder.Entity<ActiveReading>().ToTable("ActiveReadings").HasKey(ar => ar.Id);
            modelBuilder.Entity<ActiveReading>().Property(ar => ar.Name).IsRequired();
            modelBuilder.Entity<ActiveReading>().Property(ar => ar.DataFilepath).IsRequired();
            modelBuilder.Entity<ActiveReading>().Property(ar => ar.ActiveSince).IsRequired();
            modelBuilder.Entity<ActiveReading>().HasOne(ar => ar.ReadingType).WithMany(rt => rt.ActiveReadings);
        }
    }
}
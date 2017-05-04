using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ZRdotnetcore.Data;

namespace ZRdotnetcore.Migrations
{
    [DbContext(typeof(YoctoDbContext))]
    [Migration("20170504173949_YoctoSchema")]
    partial class YoctoSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("ZRdotnetcore.Models.ActiveReading", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataFilepath")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeviceId");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("ActiveReadings");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.Device", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedOn");

                    b.Property<string>("DeviceTypeId");

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2017, 5, 4, 19, 39, 48, 697, DateTimeKind.Local));

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DeviceTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.DeviceType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("DeviceTypes");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.Reading", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DeviceId");

                    b.Property<string>("ReadValue")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("ReadingTypeId");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2017, 5, 4, 19, 39, 48, 713, DateTimeKind.Local));

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("ReadingTypeId");

                    b.ToTable("Readings");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.ReadingType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ReadingTypes");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(70);

                    b.Property<string>("FullName")
                        .HasMaxLength(70);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.ActiveReading", b =>
                {
                    b.HasOne("ZRdotnetcore.Models.Device", "Device")
                        .WithMany("ActiveReadings")
                        .HasForeignKey("DeviceId");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.Device", b =>
                {
                    b.HasOne("ZRdotnetcore.Models.DeviceType", "DeviceType")
                        .WithMany("Devices")
                        .HasForeignKey("DeviceTypeId");

                    b.HasOne("ZRdotnetcore.Models.User", "User")
                        .WithMany("Devices")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ZRdotnetcore.Models.Reading", b =>
                {
                    b.HasOne("ZRdotnetcore.Models.Device", "Device")
                        .WithMany("Readings")
                        .HasForeignKey("DeviceId");

                    b.HasOne("ZRdotnetcore.Models.ReadingType", "ReadingType")
                        .WithMany("Readings")
                        .HasForeignKey("ReadingTypeId");
                });
        }
    }
}

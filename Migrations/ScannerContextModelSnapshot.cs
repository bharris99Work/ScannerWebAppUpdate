﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScannerWebAppUpdate.Models;

#nullable disable

namespace ScannerWebAppUpdate.Migrations
{
    [DbContext(typeof(ScannerContext))]
    partial class ScannerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("ScannerWebAppUpdate.Models.Part", b =>
                {
                    b.Property<int>("PartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JobNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReturnOption")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TechOption")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PartId");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.PartHistory", b =>
                {
                    b.Property<int>("PartHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChangedValues")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateChanged")
                        .HasColumnType("TEXT");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PartHistoryId");

                    b.ToTable("PartHistory");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.ReturnOption", b =>
                {
                    b.Property<int>("ReturnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ReturnId");

                    b.ToTable("ReturnOptions");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.TechOption", b =>
                {
                    b.Property<int>("TechId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TechId");

                    b.ToTable("TechOptions");
                });
#pragma warning restore 612, 618
        }
    }
}

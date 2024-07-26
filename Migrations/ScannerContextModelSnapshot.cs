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

            modelBuilder.Entity("ScannerWebAppUpdate.Models.JobPart", b =>
                {
                    b.Property<int>("JobPartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssignedQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("truckId")
                        .HasColumnType("INTEGER");

                    b.HasKey("JobPartId");

                    b.ToTable("JobParts");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.Jobs", b =>
                {
                    b.Property<int>("JobsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("JobNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("JobsId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.POPart", b =>
                {
                    b.Property<int>("POPartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReturnStatus")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("POPartId");

                    b.ToTable("POParts");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.Part", b =>
                {
                    b.Property<int>("PartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PartId");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("PurchaseOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TruckId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PurchaseOrderId");

                    b.ToTable("PurchaseOrders");
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

            modelBuilder.Entity("ScannerWebAppUpdate.Models.Tech", b =>
                {
                    b.Property<int>("TechId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TechName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TechId");

                    b.ToTable("Techs");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.TechTruck", b =>
                {
                    b.Property<int>("TechTruckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TechId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TruckId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TechTruckId");

                    b.ToTable("TechTrucks");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.Truck", b =>
                {
                    b.Property<int>("TruckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TruckName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TruckId");

                    b.ToTable("Trucks");
                });

            modelBuilder.Entity("ScannerWebAppUpdate.Models.TruckPart", b =>
                {
                    b.Property<int>("TruckPartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuantityAllocated")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuantityAvalible")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TruckId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TruckPartId");

                    b.ToTable("TruckParts");
                });
#pragma warning restore 612, 618
        }
    }
}

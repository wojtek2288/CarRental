﻿// <auto-generated />
using System;
using CarRental.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarRental.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220119132756_AddRentalFields")]
    partial class AddRentalFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarRental.Models.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Horsepower")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("datetime2");

                    b.Property<int>("YearOfProduction")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Brand");

                    b.HasIndex("Horsepower");

                    b.HasIndex("Model");

                    b.HasIndex("YearOfProduction");

                    b.ToTable("Cars");

                    b.HasData(
                        new
                        {
                            Id = new Guid("de8725ba-e24d-4bea-b3eb-61f459c4b0c3"),
                            Brand = "Lego",
                            Description = "Taki fajny kolorowy i szybki",
                            Horsepower = 9999,
                            Model = "Custom Model",
                            TimeAdded = new DateTime(2021, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearOfProduction = 2040
                        });
                });

            modelBuilder.Entity("CarRental.Models.Quota", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("RentDuration")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Quotas");
                });

            modelBuilder.Entity("CarRental.Models.Rental", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<bool>("Returned")
                        .HasColumnType("bit");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("CarRental.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DriversLicenseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                            AuthID = "googleid2",
                            DateOfBirth = new DateTime(2000, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DriversLicenseDate = new DateTime(2018, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user2@website.com",
                            Location = "Warsaw",
                            Role = 1
                        },
                        new
                        {
                            Id = new Guid("c72d2e73-93b6-4ddb-bf6e-c778dd425e6b"),
                            AuthID = "googleid",
                            DateOfBirth = new DateTime(1990, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DriversLicenseDate = new DateTime(2008, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user@website.com",
                            Location = "Warsaw",
                            Role = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
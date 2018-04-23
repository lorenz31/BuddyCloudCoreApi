﻿// <auto-generated />
using BuddyCloudCoreApi2.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BuddyCloudCoreApi2.DAL.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactNumber")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOrdered");

                    b.Property<string>("ItemName")
                        .IsRequired();

                    b.Property<int>("Total");

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Stock", b =>
                {
                    b.Property<Guid>("StockId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<decimal>("MarkupPrice");

                    b.Property<int>("Qty");

                    b.Property<Guid>("SKU");

                    b.Property<decimal>("SellingPrice");

                    b.Property<string>("StockName")
                        .IsRequired();

                    b.Property<decimal>("UnitPrice");

                    b.Property<Guid>("UserId");

                    b.HasKey("StockId");

                    b.HasIndex("UserId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Transactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<decimal>("Subtotal");

                    b.Property<DateTime>("TransactionDate");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Customer", b =>
                {
                    b.HasOne("BuddyCloudCoreApi2.Core.Identity.ApplicationUser", "User")
                        .WithMany("Customers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Order", b =>
                {
                    b.HasOne("BuddyCloudCoreApi2.Core.Models.Transactions", "Transaction")
                        .WithMany("Orders")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Stock", b =>
                {
                    b.HasOne("BuddyCloudCoreApi2.Core.Identity.ApplicationUser", "User")
                        .WithMany("Stocks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BuddyCloudCoreApi2.Core.Models.Transactions", b =>
                {
                    b.HasOne("BuddyCloudCoreApi2.Core.Models.Customer", "Customer")
                        .WithMany("Transactions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

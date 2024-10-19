﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XUnitTestProjectWebApp.Context;

#nullable disable

namespace XUnitTestProjectWebApp.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20241019081628_mig_UpdateCategoryIdNullable")]
    partial class mig_UpdateCategoryIdNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("XUnitTestProjectWebApp.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Switchler"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Routerlar"
                        });
                });

            modelBuilder.Entity("XUnitTestProjectWebApp.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ProductColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("ProductPrice")
                        .HasColumnType("float");

                    b.Property<int?>("ProductStock")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("XUnitTestProjectWebApp.Models.Product", b =>
                {
                    b.HasOne("XUnitTestProjectWebApp.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("XUnitTestProjectWebApp.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
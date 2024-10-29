﻿// <auto-generated />
using System;
using DataMigration.TestProject.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DataMigration.TestProject.Migrations
{
    [DbContext(typeof(PostgresContext))]
    partial class PostgresContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DataMigration.TestProject.Models.Sales", b =>
                {
                    b.Property<Guid>("SaleId")
                        .HasColumnType("uuid")
                        .HasColumnName("sale_ıd");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_ıd");

                    b.Property<string>("ProductName")
                        .HasColumnType("text")
                        .HasColumnName("product_name");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("sale_date");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("SaleId", "TenantId");

                    b.ToTable("sales");
                });
#pragma warning restore 612, 618
        }
    }
}

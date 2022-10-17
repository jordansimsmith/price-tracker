﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PriceTracker.Infrastructure.Data;

#nullable disable

namespace PriceTracker.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PriceTrackerContext))]
    [Migration("20221017230913_AddInStockColumn")]
    partial class AddInStockColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PriceTracker.Core.Entities.PriceHistory", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("InStock")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("TargetName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TargetPageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TargetUniqueId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("PriceHistories");
                });
#pragma warning restore 612, 618
        }
    }
}

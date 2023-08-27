﻿// <auto-generated />
using System;
using MVDTestApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVDTestApp.Data.Migrations
{
    [DbContext(typeof(MVDTestContext))]
    [Migration("20230821113857_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MVDTestApp.Data.Entityes.WorkTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("Completion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Executors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FactualHours")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlannedHours")
                        .HasColumnType("int");

                    b.Property<DateTime>("Registration")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("WorkTaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkTaskId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("MVDTestApp.Data.Entityes.WorkTask", b =>
                {
                    b.HasOne("MVDTestApp.Data.Entityes.WorkTask", null)
                        .WithMany("SubTasks")
                        .HasForeignKey("WorkTaskId");
                });

            modelBuilder.Entity("MVDTestApp.Data.Entityes.WorkTask", b =>
                {
                    b.Navigation("SubTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
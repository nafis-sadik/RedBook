﻿// <auto-generated />
using System;
using Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Identity.Data.Migrations
{
    [DbContext(typeof(UserManagementSystemContext))]
    [Migration("20230222000036_SeedData")]
    partial class SeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Identity.Data.Entities.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Identity.Data.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Organizations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrganizationName = "Blume Digital Corp."
                        });
                });

            modelBuilder.Entity("Identity.Data.Entities.Policy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Authorize")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("UserGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("RouteId");

                    b.HasIndex("UserGroupId");

                    b.ToTable("Policies");
                });

            modelBuilder.Entity("Identity.Data.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrganizationId = 1,
                            RoleName = "System Admin"
                        });
                });

            modelBuilder.Entity("Identity.Data.Entities.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Route1")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Route");

                    b.Property<string>("RouteName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Identity.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "00000000-0000-0000-0000-000000000000",
                            AccountBalance = 999999999999m,
                            FirstName = "Nafis",
                            LastName = "Sadik",
                            OrganizationId = 1,
                            Password = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJPQk8xM25hZnUuIiwibmJmIjoxNjc3MDI0MDM2LCJleHAiOjE2Nzc2Mjg4MzYsImlhdCI6MTY3NzAyNDAzNn0.pmlESxWJkdBj8SRc_8AMksUIOFQM0T2rhsuN7B54QQ4",
                            Status = "A",
                            UserName = "nafis_sadik"
                        });
                });

            modelBuilder.Entity("Identity.Data.Entities.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("UserGroupName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Identity.Data.Entities.Policy", b =>
                {
                    b.HasOne("Identity.Data.Entities.Role", "Role")
                        .WithMany("Policies")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_Policies_Roles");

                    b.HasOne("Identity.Data.Entities.Route", "Route")
                        .WithMany("Policies")
                        .HasForeignKey("RouteId")
                        .IsRequired()
                        .HasConstraintName("FK_Policies_Routes");

                    b.HasOne("Identity.Data.Entities.UserGroup", "UserGroup")
                        .WithMany("Policies")
                        .HasForeignKey("UserGroupId")
                        .IsRequired()
                        .HasConstraintName("FK_Policies_UserGroups");

                    b.Navigation("Role");

                    b.Navigation("Route");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("Identity.Data.Entities.Role", b =>
                {
                    b.HasOne("Identity.Data.Entities.Organization", "Organization")
                        .WithMany("Roles")
                        .HasForeignKey("OrganizationId")
                        .IsRequired()
                        .HasConstraintName("FK_Roles_Organizations");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Identity.Data.Entities.Route", b =>
                {
                    b.HasOne("Identity.Data.Entities.Application", "Application")
                        .WithMany("Routes")
                        .HasForeignKey("ApplicationId")
                        .IsRequired()
                        .HasConstraintName("FK_Routes_Applications");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Identity.Data.Entities.User", b =>
                {
                    b.HasOne("Identity.Data.Entities.Organization", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .HasConstraintName("FK_Users_Organizations");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Identity.Data.Entities.UserGroup", b =>
                {
                    b.HasOne("Identity.Data.Entities.Organization", "Organization")
                        .WithMany("UserGroups")
                        .HasForeignKey("OrganizationId")
                        .IsRequired()
                        .HasConstraintName("FK_UserGroups_Organizations");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Identity.Data.Entities.Application", b =>
                {
                    b.Navigation("Routes");
                });

            modelBuilder.Entity("Identity.Data.Entities.Organization", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("UserGroups");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Identity.Data.Entities.Role", b =>
                {
                    b.Navigation("Policies");
                });

            modelBuilder.Entity("Identity.Data.Entities.Route", b =>
                {
                    b.Navigation("Policies");
                });

            modelBuilder.Entity("Identity.Data.Entities.UserGroup", b =>
                {
                    b.Navigation("Policies");
                });
#pragma warning restore 612, 618
        }
    }
}

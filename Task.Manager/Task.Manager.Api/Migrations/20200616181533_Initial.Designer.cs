﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task.Manager.Api.Data;

namespace Task.Manager.Api.Migrations
{
    [DbContext(typeof(ManagerDbContext))]
    [Migration("20200616181533_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Task.Manager.Entities.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WorkerId")
                        .HasColumnType("int");

                    b.HasKey("AssignmentId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("WorkerId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Task.Manager.Entities.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AssignmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("CommentId");

                    b.HasIndex("AssignmentId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Task.Manager.Entities.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Task.Manager.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Task.Manager.Entities.Worker", b =>
                {
                    b.Property<int>("WorkerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WorkerId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ProjectId");

                    b.HasIndex("RoleId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Task.Manager.Entities.Assignment", b =>
                {
                    b.HasOne("Task.Manager.Entities.Project", "Project")
                        .WithMany("Assignments")
                        .HasForeignKey("ProjectId");

                    b.HasOne("Task.Manager.Entities.Worker", "Worker")
                        .WithMany("Assignments")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Task.Manager.Entities.Comment", b =>
                {
                    b.HasOne("Task.Manager.Entities.Assignment", "Assignment")
                        .WithMany("Comments")
                        .HasForeignKey("AssignmentId");
                });

            modelBuilder.Entity("Task.Manager.Entities.Worker", b =>
                {
                    b.HasOne("Task.Manager.Entities.Project", "Project")
                        .WithMany("Workers")
                        .HasForeignKey("ProjectId");

                    b.HasOne("Task.Manager.Entities.Role", "Role")
                        .WithMany("Workers")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}

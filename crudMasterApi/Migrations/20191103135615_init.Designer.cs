﻿// <auto-generated />
using System;
using CrudMasterApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrudMasterApi.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20191103135615_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CrudMasterApi.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Kovin",
                            PostalCode = "26220",
                            RegionId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Beograd",
                            PostalCode = "11000",
                            RegionId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "Banja Luka",
                            PostalCode = "26000",
                            RegionId = 2
                        },
                        new
                        {
                            Id = 4,
                            Name = "Bihac",
                            PostalCode = "21000",
                            RegionId = 2
                        },
                        new
                        {
                            Id = 5,
                            Name = "Luanda",
                            PostalCode = "11000",
                            RegionId = 3
                        });
                });

            modelBuilder.Entity("CrudMasterApi.Entities.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Principal")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.Property<int?>("SchoolId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.HasIndex("SchoolId1");

                    b.ToTable("Modules");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Electrotehnical engineering",
                            Principal = "Jane Doe",
                            SchoolId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Electrotehnical engineering",
                            Principal = "John Doe",
                            SchoolId = 2
                        },
                        new
                        {
                            Id = 3,
                            Name = "Electrotehnical engineering",
                            Principal = "Marc Skimet",
                            SchoolId = 3
                        },
                        new
                        {
                            Id = 4,
                            Name = "Electrotehnical engineering",
                            Principal = "Johny Noxvile",
                            SchoolId = 1
                        },
                        new
                        {
                            Id = 5,
                            Name = "Electrotehnical engineering",
                            Principal = "Partic Star",
                            SchoolId = 1
                        });
                });

            modelBuilder.Entity("CrudMasterApi.Entities.ModuleSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdModule")
                        .HasColumnType("int");

                    b.Property<int>("IdSubject")
                        .HasColumnType("int")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasAlternateKey("IdModule", "IdSubject");

                    b.HasIndex("IdSubject");

                    b.ToTable("ModuleSubjects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IdModule = 2,
                            IdSubject = 1
                        },
                        new
                        {
                            Id = 2,
                            IdModule = 1,
                            IdSubject = 1
                        },
                        new
                        {
                            Id = 3,
                            IdModule = 3,
                            IdSubject = 1
                        },
                        new
                        {
                            Id = 4,
                            IdModule = 3,
                            IdSubject = 3
                        });
                });

            modelBuilder.Entity("CrudMasterApi.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestInt")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Region");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Srbija",
                            TestInt = 111
                        },
                        new
                        {
                            Id = 2,
                            Name = "Bosna",
                            TestInt = 222
                        },
                        new
                        {
                            Id = 3,
                            Name = "Angola",
                            TestInt = 333
                        });
                });

            modelBuilder.Entity("CrudMasterApi.Entities.School", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Schools");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Mail = "gimeko@yahoo.com",
                            Name = "Gimnazija i ekonomska škola Branko Radičević"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 1,
                            Mail = "gimeko@yahoo.com",
                            Name = "Gimnazija i ekonomska škola Branko Radičević"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 2,
                            Mail = "gimeko@yahoo.com",
                            Name = "Gimnazija i ekonomska škola Branko Radičević"
                        },
                        new
                        {
                            Id = 4,
                            CityId = 3,
                            Mail = "gimeko@yahoo.com",
                            Name = "Gimnazija i ekonomska škola Branko Radičević"
                        },
                        new
                        {
                            Id = 5,
                            CityId = 1,
                            Name = "Srednja stručna škola Mihajlo Pupin"
                        });
                });

            modelBuilder.Entity("CrudMasterApi.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Subjects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Electrotehnics"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Physics"
                        },
                        new
                        {
                            Id = 3,
                            Name = "English"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Maths"
                        });
                });

            modelBuilder.Entity("CrudMasterApi.Entities.City", b =>
                {
                    b.HasOne("CrudMasterApi.Entities.Region", "Region")
                        .WithMany("Cities")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("CrudMasterApi.Entities.Module", b =>
                {
                    b.HasOne("CrudMasterApi.Entities.School", "School")
                        .WithMany("Modules")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CrudMasterApi.Entities.School", null)
                        .WithMany()
                        .HasForeignKey("SchoolId1");
                });

            modelBuilder.Entity("CrudMasterApi.Entities.ModuleSubject", b =>
                {
                    b.HasOne("CrudMasterApi.Entities.Module", "Module")
                        .WithMany("SubjectsOfModule")
                        .HasForeignKey("IdModule")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CrudMasterApi.Entities.Subject", "Subject")
                        .WithMany("ModulesOfSubject")
                        .HasForeignKey("IdSubject")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CrudMasterApi.Entities.School", b =>
                {
                    b.HasOne("CrudMasterApi.Entities.City", "City")
                        .WithMany("Schools")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

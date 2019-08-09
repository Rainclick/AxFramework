﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Framework.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressType");

                    b.Property<string>("City");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("CreatorUserId");

                    b.Property<DateTime?>("ExpireDateTime");

                    b.Property<DateTime>("InsertDateTime");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsMainAddress");

                    b.Property<DateTime?>("ModifiedDateTime");

                    b.Property<int?>("ModifierUserId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Entities.Framework.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Callsite");

                    b.Property<int>("CreatorUserId");

                    b.Property<string>("Exception");

                    b.Property<DateTime>("InsertDateTime");

                    b.Property<string>("Level")
                        .IsRequired();

                    b.Property<DateTime>("Logged");

                    b.Property<string>("Logger");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedDateTime");

                    b.Property<int?>("ModifierUserId");

                    b.Property<string>("Port");

                    b.Property<string>("RemoteAddress");

                    b.Property<string>("ServerAddress");

                    b.Property<string>("ServerName");

                    b.Property<string>("Url");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Entities.Framework.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("CreatorUserId");

                    b.Property<bool>("HaveDuplicate");

                    b.Property<string>("Height");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("InsertDateTime");

                    b.Property<bool>("IsPopup");

                    b.Property<string>("Key");

                    b.Property<DateTime?>("ModifiedDateTime");

                    b.Property<int?>("ModifierUserId");

                    b.Property<int>("Order");

                    b.Property<int?>("ParentId");

                    b.Property<string>("Title");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Entities.Framework.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate");

                    b.Property<int>("CreatorUserId");

                    b.Property<string>("FirstName");

                    b.Property<int>("GenderType");

                    b.Property<DateTime>("InsertDateTime");

                    b.Property<bool>("IsActive");

                    b.Property<DateTimeOffset?>("LastLoginDate");

                    b.Property<string>("LastName");

                    b.Property<DateTime?>("ModifiedDateTime");

                    b.Property<int?>("ModifierUserId");

                    b.Property<string>("Password");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Entities.Framework.Address", b =>
                {
                    b.HasOne("Entities.Framework.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Entities.Framework.Menu", b =>
                {
                    b.HasOne("Entities.Framework.Menu", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });
#pragma warning restore 612, 618
        }
    }
}

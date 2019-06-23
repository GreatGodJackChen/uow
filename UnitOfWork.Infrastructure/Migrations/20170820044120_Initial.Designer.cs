﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using UnitOfWork;

namespace UnitOfWork.Migrations
{
    [DbContext(typeof(UnitOfWorkDbContext))]
    [Migration("20170820044120_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UnitOfWork.Customer.ContactAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("ContactPhone");

                    b.Property<string>("ContactRealName");

                    b.Property<string>("County");

                    b.Property<int?>("CustomerId");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Province");

                    b.Property<string>("Street");

                    b.Property<string>("Zip");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("UnitOfWork.Customer.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerName");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("UnitOfWork.Goods.Goods", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("GoodsCategoryId");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("Stock");

                    b.HasKey("Id");

                    b.HasIndex("GoodsCategoryId");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("UnitOfWork.Goods.GoodsCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("GoodsCategories");
                });

            modelBuilder.Entity("UnitOfWork.ShoppingCart.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("UnitOfWork.ShoppingCart.ShoppingCartLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GoodsId");

                    b.Property<int>("Qty");

                    b.Property<int>("ShoppingCartId");

                    b.HasKey("Id");

                    b.HasIndex("GoodsId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingCartLines");
                });

            modelBuilder.Entity("UnitOfWork.Customer.ContactAddress", b =>
                {
                    b.HasOne("UnitOfWork.Customer.Customer")
                        .WithMany("ShippingAddresses")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("UnitOfWork.Goods.Goods", b =>
                {
                    b.HasOne("UnitOfWork.Goods.GoodsCategory", "GoodsCategory")
                        .WithMany("GoodsList")
                        .HasForeignKey("GoodsCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UnitOfWork.ShoppingCart.ShoppingCart", b =>
                {
                    b.HasOne("UnitOfWork.Customer.Customer", "Customer")
                        .WithOne("ShoppingCart")
                        .HasForeignKey("UnitOfWork.ShoppingCart.ShoppingCart", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UnitOfWork.ShoppingCart.ShoppingCartLine", b =>
                {
                    b.HasOne("UnitOfWork.Goods.Goods", "Goods")
                        .WithMany()
                        .HasForeignKey("GoodsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UnitOfWork.ShoppingCart.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartLines")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models1.Model;

#nullable disable

namespace Models1.Migrations
{
    [DbContext(typeof(DBSlaynTest))]
    partial class DBSlaynTestModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Models1.Model.CounterPartyClass", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginOfAccessToTheLC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoginOfPsswordToTheLC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Meta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("counterPartyClass");
                });

            modelBuilder.Entity("Models1.Model.OrderClass", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CounterPartyClassId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DateСreation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("sum")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CounterPartyClassId");

                    b.ToTable("orderClass");
                });

            modelBuilder.Entity("Models1.Model.PositionClass", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("OldQuantity")
                        .HasColumnType("float");

                    b.Property<string>("OrderClassId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("QuantityAllStok")
                        .HasColumnType("float");

                    b.Property<double>("QuantityStock")
                        .HasColumnType("float");

                    b.Property<long?>("priceOldOrder")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OrderClassId");

                    b.ToTable("positionClass");
                });

            modelBuilder.Entity("Models1.Model.PriceTypeClass", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("PositionClassId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.HasIndex("PositionClassId");

                    b.ToTable("priceTypeClass");
                });

            modelBuilder.Entity("Models1.Model.UserBasket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("CounterPartyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PositionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CounterPartyId");

                    b.HasIndex("PositionId");

                    b.ToTable("userBaskets");
                });

            modelBuilder.Entity("Models1.Model.OrderClass", b =>
                {
                    b.HasOne("Models1.Model.CounterPartyClass", null)
                        .WithMany("counterPartyOrders")
                        .HasForeignKey("CounterPartyClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models1.Model.PositionClass", b =>
                {
                    b.HasOne("Models1.Model.OrderClass", null)
                        .WithMany("positions")
                        .HasForeignKey("OrderClassId");
                });

            modelBuilder.Entity("Models1.Model.PriceTypeClass", b =>
                {
                    b.HasOne("Models1.Model.PositionClass", null)
                        .WithMany("PriceTypes")
                        .HasForeignKey("PositionClassId");
                });

            modelBuilder.Entity("Models1.Model.UserBasket", b =>
                {
                    b.HasOne("Models1.Model.CounterPartyClass", "CounterParty")
                        .WithMany("Basket")
                        .HasForeignKey("CounterPartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models1.Model.PositionClass", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CounterParty");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Models1.Model.CounterPartyClass", b =>
                {
                    b.Navigation("Basket");

                    b.Navigation("counterPartyOrders");
                });

            modelBuilder.Entity("Models1.Model.OrderClass", b =>
                {
                    b.Navigation("positions");
                });

            modelBuilder.Entity("Models1.Model.PositionClass", b =>
                {
                    b.Navigation("PriceTypes");
                });
#pragma warning restore 612, 618
        }
    }
}

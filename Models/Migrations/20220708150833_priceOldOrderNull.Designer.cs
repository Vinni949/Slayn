﻿// <auto-generated />
using Models1.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Models1.Migrations
{
    [DbContext(typeof(DBSlaynTest))]
    [Migration("20220708150833_priceOldOrderNull")]
    partial class priceOldOrderNull
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DateСreation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sum")
                        .HasColumnType("int");

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

                    b.Property<string>("OrderClassId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("priceOldOrder")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("Models1.Model.OrderClass", b =>
                {
                    b.HasOne("Models1.Model.CounterPartyClass", null)
                        .WithMany("counterPartyOrders")
                        .HasForeignKey("CounterPartyClassId");
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

            modelBuilder.Entity("Models1.Model.CounterPartyClass", b =>
                {
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

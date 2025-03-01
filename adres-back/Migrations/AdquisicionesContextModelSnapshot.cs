﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using adres.Persistence;

#nullable disable

namespace adres.Migrations
{
    [DbContext(typeof(AdquisicionesContext))]
    partial class AdquisicionesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("adres.Models.Adquisicion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Activo")
                        .HasColumnType("boolean");

                    b.Property<long>("Cantidad")
                        .HasColumnType("bigint");

                    b.Property<string>("Documentacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("date");

                    b.Property<decimal>("Presupuesto")
                        .HasColumnType("numeric");

                    b.Property<string>("Proveedor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TipoBienServicio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unidad")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ValorUnitario")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("adquisicion", (string)null);
                });

            modelBuilder.Entity("adres.Models.Historico", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdquisicionId")
                        .HasColumnType("uuid");

                    b.Property<string>("DetalleCambio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TipoCambio")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AdquisicionId");

                    b.ToTable("historico", (string)null);
                });

            modelBuilder.Entity("adres.Models.Historico", b =>
                {
                    b.HasOne("adres.Models.Adquisicion", "Adquisicion")
                        .WithMany()
                        .HasForeignKey("AdquisicionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adquisicion");
                });
#pragma warning restore 612, 618
        }
    }
}

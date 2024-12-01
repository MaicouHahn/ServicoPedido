﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServicoPedido.Repositorio.Infra;

#nullable disable

namespace ServicoPedido.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241201174206_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("ServicoPedido.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodPedido")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DataPedido")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IdFornecedor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdItem")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPedido");

                    b.ToTable("pedidos");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Trabalho1.Data;

#nullable disable

namespace Trabalho1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250611015819_InitialFinalReset")]
    partial class InitialFinalReset
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.4");

            modelBuilder.Entity("Trabalho1.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Entrada")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Pago")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Saida")
                        .HasColumnType("TEXT");

                    b.Property<int>("VagaId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("ValorTotal")
                        .HasColumnType("TEXT");

                    b.Property<int>("VeiculoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VagaId");

                    b.HasIndex("VeiculoId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Trabalho1.Models.TipoVeiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TipoVeiculos");
                });

            modelBuilder.Entity("Trabalho1.Models.Vaga", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Andar")
                        .HasColumnType("TEXT");

                    b.Property<int>("Numero")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ocupada")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Setor")
                        .HasColumnType("TEXT");

                    b.Property<int>("TipoVeiculoId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VeiculoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TipoVeiculoId");

                    b.HasIndex("VeiculoId");

                    b.ToTable("Vagas");
                });

            modelBuilder.Entity("Trabalho1.Models.Veiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT");

                    b.Property<int>("TipoVeiculoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TipoVeiculoId");

                    b.ToTable("Veiculos");
                });

            modelBuilder.Entity("Trabalho1.Models.Ticket", b =>
                {
                    b.HasOne("Trabalho1.Models.Vaga", "Vaga")
                        .WithMany()
                        .HasForeignKey("VagaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Trabalho1.Models.Veiculo", "Veiculo")
                        .WithMany()
                        .HasForeignKey("VeiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vaga");

                    b.Navigation("Veiculo");
                });

            modelBuilder.Entity("Trabalho1.Models.Vaga", b =>
                {
                    b.HasOne("Trabalho1.Models.TipoVeiculo", "Tipo")
                        .WithMany()
                        .HasForeignKey("TipoVeiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Trabalho1.Models.Veiculo", "Veiculo")
                        .WithMany()
                        .HasForeignKey("VeiculoId");

                    b.Navigation("Tipo");

                    b.Navigation("Veiculo");
                });

            modelBuilder.Entity("Trabalho1.Models.Veiculo", b =>
                {
                    b.HasOne("Trabalho1.Models.TipoVeiculo", "TipoVeiculo")
                        .WithMany("Veiculos")
                        .HasForeignKey("TipoVeiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TipoVeiculo");
                });

            modelBuilder.Entity("Trabalho1.Models.TipoVeiculo", b =>
                {
                    b.Navigation("Veiculos");
                });
#pragma warning restore 612, 618
        }
    }
}

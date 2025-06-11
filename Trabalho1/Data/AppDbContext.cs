using Microsoft.EntityFrameworkCore;
using Trabalho1.Models;

namespace Trabalho1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TipoVeiculo> TipoVeiculos { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para Vaga
            modelBuilder.Entity<Vaga>(entity =>
            {
                // Garante que o TipoVeiculo é obrigatório para Vaga
                entity.HasOne(v => v.Tipo)
                      .WithMany() // Ou WithMany(t => t.Vagas) se TipoVeiculo tiver uma coleção de Vagas
                      .HasForeignKey(v => v.TipoVeiculoId)
                      .IsRequired(); // TipoVeiculoId é Required no modelo Vaga.cs
            });

            // Configuração para Veiculo
            modelBuilder.Entity<Veiculo>(entity =>
            {
                entity.HasOne(v => v.TipoVeiculo)
                      .WithMany(t => t.Veiculos) // Assumindo que TipoVeiculo tem List<Veiculo> Veiculos
                      .HasForeignKey(v => v.TipoVeiculoId)
                      .IsRequired();
            });

            // Configuração para Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasOne(t => t.Veiculo)
                      .WithMany()
                      .HasForeignKey(t => t.VeiculoId)
                      .IsRequired();

                entity.HasOne(t => t.Vaga) // Relação Ticket com Vaga
                      .WithMany()
                      .HasForeignKey(t => t.VagaId)
                      .IsRequired();
            });
        }
    }
}
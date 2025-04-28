using Microsoft.EntityFrameworkCore;
using Trabalho1.Models;

namespace Trabalho1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<TipoVeiculo> TipoVeiculos { get; set; }
        public DbSet<Vaga> Vagas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuração das relações entre entidades
            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.TipoVeiculo)
                .WithMany(t => t.Veiculos)
                .HasForeignKey(v => v.TipoVeiculoId);
                
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Veiculo)
                .WithMany()
                .HasForeignKey(t => t.VeiculoId);
                
            modelBuilder.Entity<Vaga>()
                .HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId);
        }
    }
}
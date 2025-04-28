using Microsoft.EntityFrameworkCore;
using Trabalho1.Models; 

namespace Trabalho1.Data;
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para suas tabelas
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TipoVeiculo> TipoVeiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoVeiculo>()
                .HasMany(t => t.Veiculos)
                .WithOne(v => v.TipoVeiculo)
                .HasForeignKey(v => v.TipoVeiculoId);

            modelBuilder.Entity<Veiculo>()
                .HasMany(v => v.Tickets)
                .WithOne(t => t.Veiculo)
                .HasForeignKey(t => t.VeiculoId);

            modelBuilder.Entity<Vaga>()
                .HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Veiculo>()
                .HasIndex(v => v.Placa)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

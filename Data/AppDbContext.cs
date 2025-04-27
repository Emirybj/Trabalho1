using Microsoft.EntityFrameworkCore;
using Trabalho1.Models;

namespace Trabalho1.Data
{
    ///<summary>
    /// Banco de dados para acessar os dados do estacionamento.
    ///</summary>
public class AppDbContext : DbContext
{
    ///Construtor para injeção de dependência
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
{
}

///<summary>
/// Veiculos cadastrados no estacionamento.
///</summary>
public DbSet<Veiculo> Veiculos { get; set; }

///<summary>
/// Tipo de veículos cadastrados no sistema.
///</summary>
public DbSet<TipoVeiculo> TipoVeiculos { get; set; }

///<summary>
/// Tickets de estacionamento emitidos para veículos.
///</summary>
public DbSet<Ticket> Tickets { get; set; }

///<summary>
/// Configurações do modelo de dados e seus relacionamentos.
///</summary>
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Relacionamento entre TipoVeiculo e Veiculo
    modelBuilder.Entity<TipoVeiculo>()
        .HasMany(t => t.Veiculos) // um tipo de veiculo pode ter varios veiculos
        .WithOne(v => v.TipoVeiculo) // Cada ticket pertence a um veiculo
        .HasForeignKey(v => v.TipoVeiculoId); // Relacionamento pela chave VeiculoId

    // Relacionamento entre Veiculo e ticket
    modelBuilder.Entity<Veiculo>()
        .HasMany(v => v.Tickets) //Um veiculo pode ter varios tickets
        .WithOne(t => t.Veiculo) // Cada ticket pertence a um veiculo
        .HasForeignKey(t => t.VeiculoId); // relacionamento pela chave VeiculoId

    //Garantir que as placas sejam unicas
    modelBuilder.Entity<Veiculo>()
        .HasIndex(v => v.Placa)
        .IsUnique();
    
    base.OnModelCreating(modelBuilder); 
    
    }
}
}


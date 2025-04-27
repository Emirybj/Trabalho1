using Microsoft.EntityFrameworkCore;

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
public DbSet<Models.Veiculo> Veiculos { get; set; }

///<summary>
/// Tipo de veículos cadastrados no sistema.
///</summary>
public DbSet<Models.TipoVeiculo> TipoVeiculos { get; set; }

///<summary>
/// Tickets de estacionamento emitidos para veículos.
///</summary>
public DbSet<Models.Ticket> Tickets { get; set; }

///<summary>
/// Configurações do modelo de dados e seus relacionamentos.
///</summary>
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Relacionamento entre TipoVeiculo e Veiculo
    modelBuilder.Entity<Models.TipoVeiculo>()
        .Hasmany(t => t.Veiculos) // um tipo de veiculo pode ter varios veiculos
        .WithOne(v => v.TipoVeiculo) // Cada ticket pertence a um veiculo
        .HasForeingKey(t => t.Veiculo); // Relacionamento pela chave VeiculoId

    // Relacionamento entre Veiculo e ticket
    modelBuilder.Entity<Models.Veiculo>()
        .HasMany(v => v.Tickets) //Um veiculo pode ter varios tickets
        .WithOne(t => t.Veiculo) // Cada ticket pertence a um veiculo
        .HasForeingKey(t => t.VeiculoId); // relacionamento pela chave VeiculoId

    //Garantir que as placas sejam unicas
    modelBuilder.Entity<Models.Veiculo>()
        .HasIndex(v => v.Placa)
        .IsUnique();
    
    base.OnModelCreating(modelBuilder); 
    
    }
}
}


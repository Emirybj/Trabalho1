// Veiculo class
namespace EstacionamentoAPI.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int TipoVeiculoId { get; set; }
        public TipoVeiculo? TipoVeiculo { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
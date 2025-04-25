// TipoVeiculo class
namespace EstacionamentoAPI.Models
{
    public class TipoVeiculo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal ValorHora { get; set; }
        public ICollection<Veiculo>? Veiculos { get; set; }
    }

}    

    
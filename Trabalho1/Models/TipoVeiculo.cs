using System.ComponentModel.DataAnnotations;
namespace Trabalho1.Models
{
    
    /// Tipo do veículo (carro, moto, etc.)
    
    public class TipoVeiculo
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public List<Veiculo>? Veiculos { get; set; }
    }

}

    
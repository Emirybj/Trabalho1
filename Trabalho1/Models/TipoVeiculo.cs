using System.ComponentModel.DataAnnotations;
namespace Trabalho1.Models
{
    
    /// Tipo do veículo (carro, moto, etc.)
    
    public class TipoVeiculo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        public List<Veiculo>? Veiculos { get; set; }
    }

}

    
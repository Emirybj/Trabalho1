using System.ComponentModel.DataAnnotations;

namespace Trabalho1.Models
{
    /// <summary>
    /// Tipo do veículo (carro, moto, etc.)
    /// </summary>
    public class TipoVeiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do tipo de veículo é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        // Quando criar um TipoVeiculo, não precisa informar a lista de veículos
        public List<Veiculo>? Veiculos { get; set; }
    }
}

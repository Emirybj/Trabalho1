// TipoVeiculo class
using System.ComponentModel.DataAnnotantions;

namespace EstacionamentoAPI.Models
{
    ///<summary>
    /// Tipo do veículo (carro, moto, etc.)
    /// </summary>
    public int Id { get; set; }
    {

    [Required]
    public string Nome { get; set; } = string.Empty;

    public List<Veiculo>? Veiculos { get; set; }

    }
}

    
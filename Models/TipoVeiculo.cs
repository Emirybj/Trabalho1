using System.ComponentModel.DataAnnotations;
namespace Trabalho1.Models
{
    ///<summary>
    /// Tipo do veículo (carro, moto, etc.)
    /// </summary>
    public class TipoVeiculo
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public List<TipoVeiculo>? Veiculos { get; set; }
    }

}

    
using System.ComponentModel.DataAnnotations;


namespace Trabalho1.Models
{
    /// <summary>
    /// Representa um veiculo no sistema
    /// </summary>
    public class Veiculo
    {
        ///ID do veículo (chave primária)
        public int Id { get; set; }
        
        /// Placa do veículo (tem que ser obrigatória)
        [Required]
        [StringLength(8)]
        public string Placa { get; set; } = string.Empty;

        ///Modelo do Veículo
        [Required]
        public string Modelo { get; set; } = string.Empty;

        ///Relação com o tipo  de veículo
        public int TipoVeiculoId { get; set; }

        public TipoVeiculo? TipoVeiculo { get; set; }
    }
}
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
        [StringLength(8, MinimumLength = 7, ErrorMessage = "A placa deve ter entre 7 e 8 caracteres.")]
        public string Placa { get; set; } = string.Empty;

        ///Modelo do Veículo
        [Required]
        [StringLength(50, MinimumLength =2, ErrorMessage = "O modelo deve ter entre 2 e 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        ///Relação com o tipo  de veículo
        public int TipoVeiculoId { get; set; }

        public TipoVeiculo? TipoVeiculo { get; set; }
    }
}
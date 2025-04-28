using System.ComponentModel.DataAnnotations;

namespace Trabalho1.Models
{
    /// <summary>
    /// Representa um veículo no sistema
    /// </summary>
    public class Veiculo
    {
        /// <summary>ID do veículo (chave primária)</summary>
        public int Id { get; set; }

        /// <summary>Placa do veículo (obrigatória, 8 caracteres)</summary>
        [Required(ErrorMessage = "A placa do veículo é obrigatória.")]
        [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres.")]
        public string Placa { get; set; } = string.Empty;

        /// <summary>Modelo do veículo</summary>
        [Required(ErrorMessage = "O modelo do veículo é obrigatório.")]
        public string Modelo { get; set; } = string.Empty;

        /// <summary>Chave estrangeira para TipoVeiculo</summary>
        [Required(ErrorMessage = "O tipo do veículo é obrigatório.")]
        public int TipoVeiculoId { get; set; }

        /// <summary>Informações do tipo do veículo</summary>
        public TipoVeiculo? TipoVeiculo { get; set; }
    }
}

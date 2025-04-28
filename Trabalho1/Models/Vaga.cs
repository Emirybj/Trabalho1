using System.ComponentModel.DataAnnotations;

namespace Trabalho1.Models
{
    /// <summary>
    /// Representa uma vaga no estacionamento
    /// </summary>
    public class Vaga
    {
        /// <summary>ID da vaga (chave primária)</summary>
        public int Id { get; set; }

        /// <summary>Número da vaga</summary>
        [Required(ErrorMessage = "O número da vaga é obrigatório.")]
        public int Numero { get; set; }

        /// <summary>Status da vaga: true = ocupada, false = livre</summary>
        public bool Ocupada { get; set; } = false;

        /// <summary>ID do veículo atualmente estacionado na vaga (se houver)</summary>
        public int? VeiculoId { get; set; }

        /// <summary>Informações do veículo (se houver)</summary>
        public Veiculo? Veiculo { get; set; }
    }
}

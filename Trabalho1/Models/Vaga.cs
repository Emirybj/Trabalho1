using System.ComponentModel.DataAnnotations;

namespace Trabalho1.Models
{
    /// <summary>
    /// Representa uma vaga no estacionamento
    /// </summary>
    public class Vaga
    {
        /// ID da vaga (chave primária)
        public int Id { get; set; }

        /// Número da vaga
        [Required]
        public int Numero { get; set; }

        /// Status da vaga: true = ocupada, false = livre
        public bool Ocupada { get; set; } = false;

        /// Veículo atualmente estacionado na vaga (se houver)
        public int? VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }
    }
}

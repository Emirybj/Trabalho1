using System.ComponentModel.DataAnnotations;

namespace Trabalho1.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public DateTime Entrada { get; set; }

        public DateTime? Saida { get; set; }

        public decimal? ValorTotal { get; set; }

        [Required]
        public int VeiculoId { get; set; }

        public Veiculo? Veiculo { get; set; }

     /// ID da vaga associada a este ticket
        [Required]
        public int VagaId { get; set; }

        /// Propriedade de navegação para a Vaga
        public Vaga? Vaga { get; set; }

        /// Indica se o ticket foi pago
        public bool Pago { get; set; } = false; // Valor padrão como não pago
    }
}

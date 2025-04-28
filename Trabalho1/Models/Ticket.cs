using System.ComponentModel.DataAnnotations;

namespace Trabalho1.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        // Entrada opcional na criação do Ticket (será definida no Controller)
        public DateTime? Entrada { get; set; }

        public DateTime? Saida { get; set; }

        public decimal? ValorTotal { get; set; }

        [Required]
        public int VeiculoId { get; set; }

        public Veiculo? Veiculo { get; set; }
    }
}

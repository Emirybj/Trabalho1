using System.ComponentModel.DataAnnotations;

namespace EstacionamentoAPI.Models //Ticket class
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
    }

}
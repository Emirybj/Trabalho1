using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
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
            [Range(1, 999, ErrorMessage ="Número da vaga deve ser entre 1 e 999.")]
            public int Numero { get; set; }

            /// Status da vaga: true = ocupada, false = livre
            public bool Ocupada { get; set; } = false;

            /// Veículo atualmente estacionado na vaga (se houver)
            public int? VeiculoId { get; set; }
            public Veiculo? Veiculo { get; set; } // Propriedade de navegação para o veículo

            /// ID do Tipo de Veículo associado a esta vaga (ex: Carro, Moto)
            [Required] // Toda vaga deve ter um tipo
            public int TipoVeiculoId { get; set; }

            /// Propriedade de navegação para o Tipo de Veículo
            [ForeignKey("TipoVeiculoId")]
            public TipoVeiculo? Tipo { get; set; }

            /// Andar onde a vaga está localizada 
            public string? Andar { get; set; }

            /// Setor onde a vaga está localizada 
            public string? Setor { get; set; }
        }
    }
    
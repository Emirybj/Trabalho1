using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trabalho1.Models; 
using Trabalho1.Data;
using Microsoft.Extensions.Logging; 

namespace Trabalho1.Controllers
{
    [Route("api/[controller]")] // Define que as rotas serão /api/Ticket
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TicketController> _logger; // Injeção do logger para depuração

        public TicketController(AppDbContext context, ILogger<TicketController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            // Retorna todos os tickets incluindo informações do veículo, tipo de veículo E VAGA
            return await _context.Tickets
                .Include(t => t.Veiculo) // Inclui os dados do veículo
                    .ThenInclude(v => v.TipoVeiculo) // E também o tipo do veículo do veículo
                .Include(t => t.Vaga) // Inclui os dados da vaga
                .ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Veiculo)
                .ThenInclude(v => v.TipoVeiculo)
                .Include(t => t.Vaga)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound("Ticket não encontrado.");
            }

            return ticket;
        }

        // Este método agora recebe um RegistrarTicketRequest e gerencia o veículo internamente.
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket([FromBody] RegistrarTicketRequest request) // <--- ESTE É O NOVO PARÂMETRO!
        {
            _logger.LogInformation("Recebida requisição POST para Ticket (RegistrarTicketRequest) com Placa: {Placa}", request.Placa);

            // === INÍCIO DA TRANSAÇÃO PARA GARANTIR ATOMICIDADE ===
            // Garante que ou o veículo é criado/encontrado, ticket e vaga são atualizados, OU NADA É.
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Procurar ou criar o veículo
                var veiculo = await _context.Veiculos.FirstOrDefaultAsync(v => v.Placa == request.Placa);

                if (veiculo == null)
                {
                    // Veículo não existe, criá-lo
                    _logger.LogInformation("Veículo com placa {Placa} não encontrado. Criando novo veículo.", request.Placa);
                    veiculo = new Veiculo
                    {
                        Placa = request.Placa,
                        Modelo = request.Modelo,
                        TipoVeiculoId = request.TipoVeiculoId
                    };
                    _context.Veiculos.Add(veiculo);
                    await _context.SaveChangesAsync(); // Salva o veículo para obter o ID
                    _logger.LogInformation("Novo veículo {Placa} criado com ID: {VeiculoId}", veiculo.Placa, veiculo.Id);
                }
                else
                {
                    _logger.LogInformation("Veículo com placa {Placa} já existe. Usando veículo existente com ID: {VeiculoId}", request.Placa, veiculo.Id);
                    // Se você não quer que as informações do veículo sejam atualizadas aqui, pode remover esta parte.
                }

                // Verifica se o veículo já tem um ticket aberto
                var ticketAberto = await _context.Tickets
                    .FirstOrDefaultAsync(t => t.VeiculoId == veiculo.Id && t.Saida == null);
                
                if (ticketAberto != null)
                {
                    _logger.LogWarning("Tentativa de registrar ticket para veículo já estacionado (ticket aberto). Placa: {Placa}", veiculo.Placa);
                    await transaction.RollbackAsync(); // Reverte qualquer criação de veículo se for o caso
                    return BadRequest("Este veículo já está estacionado.");
                }

                // Busca uma vaga livre
                var vagaDisponivel = await _context.Vagas
                    .FirstOrDefaultAsync(v => !v.Ocupada);

                if (vagaDisponivel == null)
                {
                    _logger.LogWarning("Estacionamento lotado. Nenhuma vaga disponível para a placa {Placa}.", request.Placa);
                    await transaction.RollbackAsync(); // Reverte criação de veículo
                    return BadRequest("Estacionamento lotado.");
                }

                // Criar o novo ticket
                var newTicket = new Ticket
                {
                    VeiculoId = veiculo.Id,
                    VagaId = vagaDisponivel.Id,
                    Entrada = DateTime.Now,
                    Saida = null,
                    ValorTotal = null,
                    Pago = false
                };
                _context.Tickets.Add(newTicket);
                await _context.SaveChangesAsync(); // Salva o ticket para obter o ID

                // Atualiza a vaga para ocupada e associar o veículo
                vagaDisponivel.Ocupada = true;
                vagaDisponivel.VeiculoId = veiculo.Id;
                _context.Vagas.Update(vagaDisponivel); 
                await _context.SaveChangesAsync(); // Salva as alterações na vaga

                await transaction.CommitAsync(); // Confirma todas as operações da transação
                _logger.LogInformation("Ticket para placa {Placa} registrado com sucesso na vaga {NumeroVaga}. Ticket ID: {TicketId}", veiculo.Placa, vagaDisponivel.Numero, newTicket.Id);
                
                return CreatedAtAction(nameof(GetTicket), new { id = newTicket.Id }, newTicket);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Reverte todas as operações se algo der errado
                _logger.LogError(ex, "Erro crítico ao registrar ticket para placa {Placa}. Transação revertida.", request.Placa);
                return StatusCode(500, "Erro interno ao registrar ticket.");
            }
        }

        // usa para finalizar um ticket
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            var ticketExistente = await _context.Tickets
                .Include(t => t.Veiculo)
                .Include(t => t.Vaga)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticketExistente == null)
            {
                return NotFound("Ticket não encontrado.");
            }

            if (ticketExistente.Saida != null)
            {
                return BadRequest("Este ticket já foi finalizado.");
            }

            ticketExistente.Saida = DateTime.Now;
            ticketExistente.ValorTotal = CalcularValorEstacionamento(
                ticketExistente.Entrada, 
                ticketExistente.Saida.Value);
            
            ticketExistente.Pago = true; 

            if (ticketExistente.Vaga != null && ticketExistente.Vaga.Ocupada)
            {
                ticketExistente.Vaga.Ocupada = false;
                ticketExistente.Vaga.VeiculoId = null;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound("Ticket não encontrado.");
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Endpoint específico para retirar o veículo, baseado na placa
        [HttpPost("retirar")]
        public async Task<ActionResult<Ticket>> RetirarVeiculo([FromBody] RetirarVeiculoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Placa))
            {
                return BadRequest("A placa é obrigatória.");
            }

            var veiculo = await _context.Veiculos
                                        .FirstOrDefaultAsync(v => v.Placa == request.Placa);

            if (veiculo == null)
            {
                return NotFound("Veículo com esta placa não encontrado.");
            }

            var ticketAberto = await _context.Tickets
                .Include(t => t.Veiculo)
                .Include(t => t.Vaga)
                .FirstOrDefaultAsync(t => t.VeiculoId == veiculo.Id && t.Saida == null);

            if (ticketAberto == null)
            {
                return NotFound("Não há ticket aberto para este veículo.");
            }

            ticketAberto.Saida = DateTime.Now;
            ticketAberto.ValorTotal = CalcularValorEstacionamento(
                ticketAberto.Entrada, 
                ticketAberto.Saida.Value);
            ticketAberto.Pago = true; 

            if (ticketAberto.Vaga != null)
            {
                ticketAberto.Vaga.Ocupada = false;
                ticketAberto.Vaga.VeiculoId = null;
            }

            await _context.SaveChangesAsync();

            return Ok(ticketAberto);
        }

        // Método auxiliar para calcular o valor do estacionamento
        private decimal CalcularValorEstacionamento(DateTime entrada, DateTime saida)
        {
            var tempoEstacionado = saida - entrada;
            decimal valorPorHora = 3.0m;
            
            if (tempoEstacionado.TotalHours <= 0)
            {
                return valorPorHora;
            }
            return Math.Ceiling((decimal)tempoEstacionado.TotalHours) * valorPorHora;
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }

    // Classe de Request para o endpoint de RetirarVeiculo recebe a placa
    public class RetirarVeiculoRequest
    {
        public string Placa { get; set; } = string.Empty;
    }
}


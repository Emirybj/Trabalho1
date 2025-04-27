using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trabalho1.Models;

namespace Trabalho1.Controllers
{
    [Route("api/[controller]")] // Define que as rotas serão /api/Ticket
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context; // Conexão com o banco de dados

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            // Retorna todos os tickets incluindo informações do veículo e tipo de veículo
            return await _context.Tickets
                .Include(t => t.Veiculo) // Inclui os dados do veículo
                .ThenInclude(v => v.TipoVeiculo) // E também o tipo do veículo
                .ToListAsync();
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            // Verifica se o veículo existe
            var veiculo = await _context.Veiculos.FindAsync(ticket.VeiculoId);
            if (veiculo == null)
            {
                return BadRequest("Veículo não encontrado.");
            }

            // Verifica se o veículo já está estacionado (tem ticket aberto)
            var ticketAberto = await _context.Tickets
                .FirstOrDefaultAsync(t => t.VeiculoId == ticket.VeiculoId && t.Saida == null);
            
            if (ticketAberto != null)
            {
                return BadRequest("Este veículo já está estacionado.");
            }

            // Busca uma vaga livre
            var vagaDisponivel = await _context.Vagas
                .FirstOrDefaultAsync(v => !v.Ocupada);

            if (vagaDisponivel == null)
            {
                return BadRequest("Estacionamento lotado.");
            }

            // Configura os dados do novo ticket
            ticket.Entrada = DateTime.Now; // Marca horário de entrada
            ticket.Saida = null; // Saída ainda não definida
            ticket.ValorTotal = null; // Valor só será calculado na saída

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            // Encontra o ticket que está sendo finalizado
            var ticketExistente = await _context.Tickets
                .Include(t => t.Veiculo)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticketExistente == null)
            {
                return NotFound("Ticket não encontrado.");
            }

            // Marca a hora de saída e calcula o valor
            ticketExistente.Saida = DateTime.Now;
            ticketExistente.ValorTotal = CalcularValorEstacionamento(
                ticketExistente.Entrada, 
                ticketExistente.Saida.Value);

            // Libera a vaga que estava ocupada
            var vaga = await _context.Vagas
                .FirstOrDefaultAsync(v => v.VeiculoId == ticketExistente.VeiculoId && v.Ocupada);
            
            if (vaga != null)
            {
                vaga.Ocupada = false;
                vaga.VeiculoId = null;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para calcular o valor do estacionamento
        private decimal CalcularValorEstacionamento(DateTime entrada, DateTime saida)
        {
            // Calcula o tempo que o veículo ficou estacionado
            var tempoEstacionado = saida - entrada;
            
            // Cobra R$5,00 por hora (exemplo)
            decimal valorPorHora = 5.0m;
            
            // Arredonda para cima (ex: 1h10min conta como 2h)
            return Math.Ceiling((decimal)tempoEstacionado.TotalHours) * valorPorHora;
        }
    }
}
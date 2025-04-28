using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trabalho1.Models;
using Trabalho1.Data;

namespace Trabalho1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets
                .Include(t => t.Veiculo)
                .ThenInclude(v => v.TipoVeiculo)
                .ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Veiculo)
                .ThenInclude(v => v.TipoVeiculo)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound("Ticket não encontrado.");
            }

            return ticket;
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var veiculo = await _context.Veiculos.FindAsync(ticket.VeiculoId);
            if (veiculo == null)
            {
                return BadRequest("Veículo não encontrado.");
            }

            var ticketAberto = await _context.Tickets
                .FirstOrDefaultAsync(t => t.VeiculoId == ticket.VeiculoId && t.Saida == null);

            if (ticketAberto != null)
            {
                return BadRequest("Este veículo já está estacionado.");
            }

            var vagaDisponivel = await _context.Vagas
                .FirstOrDefaultAsync(v => !v.Ocupada);

            if (vagaDisponivel == null)
            {
                return BadRequest("Estacionamento lotado.");
            }

            ticket.Entrada = DateTime.Now;
            ticket.Saida = null;
            ticket.ValorTotal = null;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id)
        {
            var ticketExistente = await _context.Tickets
                .Include(t => t.Veiculo)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticketExistente == null)
            {
                return NotFound("Ticket não encontrado.");
            }

            // Garante que a entrada não é nula
            if (ticketExistente.Entrada == default)
            {
                return BadRequest("Data de entrada inválida.");
            }

            ticketExistente.Saida = DateTime.Now;

            ticketExistente.ValorTotal = CalcularValorEstacionamento(
                ticketExistente.Entrada, 
                ticketExistente.Saida ?? DateTime.Now // Protegendo contra nulo
            );

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

        private decimal CalcularValorEstacionamento(DateTime entrada, DateTime saida)
        {
            var tempoEstacionado = saida - entrada;
            decimal valorPorHora = 5.0m;
            return Math.Ceiling((decimal)tempoEstacionado.TotalHours) * valorPorHora;
        }
    }
}

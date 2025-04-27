using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trabalho1.Models;

namespace Trabalho1.Controllers
{
    [Route("api/[controller]")]  // Todas as rotas começam com /api/Vaga
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly AppDbContext _context;  // Conexão com o banco de dados

        public VagaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vaga
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vaga>>> GetVagas()
        {
            // Retorna todas as vagas do estacionamento
            return await _context.Vagas.ToListAsync();
        }

        // GET: api/Vaga/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vaga>> GetVaga(int id)
        {
            // Busca uma vaga específica pelo ID
            var vaga = await _context.Vagas.FindAsync(id);

            if (vaga == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            return vaga;
        }

        // GET: api/Vaga/Livres
        [HttpGet("Livres")]
        public async Task<ActionResult<IEnumerable<Vaga>>> GetVagasLivres()
        {
            // Retorna apenas as vagas que estão livres (não ocupadas)
            return await _context.Vagas.Where(v => !v.Ocupada).ToListAsync();
        }

        // POST: api/Vaga
        [HttpPost]
        public async Task<ActionResult<Vaga>> PostVaga(Vaga vaga)
        {
            // Verifica se já existe vaga com esse número
            if (_context.Vagas.Any(v => v.Numero == vaga.Numero))
            {
                return BadRequest("Já existe uma vaga com este número.");
            }

            // Nova vaga sempre começa como livre
            vaga.Ocupada = false;
            vaga.VeiculoId = null;

            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVaga), new { id = vaga.Id }, vaga);
        }

        // PUT: api/Vaga/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVaga(int id, Vaga vaga)
        {
            // Verifica se o ID da URL bate com o da vaga
            if (id != vaga.Id)
            {
                return BadRequest("ID da vaga não confere.");
            }

            // Verifica se a vaga existe
            var vagaExistente = await _context.Vagas.FindAsync(id);
            if (vagaExistente == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            // Atualiza apenas os campos permitidos
            vagaExistente.Numero = vaga.Numero;
            vagaExistente.Ocupada = vaga.Ocupada;

            // Se está marcando como livre, remove o veículo
            if (!vaga.Ocupada)
            {
                vagaExistente.VeiculoId = null;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VagaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Vaga/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaga(int id)
        {
            // Busca a vaga para deletar
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            // Não permite deletar vaga ocupada
            if (vaga.Ocupada)
            {
                return BadRequest("Não é possível excluir vaga ocupada.");
            }

            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VagaExists(int id)
        {
            // Método auxiliar para verificar se vaga existe
            return _context.Vagas.Any(e => e.Id == id);
        }
    }
}
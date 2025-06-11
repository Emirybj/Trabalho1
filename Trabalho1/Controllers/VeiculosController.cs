using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho1.Data;
using Trabalho1.Models;
using System.Collections.Generic;
using System.Linq; // Adicione este using
using System.Threading.Tasks;

namespace Trabalho1.Controllers
{
    ///<summary>
    /// Controlador para gerenciar ve\u00EDculos
    ///</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        ///<summary>
        /// Retorna todos os ve\u00EDculos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            return await _context.Veiculos
                .Include(v => v.TipoVeiculo)
                .ToListAsync();
        }

        ///<summary>
        /// Retorna um ve\u00EDculo pelo ID
        ///</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.TipoVeiculo)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
                return NotFound();

            return veiculo;
        }

        ///<summary>
        /// Atualiza os dados de um ve\u00EDculo
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o tipo de ve\u00EDculo existe
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de ve\u00EDculo inv\u00E1lido");

            // Verifica placa duplicada para outros ve\u00EDculos (excluindo o ve\u00EDculo atual)
            if (await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa && v.Id != veiculo.Id))
                return BadRequest("Placa j\u00E1 cadastrada por outro ve\u00EDculo."); // Mensagem mais clara

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        ///<summary>
        /// Adiciona um novo ve\u00EDculo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se tipo de ve\u00EDculo existe
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de ve\u00EDculo inv\u00E1lido");

            // === A REGRA DE VERIFICA\u00C7\u00C3O DE PLACA DUPLICADA ===
            // Esta \u00E9 a linha que causa o problema se o ve\u00EDculo n\u00E3o foi REALMENTE removido ou se h\u00E1 um ticket \u00F3rf\u00E3o.
            if (await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa))
                return BadRequest("Placa j\u00E1 cadastrada."); // Mensagem de erro que voc\u00EA est\u00E1 a ver.

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeiculo", new { id = veiculo.Id }, veiculo);
        }

        ///<summary>
        /// Remove um ve\u00EDculo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound("Ve\u00EDculo n\u00E3o encontrado."); // Mensagem mais espec\u00EDfica

            // === NOVO: Verifique se h\u00E1 tickets relacionados a este ve\u00EDculo ===
            // Se h\u00E1 tickets para este ve\u00EDculo, n\u00E3o podemos simplesmente apag\u00E1-lo diretamente,
            // ou a base de dados pode ficar inconsistente (tickets com VeiculoId inv\u00E1lido).
            var hasRelatedTickets = await _context.Tickets.AnyAsync(t => t.VeiculoId == id);
            if (hasRelatedTickets)
            {
                // Op\u00E7\u00E3o 1 (Mais segura): Retornar erro e exigir remo\u00E7\u00E3o dos tickets primeiro
                return BadRequest("N\u00E3o \u00E9 poss\u00EDvel excluir o ve\u00EDculo, pois ele possui tickets no hist\u00F3rico.");

                // Op\u00E7\u00E3o 2 (Mais complexa, requer revis\u00E3o de l\u00F3gica): Apagar tickets em cascata ou desvincular
                // Para simplificar, a op\u00E7\u00E3o 1 \u00E9 mais clara para o usu\u00E1rio.
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // M\u00E9todo auxiliar que verifica se ve\u00EDculo existe
        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}

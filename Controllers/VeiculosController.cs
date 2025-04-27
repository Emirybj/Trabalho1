using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstacionamentoAPI.Data;
using EstacionamentoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstacionamentoAPI.Controllers
{
    ///<summary>
    /// Controlador para gerenciar veículos
    ///</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext_context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        ///<summary>
        /// Retorna todos os veículos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            return await _context.Veiculos
                .Include(v => v.TipoVeiculo)
                .ToListAsync();
        }

        ///<summary>
        /// Retorna um veiculo pelo ID
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
        /// Atualiza os dados de um veículo
        /// </summary>
        [HttpPut("{id}")]
        public async Taks<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
                return BadRequest();

            //Verifica se o tipo do veiculo existe
            if (!await _context.TipoVeiculo.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de veículo inválido");

            //verifica placa duplicada
            if (await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa && v.Id != veiculo.Id))
                return BadRequest("Placa já cadastrada");

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
        /// Adiciona um novo veiculo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            //Verifica se tipo de veiculo existe
            if (!await _context.TipoVeiculo.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de veículo inválido");

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangeAsync();

            return CreatedAtAction("GetVeiculo", new { id = veiculo.Id }, veiculo);
        }

        ///<summary>
        /// remove um veiculo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound();

            //verifica se tem tickets ligados
            if(await _context.Tickets.AnyAsync(t => t.VeiculoId == id))
                return BadRequest("Não é possível excluir um veículo com tickets");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangeAsync();

            return NoContent();
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(v => v.Id == id);
        }
    }
}

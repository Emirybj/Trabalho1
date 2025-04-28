using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho1.Data;
using Trabalho1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trabalho1.Controllers
{
    ///<summary>
    /// Controlador para gerenciar veículos
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
        /// Retorna um veículo pelo ID
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
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o tipo de veículo existe
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de veículo inválido");

            // Verifica placa duplicada
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
        /// Adiciona um novo veículo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se tipo de veículo existe
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de veículo inválido");

            // Verifica placa duplicada
            if (await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa))
                return BadRequest("Placa já cadastrada");

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeiculo", new { id = veiculo.Id }, veiculo);
        }

        ///<summary>
        /// Remove um veículo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar que verifica se veículo existe
        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}

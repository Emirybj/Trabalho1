using Microsoft.AspNetCore.Mvc;
using Microsoft.EntetyFrameworkCore;
using EstacionamentoAPI.Data;
using EstacionamentoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstacionamentoAPI.Collections
{
    ///<summary>
    /// Controlador para gerenciar tipos de veículos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TipoVeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoVeiculosController(AppDbContext context)
        {
            _context = context;
        }

        ///<summary>
        /// Retorna todos os tipos de veículos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoVeiculo>> GetTipoVeiculo()
        {
            return await _context.TipoVeiculo.ToListAsync();
        }

        ///<summary>
        /// Retorna um tipo de veículo pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoVeiculo>> GetTipoVeiculo(int id)
        {
            var tipoVeiculo = await _context.TipoVeiculo.FindAsync(id);

            if (tipoVeiculo == null)
                return NotFound();

            return tipoVeiculo;    
        }

        ///<summary>
        /// Atualiza um tipo de veículo
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoVeiculo(int id, TipoVeiculo tipoVeiculo)
        {
            if (id ! tipoVeiculo.Id)
                return BadRequest();

            _context.Entry(tipoVeiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoVeiculoExits(id))
                    return NotFound();
                else
                    throw
            }

            return NoContent();
        }

        ///<summary>
        /// Adciona um tipo de veiculo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TipoVeiculo>> PostTipoVeiculo(TipoVeiculo tipoVeiculo)
        {
            _context.TipoVeiculo.Add(tipoVeiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoVeiculo", new { id = tipoVeiculo.Id }, tipoVeiculo);
        }

        ///<summary>
        /// Remove um tipo de veículo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoVeiculo(int id)
        {
            var tipoVeiculo = await _context.TipoVeiculo.FindAsync(id);
            if (tipoVeiculo == null)
                return NotFound();

            //Verifica se existe algum veículo usando este tipo
            var temVeículos = await _context.Veiculo.AnyAsync(v => v.TipoVeiculoId == id);

            _context.TipoVeiculos.Remove(tipoVeiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoVeiculoExits(int id)
        {
            return _context.TipoVeiculo.Any( e => i.Id == id);
        }
    }   
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trabalho1.Models;
using Trabalho1.Data;
using Microsoft.Extensions.Logging;

namespace Trabalho1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VagaController> _logger; // Logger injetado no construtor

        public VagaController(AppDbContext context, ILogger<VagaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Vaga
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vaga>>> GetVagas()
        {
            return await _context.Vagas
                                 .Include(v => v.Tipo) // Inclui o TipoVeiculo relacionado
                                 .ToListAsync();
        }

        // GET: api/Vaga/5
        [HttpGet("{id}")] // O nome da ação é "GetVaga" por conta do padrão
        public async Task<ActionResult<Vaga>> GetVaga(int id)
        {
            var vaga = await _context.Vagas
                                     .Include(v => v.Tipo) // Inclui o TipoVeiculo ao buscar por ID
                                     .FirstOrDefaultAsync(v => v.Id == id);

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
            return await _context.Vagas.Where(v => !v.Ocupada).ToListAsync();
        }

        // POST: api/Vaga
        [HttpPost]
        public async Task<ActionResult<Vaga>> PostVaga(Vaga vaga)
        {
            _logger.LogInformation("Recebida requisição POST para Vaga: Número={NumeroVaga}, TipoID={TipoId}", vaga.Numero, vaga.TipoVeiculoId);

            // Se o ModelState não for válido, vai retornar BadRequest com os erros
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Falha de validação do modelo para Vaga. Erros: {@ValidationErrors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            // Verifica se já existe vaga com esse número
            if (_context.Vagas.Any(v => v.Numero == vaga.Numero))
            {
                _logger.LogWarning("Tentativa de cadastrar vaga com número duplicado: {Numero}", vaga.Numero);
                return BadRequest("Já existe uma vaga com este número.");
            }

            // Verifica se o TipoVeiculoId fornecido já existe no banco de dados
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == vaga.TipoVeiculoId))
            {
                _logger.LogWarning("Tentativa de cadastrar vaga com TipoVeiculoId inválido: {TipoId}", vaga.TipoVeiculoId);
                return BadRequest("Tipo de veículo inválido.");
            }

            // Define status inicial para nova vaga
            vaga.Ocupada = false;
            vaga.VeiculoId = null; // Vaga nova que não tem veículo associado

            _context.Vagas.Add(vaga);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Vaga {NumeroVaga} cadastrada com sucesso. ID: {VagaId}", vaga.Numero, vaga.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar nova vaga no banco de dados.");
                return StatusCode(500, "Erro interno ao salvar a vaga.");
            }

            // Vai retorna 201 Created com a URL para a nova vaga criada
            // GetVaga é o nome da ação que busca uma única vaga por ID.
            return CreatedAtAction("GetVaga", new { id = vaga.Id }, vaga);
        }

        // PUT: api/Vaga/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVaga(int id, Vaga vaga)
        {
            if (id != vaga.Id)
            {
                return BadRequest("ID da vaga não confere.");
            }

            if (!ModelState.IsValid) // Adiciona validação para o PUT também
            {
                return BadRequest(ModelState);
            }

            // Verifiqua se o TipoVeiculoId é válido ao atualizar
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == vaga.TipoVeiculoId))
            {
                return BadRequest("Tipo de veículo inválido.");
            }

            var vagaExistente = await _context.Vagas.FindAsync(id);
            if (vagaExistente == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            // Atualiza apenas as propriedades que podem ser alteradas externamente
            vagaExistente.Numero = vaga.Numero;
            vagaExistente.Andar = vaga.Andar;
            vagaExistente.Setor = vaga.Setor;
            vagaExistente.TipoVeiculoId = vaga.TipoVeiculoId; // Permite alterar o tipo da vaga

            try
            {
                _context.Entry(vagaExistente).State = EntityState.Modified; // Marca como modificado
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
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null)
            {
                return NotFound("Vaga não encontrada.");
            }

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
            return _context.Vagas.Any(e => e.Id == id);
        }
    }
}

    
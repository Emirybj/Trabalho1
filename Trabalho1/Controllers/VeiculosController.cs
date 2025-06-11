using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho1.Data;
using Trabalho1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trabalho1.Controllers
{
    ///<summary>
    /// Controlador para gerenciar veículos
    ///</summary>
    [Route("api/[controller]")] // Define a rota base para este controlador como 'api/Veiculos'
    [ApiController] // Indica que esta classe é um controlador de API e habilita comportamentos específicos de API
    [Produces("application/json")] // Especifica que as ações deste controlador produzirão respostas no formato JSON
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context; // Declara uma variável somente leitura para o contexto do banco de dados

        /// <summary>
        /// Construtor da classe VeiculosController.
        /// Inicializa o controlador com o contexto do banco de dados.
        /// </summary>
        /// <param name="context">O contexto do banco de dados da aplicação, injetado via DI.</param>
        public VeiculosController(AppDbContext context)
        {
            _context = context; // Atribui o contexto do banco de dados injetado à variável local
        }

        ///<summary>
        /// Retorna todos os veículos
        /// </summary>
        /// <returns>Uma lista de veículos, incluindo seus tipos de veículo associados.</returns>
        [HttpGet] // Mapeia este método para requisições HTTP GET
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            // Retorna todos os veículos do banco de dados, incluindo os dados do tipo de veículo relacionado.
            return await _context.Veiculos
                .Include(v => v.TipoVeiculo) // Inclui os dados da entidade TipoVeiculo relacionada
                .ToListAsync(); // Converte a consulta para uma lista assincronamente
        }

        ///<summary>
        /// Retorna um veículo pelo ID
        ///</summary>
        /// <param name="id">O ID do veículo a ser retornado.</param>
        /// <returns>Um veículo específico ou NotFound se não for encontrado.</returns>
        [HttpGet("{id}")] // Mapeia este método para requisições HTTP GET com um parâmetro de ID na rota
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            // Busca um veículo pelo ID, incluindo os dados do tipo de veículo relacionado.
            var veiculo = await _context.Veiculos
                .Include(v => v.TipoVeiculo) // Inclui os dados da entidade TipoVeiculo relacionada
                .FirstOrDefaultAsync(v => v.Id == id); // Encontra o primeiro veículo com o ID correspondente

            if (veiculo == null)
                return NotFound(); // Retorna 404 Not Found se o veículo não for encontrado

            return veiculo; // Retorna o veículo encontrado
        }

        ///<summary>
        /// Atualiza os dados de um veículo
        /// </summary>
        /// <param name="id">O ID do veículo a ser atualizado.</param>
        /// <param name="veiculo">Os dados do veículo atualizado.</param>
        /// <returns>NoContent se a atualização for bem-sucedida, BadRequest ou NotFound caso contrário.</returns>
        [HttpPut("{id}")] // Mapeia este método para requisições HTTP PUT com um parâmetro de ID na rota
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            // Verifica se o ID da rota corresponde ao ID do objeto veículo
            if (id != veiculo.Id)
                return BadRequest(); // Retorna 400 BadRequest se os IDs não coincidirem

            // Verifica a validade do modelo (validações de dados definidas no modelo Veiculo)
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Retorna 400 BadRequest com os erros de validação

            // Verifica se o TipoVeiculoId fornecido existe no banco de dados
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de veículo inválido"); // Retorna 400 BadRequest se o tipo de veículo for inválido

            // Verifica se a placa já está cadastrada para outro veículo (evita placas duplicadas)
            if (await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa && v.Id != veiculo.Id))
                return BadRequest("Placa já cadastrada por outro veículo."); // Retorna 400 BadRequest se a placa já existir

            // Marca o estado da entidade como Modificada para que o EF a atualize no banco de dados
            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            }
            catch (DbUpdateConcurrencyException) // Captura exceções de concorrência (ex: registro excluído por outro processo)
            {
                if (!VeiculoExists(id)) // Verifica se o veículo ainda existe
                    return NotFound(); // Retorna 404 Not Found se o veículo não existir mais
                else
                    throw; // Lança a exceção se for outro problema de concorrência
            }

            return NoContent(); // Retorna 204 NoContent, indicando que a atualização foi bem-sucedida sem conteúdo para retornar
        }

        ///<summary>
        /// Adiciona um novo veículo
        /// </summary>
        /// <param name="veiculo">Os dados do novo veículo a ser adicionado.</param>
        /// <returns>O veículo recém-criado com seu ID, ou BadRequest se a criação falhar.</returns>
        [HttpPost] // Mapeia este método para requisições HTTP POST
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            // Verifica a validade do modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Retorna 400 BadRequest com os erros de validação

            // Verifica se o TipoVeiculoId fornecido existe no banco de dados
            if (!await _context.TipoVeiculos.AnyAsync(t => t.Id == veiculo.TipoVeiculoId))
                return BadRequest("Tipo de veículo inválido"); // Retorna 400 BadRequest se o tipo de veículo for inválido

            // Verifica se a placa já está cadastrada
            if (await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa))
                return BadRequest("Placa já cadastrada."); // Retorna 400 BadRequest se a placa já existir

            _context.Veiculos.Add(veiculo); // Adiciona o novo veículo ao contexto
            await _context.SaveChangesAsync(); // Salva o novo veículo no banco de dados

            // Retorna 201 CreatedAtAction com o novo veículo e um cabeçalho de localização para o recurso criado
            return CreatedAtAction("GetVeiculo", new { id = veiculo.Id }, veiculo);
        }

        ///<summary>
        /// Remove um veículo
        /// </summary>
        /// <param name="id">O ID do veículo a ser removido.</param>
        /// <returns>NoContent se a remoção for bem-sucedida, NotFound ou BadRequest caso contrário.</returns>
        [HttpDelete("{id}")] // Mapeia este método para requisições HTTP DELETE com um parâmetro de ID na rota
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            // Busca o veículo pelo ID
            var veiculo = await _context.Veiculos.FindAsync(id);

            // Verifica se o veículo existe
            if (veiculo == null)
                return NotFound("Veículo não encontrado."); // Retorna 404 Not Found se o veículo não for encontrado

            // Verifica se há tickets relacionados a este veículo
            var hasRelatedTickets = await _context.Tickets.AnyAsync(t => t.VeiculoId == id);
            if (hasRelatedTickets)
            {
                // Retorna 400 BadRequest se existirem tickets associados, impedindo a exclusão
                return BadRequest("Não é possível excluir o veículo, pois ele possui tickets no histórico.");
            }

            _context.Veiculos.Remove(veiculo); // Remove o veículo do contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            return NoContent(); // Retorna 204 NoContent, indicando que a exclusão foi bem-sucedida
        }

        /// <summary>
        /// Verifica se um veículo com o ID especificado existe no banco de dados.
        /// </summary>
        /// <param name="id">O ID do veículo a ser verificado.</param>
        /// <returns>True se o veículo existir, False caso contrário.</returns>
        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id); // Retorna verdadeiro se algum veículo com o ID existir
        }
    }
}
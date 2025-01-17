using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Models;
using ProjetoIntegrador.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoIntegrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FactorialService _factorialService;
        private readonly ILogger<FuncionarioController> _logger;

        public FuncionarioController(AppDbContext context, FactorialService factorialService, ILogger<FuncionarioController> logger)
        {
            _context = context;
            _factorialService = factorialService;
            _logger = logger;
        }

        // GET: api/Funcionario
        [HttpGet]
        public IActionResult GetFuncionarios()
        {
            _logger.LogInformation("Obtendo lista de funcionários.");
            var funcionarios = _context.Funcionarios.ToList();
            return Ok(funcionarios);
        }

        // GET: api/Funcionario/{id}
        [HttpGet("{id}")]
        public IActionResult GetFuncionarioById(int id)
        {
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                _logger.LogWarning($"Funcionário com ID {id} não encontrado.");
                return NotFound($"Funcionário com ID {id} não encontrado.");
            }

            _logger.LogInformation($"Funcionário com ID {id} encontrado.");
            return Ok(funcionario);
        }

        // POST: api/Funcionario
        [HttpPost]
        public IActionResult AddFuncionario(Funcionario funcionario)
        {
            if (funcionario == null)
            {
                _logger.LogWarning("Tentativa de adicionar funcionário com dados nulos.");
                return BadRequest("Os dados do funcionário são inválidos.");
            }

            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();
            _logger.LogInformation("Novo funcionário adicionado.");
            return Ok(funcionario);
        }

        // PUT: api/Funcionario/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateFuncionario(int id, Funcionario funcionarioAtualizado)
        {
            var funcionarioExistente = _context.Funcionarios.FirstOrDefault(f => f.Id == id);

            if (funcionarioExistente == null)
            {
                _logger.LogWarning($"Funcionário com ID {id} não encontrado para atualização.");
                return NotFound($"Funcionário com ID {id} não encontrado.");
            }

            funcionarioExistente.Nome = funcionarioAtualizado.Nome;
            funcionarioExistente.Cargo = funcionarioAtualizado.Cargo;
            // Adicione outros campos conforme necessário

            _context.SaveChanges();
            _logger.LogInformation($"Funcionário com ID {id} atualizado com sucesso.");
            return Ok(funcionarioExistente);
        }

        // DELETE: api/Funcionario/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteFuncionario(int id)
        {
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                _logger.LogWarning($"Funcionário com ID {id} não encontrado para exclusão.");
                return NotFound($"Funcionário com ID {id} não encontrado.");
            }

            _context.Funcionarios.Remove(funcionario);
            _context.SaveChanges();
            _logger.LogInformation($"Funcionário com ID {id} excluído com sucesso.");
            return NoContent();
        }

        // POST: api/Funcionario/integrar
        [HttpPost("integrar")]
        public async Task<IActionResult> IntegrarFuncionarios()
        {
            try
            {
                _logger.LogInformation("Iniciando integração com a API Factorial.");

                // Chama o serviço para buscar dados da API Factorial
                var funcionariosFactorial = await _factorialService.GetFuncionariosAsync();

                if (funcionariosFactorial == null || !funcionariosFactorial.Any())
                {
                    _logger.LogWarning("Nenhum funcionário recebido da API Factorial.");
                    return BadRequest("Nenhum funcionário recebido da API Factorial.");
                }

                int adicionados = 0, atualizados = 0;

                foreach (var funcionarioFactorial in funcionariosFactorial)
                {
                    var funcionarioExistente = _context.Funcionarios.FirstOrDefault(f => f.Id == funcionarioFactorial.Id);

                    if (funcionarioExistente != null)
                    {
                        // Atualizar funcionário existente
                        funcionarioExistente.Nome = funcionarioFactorial.Nome;
                        funcionarioExistente.Cargo = funcionarioFactorial.Cargo;
                        // Adicione outros campos conforme necessário
                        atualizados++;
                    }
                    else
                    {
                        // Adicionar novo funcionário
                        _context.Funcionarios.Add(funcionarioFactorial);
                        adicionados++;
                    }
                }

                _context.SaveChanges();
                _logger.LogInformation($"{adicionados} funcionário(s) adicionados e {atualizados} atualizado(s) com sucesso.");
                return Ok($"{adicionados} funcionário(s) adicionados e {atualizados} atualizado(s) com sucesso.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro durante a integração com a API Factorial.");
                return StatusCode(500, "Erro ao integrar funcionários.");
            }
        }
    }
}

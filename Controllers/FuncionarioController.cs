namespace ProjetoIntegrador.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ProjetoIntegrador.Data;
    using ProjetoIntegrador.Models;
    using System.Linq;

    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Funcionario
        [HttpGet]
        public IActionResult GetFuncionarios()
        {
            var funcionarios = _context.Funcionarios.ToList();
            return Ok(funcionarios);
        }

        // POST: api/Funcionario
        [HttpPost]
        public IActionResult AddFuncionario(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();
            return Ok(funcionario);
        }
    }

}

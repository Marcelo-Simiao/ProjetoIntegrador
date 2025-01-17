using Microsoft.AspNetCore.Mvc;

namespace ProjetoIntegrador.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Bem-vindo à API do Projeto Integrador! Aplicação em desenvolvimento!!!");
        }
    }
}

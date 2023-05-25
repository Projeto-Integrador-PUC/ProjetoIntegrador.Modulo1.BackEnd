using Microsoft.AspNetCore.Mvc;

namespace ProjetoIntegrador.Modulo1.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagamentoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok()
        }
    }
}

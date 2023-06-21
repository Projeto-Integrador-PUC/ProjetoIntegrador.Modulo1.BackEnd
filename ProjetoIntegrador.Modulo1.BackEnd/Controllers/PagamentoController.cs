using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentosService _pagamentosService;
        public PagamentoController(
            IPagamentosService pagamentosService)
        {
            _pagamentosService = pagamentosService;
        }
        
        [HttpGet("preco-e-prazo")]
        public async Task<IActionResult> ObterPrecoEPrazo([FromQuery] PrecoPrazoRequest request)
        {
            var precoPrazoResponse = await _pagamentosService.CalcularFrete(request);
            return Ok(precoPrazoResponse);
        }
    }
}

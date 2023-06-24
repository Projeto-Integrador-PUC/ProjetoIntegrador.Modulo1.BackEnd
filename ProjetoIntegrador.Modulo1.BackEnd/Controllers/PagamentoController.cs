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

        [HttpPost("qr-code")]
        public async Task<IActionResult> GerarQRCode([FromQuery] double valor)
        {
            try
            {
                var codigoPix = _pagamentosService.GerarPixCopiaCola(valor);
                var resposta = new Resposta<string>(codigoPix)
                {
                    Sucesso = true,
                    Mensagem = "Código Pix Copia e Cola obtido com sucesso."
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> RealizarVenda([FromBody] Venda venda)
        {
            try
            {
                var guidResumo = await _pagamentosService.RealizarVenda(venda);
                var resposta = new Resposta<Guid>(guidResumo)
                {
                    Sucesso = true,
                    Mensagem = "Compra realizada com sucesso."
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

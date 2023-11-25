using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Net;

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
        [ProducesResponseType(typeof(Resposta<CorreiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPrecoEPrazo([FromQuery] PrecoPrazoRequest request)
        {
            try
            {
                var precoPrazoResponse = await _pagamentosService.CalcularFrete(request);
                var resposta = new Resposta<CorreiosResponse>(precoPrazoResponse)
                {
                    Sucesso = true,
                    Mensagem = "Preço e prazo calculados com sucesso"
                };
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPost("qr-code")]
        [ProducesResponseType(typeof(Resposta<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GerarQRCode([FromQuery] double valor)
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
        [ProducesResponseType(typeof(Resposta<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpGet("resumo/{guid}")]
        [ProducesResponseType(typeof(Resposta<ResumoVenda>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterResumoVenda([FromRoute] Guid guid)
        {
            try
            {
                
                var resumo = await _pagamentosService.ObterResumoVenda(guid);
                var resposta = new Resposta<ResumoVenda>(resumo)
                {
                    Sucesso = true,
                    Mensagem = "Resumo da venda obtido com sucesso."
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

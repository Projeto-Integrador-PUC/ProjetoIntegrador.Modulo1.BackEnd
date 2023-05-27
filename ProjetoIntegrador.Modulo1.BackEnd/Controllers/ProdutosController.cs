using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using ProjetoIntegrador.Modulo1.BackEnd.Validators;

namespace ProjetoIntegrador.Modulo1.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;
        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Resposta), 201)]
        public async Task<IActionResult> AdicionarProduto(Produto produto)
        {
            try
            {
                var validacao = new ProdutoValidator();
                var resultadoValidacao = validacao.Validate(produto);

                if (!resultadoValidacao.IsValid)
                    return BadRequest(string.Join(", ", resultadoValidacao.Errors.Select(err => err.ErrorMessage)));

                var resultado = await _produtosService.AdicionarProduto(produto);

                if (!resultado)
                    return BadRequest("Não foi possível cadastrar o produto!");

                var resposta = new Resposta
                {
                    Sucesso = resultado,
                    Mensagem = "Produto cadastrado com sucesso"
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("categorias")]
        [ProducesResponseType(typeof(Resposta<IEnumerable<Categoria>>), 200)]
        public async Task<IActionResult> ObterCategorias()
        {
            try
            {
                var categorias = await _produtosService.ObterCategorias();
                var resposta = new Resposta<IEnumerable<Categoria>>(categorias)
                {
                    Sucesso = true,
                    Mensagem = "Categorias obtidas com sucesso!"
                };
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Resposta<IEnumerable<Produto>>), 200)]
        public async Task<IActionResult> ObterProdutos()
        {
            try
            {
                var produtos = await _produtosService.ObterProdutos();
                var resposta = new Resposta<IEnumerable<Produto>>(produtos)
                {
                    Sucesso = true,
                    Mensagem = "Produtos obtidos com sucesso!"
                };
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("destaques")]
        [ProducesResponseType(typeof(Resposta<IEnumerable<Produto>>), 200)]
        public async Task<IActionResult> ObterProdutosDestaque()
        {
            try
            {
                var produtos = await _produtosService.ObterProdutosDestaque();
                var resposta = new Resposta<IEnumerable<Produto>>(produtos)
                {
                    Sucesso = true,
                    Mensagem = "Produtos obtidos com sucesso!"
                };
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Resposta<Produto>), 200)]
        public async Task<IActionResult> ObterDetalhesDoProduto(int id)
        {
            try
            {
                var produto = await _produtosService.ObterDetalhesDoProduto(id);
                var resposta = new Resposta<Produto>(produto)
                {
                    Sucesso = true,
                    Mensagem = "Produto obtido com sucesso!"
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

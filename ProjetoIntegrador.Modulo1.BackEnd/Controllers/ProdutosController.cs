﻿using Microsoft.AspNetCore.Mvc;
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

                return Ok("Produto cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> ObterCategorias()
        {
            try
            {
                var categorias = await _produtosService.ObterCategorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
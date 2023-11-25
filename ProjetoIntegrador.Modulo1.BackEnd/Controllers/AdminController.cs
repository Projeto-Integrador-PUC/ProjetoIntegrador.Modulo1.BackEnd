using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        
        [HttpPost("login")]
        [ProducesResponseType(typeof(Resposta), 200)]
        public async Task<IActionResult> Login([FromBody] Credenciais credenciais)
        {
            try
            {
                if (credenciais is null || credenciais.Usuario is null || credenciais.Senha is null)
                    return BadRequest(new Resposta { Sucesso = false, Mensagem = "Usuário e senha devem ser preenchidos." });

                var autenticacao = await _adminService.Autenticar(credenciais.Usuario, credenciais.Senha);

                if (autenticacao is null)
                    return Unauthorized(new Resposta { Sucesso = false, Mensagem = "Usuário ou senha inválidos" });

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, autenticacao, new AuthenticationProperties());

                var resposta = new Resposta
                {
                    Sucesso = true,
                    Mensagem = "Login efetuado com sucesso!"
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

using Microsoft.AspNetCore.Authentication.Cookies;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using System.Security.Claims;

namespace ProjetoIntegrador.Modulo1.BackEnd.Servicos
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<ClaimsPrincipal?> Autenticar(string usuario, string senha)
        {
            var success = await _adminRepository.VerificarCredenciais(usuario, senha);

            if (!success)
                return default;

            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "admin") }, CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}

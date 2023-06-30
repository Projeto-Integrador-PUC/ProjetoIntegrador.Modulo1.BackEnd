using System.Security.Claims;

namespace ProjetoIntegrador.Modulo1.BackEnd.Interfaces
{
    public interface IAdminService
    {
        Task<ClaimsPrincipal?> Autenticar(string usuario, string senha);
    }
}
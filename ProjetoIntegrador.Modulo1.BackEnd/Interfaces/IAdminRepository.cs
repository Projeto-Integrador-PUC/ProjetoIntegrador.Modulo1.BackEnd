namespace ProjetoIntegrador.Modulo1.BackEnd.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> VerificarCredenciais(string usuario, string senha);
    }
}
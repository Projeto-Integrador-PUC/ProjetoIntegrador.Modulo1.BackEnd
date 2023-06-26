using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Interfaces
{
    public interface IPagamentosRepository
    {
        Task<bool> AdicionarCartaoCredito(CartaoCredito cartao, int idResumoVenda);
        Task<bool> AtualizarStatus(Enums.SituacaoPedido status, int idResumoVenda);
        Task<(int, Guid)> CriarResumoVenda(Venda venda);
        Task<bool> CriarVenda(Produto produtoVendido, int idResumoVenda);
        Task<IEnumerable<ResumoVenda>> ObterResumoVenda(Guid guidVenda);
    }
}
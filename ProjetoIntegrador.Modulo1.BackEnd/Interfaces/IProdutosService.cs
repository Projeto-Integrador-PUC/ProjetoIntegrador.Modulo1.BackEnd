using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Interfaces
{
    public interface IProdutosService
    {
        Task<bool> AdicionarProduto(Produto produto);
        Task<bool> AtualizarProduto(Produto produto);
        Task<IEnumerable<Produto>> ObterProdutos();
        Task<bool> RemoverProduto(int id);
        Task<IEnumerable<Categoria>> ObterCategorias();
        Task<IEnumerable<Produto>> ObterProdutosDestaque();
        Task<Produto> ObterDetalhesDoProduto(int id);
    }
}
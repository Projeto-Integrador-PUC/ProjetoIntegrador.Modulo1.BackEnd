using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Servicos
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _produtosRepository;
        public ProdutosService(IProdutosRepository produtosRepository)
        {
            _produtosRepository = produtosRepository;
        }

        public async Task<bool> AdicionarProduto(Produto produto)
        {
            return await _produtosRepository.AdicionarProduto(produto);
        }

        public async Task<bool> AtualizarProduto(Produto produto)
        {
            return await _produtosRepository.AtualizarProduto(produto);
        }

        public async Task<bool> RemoverProduto(int id)
        {
            return await _produtosRepository.RemoverProduto(id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutos()
        {
            return await _produtosRepository.ObterProdutos();
        }
        
        public async Task<IEnumerable<Categoria>> ObterCategorias()
        {
            return await _produtosRepository.ObterCategorias();
        }

        public async Task<IEnumerable<ProdutoPaginado>> ObterProdutosPaginado(int pagina, int quantidade)
        {
            return await _produtosRepository.ObterProdutosPaginado(pagina, quantidade);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosDestaque()
        {
            return await _produtosRepository.ObterProdutosDestaque();
        }

        public async Task<Produto> ObterDetalhesDoProduto(int id)
        {
            return await _produtosRepository.ObterDetalhesDoProduto(id);
        }
    }
}

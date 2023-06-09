﻿using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Interfaces
{
    public interface IProdutosRepository
    {
        Task<bool> AdicionarProduto(Produto produto);
        Task<bool> AtualizarProduto(Produto produto);
        Task<bool> RemoverProduto(int id);
        Task<IEnumerable<Produto>> ObterProdutos();
        Task<IEnumerable<Categoria>> ObterCategorias();
        Task<IEnumerable<Produto>> ObterProdutosDestaque();
        Task<Produto> ObterDetalhesDoProduto(int id);
        Task<bool> RetirarDoEstoque(int id, int quantidade);
        Task<IEnumerable<ProdutoPaginado>> ObterProdutosPaginado(int pagina, int quantidade);
    }
}
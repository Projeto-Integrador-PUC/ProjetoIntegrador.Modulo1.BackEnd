using Dapper;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Data.SqlClient;

namespace ProjetoIntegrador.Modulo1.BackEnd.Repositorios
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly string _stringDeConexao;
        public ProdutosRepository(DB dbConfig)
        {
            _stringDeConexao = dbConfig.ConnectionString;
        }

        public async Task<bool> AdicionarProduto(Produto produto)
        {
            using var conexao = new SqlConnection(_stringDeConexao);
            var parametros = new
            {
                nome = produto.Nome,
                descricao = produto.Descricao,
                preco = produto.Preco,
                quantidade_estoque = produto.Quantidade,
                categoria_id = produto.Categoria,
                produto_destaque = produto.Destaque,
                imagem_base64 = produto.Imagem
            };

            var sql = $@"
            INSERT INTO produto (nome, descricao, preco, quantidade_estoque, categoria_id, produto_destaque, imagem_base64)
            VALUES (@nome, @descricao, @preco, @quantidade_estoque, @categoria_id, @produto_destaque, @imagem_base64)
            ";

            var affectedRows = await conexao.ExecuteAsync(sql, parametros);

            if (affectedRows > 0)
                return true;

            return false;
        }

        public async Task<bool> AtualizarProduto(Produto produto)
        {
            using var conexao = new SqlConnection(_stringDeConexao);
            var parametros = new
            {
                id = produto.Id,
                nome = produto.Nome,
                descricao = produto.Descricao,
                preco = produto.Preco,
                quantidade_estoque = produto.Quantidade,
                categoria_id = produto.Categoria,
                produto_destaque = produto.Destaque,
                imagem_base64 = produto.Imagem
            };

            var sql = $@"
            UPDATE produto
            SET nome = @nome, 
                descricao = @descricao, 
                preco = @preco, 
                quantidade_estoque = @quantidade_estoque, 
                categoria_id = @categoria_id, 
                produto_destaque = @produto_destaque, 
                image_base64 = @imagem_base64
            WHERE id = @id
            ";

            var affectedRows = await conexao.ExecuteAsync(sql, parametros);

            if (affectedRows > 0)
                return true;

            return false;
        }

        public async Task<bool> RemoverProduto(int id)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = @"
            DELETE FROM produto 
            WHERE id = @id
            ";

            var affectedRows = await conexao.ExecuteAsync(sql, new { id });

            if (affectedRows > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<Produto>> ObterProdutos()
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = $@"
            SELECT
                p.id AS {nameof(Produto.Id)},
                p.nome AS {nameof(Produto.Nome)},
                p.descricao AS {nameof(Produto.Descricao)},
                p.preco AS {nameof(Produto.Preco)},
                p.quantidade_estoque AS {nameof(Produto.Quantidade)},
                p.categoria_id AS {nameof(Produto.Categoria)},
                c.nome AS {nameof(Produto.NomeCategoria)},
                p.produto_destaque AS {nameof(Produto.Destaque)},
                p.imagem_base64 AS {nameof(Produto.Imagem)}
            FROM produto p
            INNER JOIN categoria c ON c.id = p.categoria_id
            ";

            return await conexao.QueryAsync<Produto>(sql);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPaginado(int pagina, int quantidade)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = $@"
            SELECT
                p.id AS {nameof(Produto.Id)},
                p.nome AS {nameof(Produto.Nome)},
                p.descricao AS {nameof(Produto.Descricao)},
                p.preco AS {nameof(Produto.Preco)},
                p.quantidade_estoque AS {nameof(Produto.Quantidade)},
                p.categoria_id AS {nameof(Produto.Categoria)},
                c.nome AS {nameof(Produto.NomeCategoria)},
                p.produto_destaque AS {nameof(Produto.Destaque)},
                p.imagem_base64 AS {nameof(Produto.Imagem)}
            FROM produto p
            INNER JOIN categoria c ON c.id = p.categoria_id
            ORDER BY p.id
            OFFSET @offset ROWS
            FETCH NEXT @quantidade ROWS ONLY
            ";

            var offset = (pagina - 1) * quantidade;

            return await conexao.QueryAsync<Produto>(sql, new { offset, quantidade });
        }


        public async Task<IEnumerable<Produto>> ObterProdutosDestaque()
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = $@"
            SELECT
                id AS {nameof(Produto.Id)},
                nome AS {nameof(Produto.Nome)},
                descricao AS {nameof(Produto.Descricao)},
                preco AS {nameof(Produto.Preco)},
                categoria_id AS {nameof(Produto.Categoria)},
                imagem_base64 AS {nameof(Produto.Imagem)}
            FROM produto
            WHERE produto_destaque = 1
            ";

            return await conexao.QueryAsync<Produto>(sql);
        }

        public async Task<IEnumerable<Categoria>> ObterCategorias()
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = $@"
            SELECT
                id AS {nameof(Categoria.Id)}, 
                nome AS {nameof(Categoria.Nome)},
                imagem_base64 AS {nameof(Categoria.Imagem)}
            FROM categoria
            ";

            return await conexao.QueryAsync<Categoria>(sql);
        }

        public async Task<Produto> ObterDetalhesDoProduto(int id)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = $@"
            SELECT
                id AS {nameof(Produto.Id)},
                nome AS {nameof(Produto.Nome)},
                descricao AS {nameof(Produto.Descricao)},
                preco AS {nameof(Produto.Preco)},
                quantidade_estoque AS {nameof(Produto.Quantidade)},
                categoria_id AS {nameof(Produto.Categoria)},
                produto_destaque AS {nameof(Produto.Destaque)},
                imagem_base64 AS {nameof(Produto.Imagem)}
            FROM produto
            WHERE id = @id
            ";

            return await conexao.QueryFirstOrDefaultAsync<Produto>(sql, new { id });
        }

        public async Task<bool> RetirarDoEstoque(int id, int quantidade)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var sql = $@"
            UPDATE produto
            SET quantidade_estoque = quantidade_estoque - @quantidade
            WHERE id = @id
            ";

            var affectedRows = await conexao.ExecuteAsync(sql, new { id, quantidade });

            if (affectedRows > 0)
                return true;

            return false;
        }
    }
}

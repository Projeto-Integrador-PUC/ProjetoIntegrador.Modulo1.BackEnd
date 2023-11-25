using Dapper;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Data.SqlClient;
using StatusPedido = ProjetoIntegrador.Modulo1.BackEnd.Models.StatusPedido;

namespace ProjetoIntegrador.Modulo1.BackEnd.Repositorios
{
    public class PagamentosRepository : IPagamentosRepository
    {
        private readonly string _stringDeConexao;
        public PagamentosRepository(DB dbConfig)
        {
            _stringDeConexao = dbConfig.ConnectionString;
        }

        public async Task<(int, Guid)> CriarResumoVenda(Venda venda)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var parametros = new
            {
                id_pagamento = venda?.Pagamento?.Id,
                codigo_frete = venda?.Entrega?.TipoEntrega,
                nome = venda?.Envio?.Nome,
                sobrenome = venda?.Envio?.Sobrenome,
                endereco = venda?.Envio?.Logradouro,
                endereco2 = venda?.Envio?.Complemento ?? "",
                estado = venda?.Envio?.Estado,
                cidade = venda?.Envio?.Cidade,
                cep = venda?.Envio?.Cep,
                email = venda?.Envio?.Email,
                valor_frete = venda?.Entrega?.Valor,
                prazo_frete = venda?.Entrega?.Prazo,
            };

            var sql = $@"
            INSERT INTO resumo_venda (id_pagamento, id_frete, nome, sobrenome, endereco, endereco2, estado, cidade, cep, email, valor_frete, prazo_frete)
            OUTPUT INSERTED.id, INSERTED.guid_resumo
            VALUES (@id_pagamento, (SELECT id FROM tipo_frete WHERE codigo = @codigo_frete), @nome, @sobrenome, @endereco, @endereco2, @estado, @cidade, @cep, @email, @valor_frete, @prazo_frete)
            ";

            return await conexao.QuerySingleAsync<(int, Guid)>(sql, parametros);
        }

        public async Task<bool> CriarVenda(Produto produtoVendido, int idResumoVenda)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var parametros = new
            {
                id_resumo_venda = idResumoVenda,
                produto_id = produtoVendido.Id,
                quantidade_vendida = produtoVendido.QuantidadeSelecionada,
                preco_unitario_venda = produtoVendido.Preco,
                data_venda = DateTime.Now
            };

            var sql = $@"
            INSERT INTO venda (id_resumo_venda, produto_id, quantidade_vendida, preco_unitario_venda, data_venda)
            VALUES (@id_resumo_venda, @produto_id, @quantidade_vendida, @preco_unitario_venda, @data_venda)
            ";

            var linhasAfetadas = await conexao.ExecuteAsync(sql, parametros);

            return linhasAfetadas > 0;
        }

        public async Task<bool> AtualizarStatus(Enums.SituacaoPedido status, int idResumoVenda)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var parametros = new
            {
                id_tipo_status = status,
                id_resumo_venda = idResumoVenda,
                data = DateTime.Now,
            };

            var sql = @"
            INSERT INTO status_pedido (id_tipo_status, id_resumo_venda, data)
            VALUES (@id_tipo_status, @id_resumo_venda, @data)
            ";

            var linhasAfetadas = await conexao.ExecuteAsync(sql, parametros);

            return linhasAfetadas > 0;
        }

        public async Task<bool> AdicionarCartaoCredito(CartaoCredito cartao, int idResumoVenda)
        {
            using var conexao = new SqlConnection(_stringDeConexao);

            var parametros = new
            {
                id_resumo_venda = idResumoVenda,
                numero_cartao = cartao.NumeroCartao,
                vencimento = cartao.Vencimento,
                cvv = cartao.CVV,
                nome_titular = cartao.NomeTitular,
            };

            var sql = @"
            INSERT INTO cartao_credito (id_resumo_venda, numero_cartao, vencimento, cvv, nome_titular)
            VALUES (@id_resumo_venda, @numero_cartao, @vencimento, @cvv, @nome_titular)
            ";

            var linhasAfetadas = await conexao.ExecuteAsync(sql, parametros);

            return linhasAfetadas > 0;
        }

        public async Task<IEnumerable<ResumoVenda>> ObterResumoVenda(Guid guidVenda)
        {
            using var conexao = new SqlConnection(_stringDeConexao);
            
            var sql = $@"
            SELECT
                id_pagamento as {nameof(ResumoVenda.IdPagamento)},
                id_frete as {nameof(ResumoVenda.IdFrete)},
                prazo_frete as {nameof(ResumoVenda.PrazoFrete)},
                valor_frete as {nameof(ResumoVenda.ValorFrete)},
                SUM(preco_unitario_venda) OVER() as {nameof(ResumoVenda.SubTotal)},
                endereco as {nameof(ResumoVenda.Logradouro)},
                endereco2 as {nameof(ResumoVenda.Complemento)},
                estado as {nameof(ResumoVenda.Estado)},
                cidade as {nameof(ResumoVenda.Cidade)},
                cep as {nameof(ResumoVenda.Cep)},
                id_tipo_status as {nameof(StatusPedido.Situacao)},
                nome_status as {nameof(StatusPedido.Descricao)},
                SP.data as {nameof(StatusPedido.DataStatus)},
                P.id as {nameof(ProdutoVendido.IdProduto)},
                imagem_base64 as {nameof(ProdutoVendido.Imagem)},
                P.nome as {nameof(ProdutoVendido.Nome)},
                quantidade_vendida as {nameof(ProdutoVendido.Quantidade)},
                preco_unitario_venda as {nameof(ProdutoVendido.ValorUnitario)}
            FROM resumo_venda RV
            INNER JOIN venda V ON RV.id = V.id_resumo_venda
            INNER JOIN produto P on V.produto_id = P.id
            INNER JOIN status_pedido SP on RV.id = SP.id_resumo_venda
            INNER JOIN tipo_status TS on SP.id_tipo_status = TS.id
            WHERE RV.guid_resumo = @guidVenda
            ";

            var resumos = await conexao.QueryAsync<ResumoVenda, StatusPedido, ProdutoVendido, ResumoVenda>(
                sql,
                (resumoVenda, statusPedido, produtoVendido) =>
                {
                    resumoVenda.Status.Add(statusPedido);
                    resumoVenda.Produtos.Add(produtoVendido);
                    return resumoVenda;
                },
                splitOn: "Situacao, IdProduto",
                param: new { guidVenda }
                );

            return resumos;
        }
    }
}

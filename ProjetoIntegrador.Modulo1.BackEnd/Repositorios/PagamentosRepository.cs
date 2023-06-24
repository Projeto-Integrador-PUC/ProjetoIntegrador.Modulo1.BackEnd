using Dapper;
using ProjetoIntegrador.Modulo1.BackEnd.Enums;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Data.SqlClient;

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
                id_pagamento = venda.Pagamento.Id,
                codigo_frete = venda.Entrega.TipoEntrega,
                nome = venda.Envio.Nome,
                sobrenome = venda.Envio.Sobrenome,
                endereco = venda.Envio.Logradouro,
                endereco2 = venda.Envio.Complemento ?? "",
                estado = venda.Envio.Estado,
                cidade = venda.Envio.Cidade,
                cep = venda.Envio.Cep,
                email = venda.Envio.Email,
                valor_frete = venda.Entrega.Valor,
                prazo_frete = venda.Entrega.Prazo,
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

        public async Task<bool> AtualizarStatus(StatusPedido status, int idResumoVenda)
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
    }
}

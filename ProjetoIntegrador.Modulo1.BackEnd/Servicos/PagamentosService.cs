﻿using pix_payload_generator.net.Models.CobrancaModels;
using pix_payload_generator.net.Models.PayloadModels;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Globalization;
using System.Xml.Serialization;
using SituacaoPedido = ProjetoIntegrador.Modulo1.BackEnd.Enums.SituacaoPedido;

namespace ProjetoIntegrador.Modulo1.BackEnd.Servicos
{
    public class PagamentosService : IPagamentosService
    {
        private readonly string _chaveRecebedor = "4f282e7a-a914-487d-b6f8-053bb4d26f35";
        private readonly string _nomeRecebedor = "Only Babies Store";
        private readonly string _cidadeRecebedor = "Sao Paulo";

        private readonly IProdutosRepository _produtosRepository;
        private readonly IPagamentosRepository _pagamentosRepository;

        public PagamentosService(IProdutosRepository produtosRepository, IPagamentosRepository pagamentosRepository)
        {
            _produtosRepository = produtosRepository;
            _pagamentosRepository = pagamentosRepository;
        }
        public async Task<CorreiosResponse> CalcularFrete(PrecoPrazoRequest request)
        {
            var precoPrazoResponse = new CorreiosResponse();

            if (request.CodigoServicos is null || request.CepDestino is null || request.CepOrigem is null)
                return precoPrazoResponse;

            foreach (var servico in request.CodigoServicos)
            {
                var precoPrazo = await CalcularFrete(request.CepOrigem, request.CepDestino, request.Peso, request.Comprimento, request.Altura, request.Largura, request.Diametro, servico);
                precoPrazoResponse.Prazos.Add(precoPrazo.Prazos.First());
            }

            return precoPrazoResponse;
        }

        public string GerarPixCopiaCola(double valor)
        {
            var cobranca = new Cobranca(_chave: _chaveRecebedor)
            {
                Valor = new Valor
                {
                    Original = valor.ToString("F2", CultureInfo.InvariantCulture),
                }
            };

            return cobranca.ToPayload("OB", new Merchant(_nomeRecebedor, _cidadeRecebedor)).GenerateStringToQrCode();
        }
        
        public async Task<Guid> RealizarVenda(Venda venda)
        {
            var (id, guidResumo) = await _pagamentosRepository.CriarResumoVenda(venda);
            
            foreach (var produto in venda.Produtos)
            {
                if (produto.QuantidadeSelecionada is null) continue;
                await _produtosRepository.RetirarDoEstoque(produto.Id, (int)produto.QuantidadeSelecionada);
                await _pagamentosRepository.CriarVenda(produto, id);
            }

            await _pagamentosRepository.AtualizarStatus(SituacaoPedido.Realizado, id);

            if (venda.Pagamento is not null && venda.Pagamento.CartaoCredito is not null)
                await _pagamentosRepository.AdicionarCartaoCredito(venda.Pagamento.CartaoCredito, id);

            return guidResumo;
        }

        public async Task<ResumoVenda> ObterResumoVenda(Guid guid)
        {
            var resumos = await _pagamentosRepository.ObterResumoVenda(guid);

            var resumoVenda = resumos.FirstOrDefault();

            if (resumoVenda is null)
                return new ResumoVenda();
            
            resumoVenda.Status = resumos.SelectMany(resumo => resumo.Status).DistinctBy(status => status.Situacao).ToList();
            resumoVenda.Produtos = resumos.SelectMany(resumo => resumo.Produtos).DistinctBy(produto => produto.IdProduto).ToList();

            return resumoVenda;
        }
        
        private static async Task<CorreiosResponse> CalcularFrete(string cepOrigem, string cepDestino, double peso, double comprimento, double altura, double largura, double diametro, string codigoServico)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"http://ws.correios.com.br/calculador/CalcPrecoPrazo.aspx?sCepOrigem={cepOrigem}&sCepDestino={cepDestino}&nVlPeso={peso}&nCdFormato=1&nVlComprimento={comprimento}&nVlAltura={altura}&nVlLargura={largura}&nVlDiametro={diametro}&nCdServico={codigoServico}&nCdEmpresa=&sDsSenha=&sCdMaoPropria=n&nVlValorDeclarado=0&sCdAvisoRecebimento=n&StrRetorno=xml&nIndicaCalculo=3");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new XmlSerializer(typeof(CorreiosResponse));
            var precoPrazoResponse = serializer.Deserialize(new StringReader(result)) as CorreiosResponse;
            precoPrazoResponse ??= new CorreiosResponse { Prazos = new List<Prazo> { new Prazo { Erro = "1" } } };

            return precoPrazoResponse;
        }
    }
}

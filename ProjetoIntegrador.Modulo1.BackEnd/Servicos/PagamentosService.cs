using ProjetoIntegrador.Modulo1.BackEnd.Extensions;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Xml.Serialization;

namespace ProjetoIntegrador.Modulo1.BackEnd.Servicos
{
    public class PagamentosService : IPagamentosService
    {
        public async Task<CorreiosResponse> CalcularFrete(PrecoPrazoRequest request)
        {
            var precoPrazoResponse = new CorreiosResponse();
            foreach (var servico in request.CodigoServicos)
            {
                var precoPrazo = await CalcularFrete(request.CepOrigem, request.CepDestino, request.Peso, request.Comprimento, request.Altura, request.Largura, request.Diametro, servico);
                precoPrazoResponse.Prazos.Add(precoPrazo.Prazos.First());
            }

            return precoPrazoResponse;
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

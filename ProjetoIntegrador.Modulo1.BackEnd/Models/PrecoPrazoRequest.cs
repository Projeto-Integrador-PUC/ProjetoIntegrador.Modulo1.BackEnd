namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class PrecoPrazoRequest
    {
        public string CepOrigem { get; set; }
        public string CepDestino { get; set; }
        public double Peso { get; set; }
        public double Comprimento { get; set; }
        public double Altura { get; set; }
        public double Largura { get; set; }
        public double Diametro { get; set; }
        public string[] CodigoServicos { get; set; }
    }
}

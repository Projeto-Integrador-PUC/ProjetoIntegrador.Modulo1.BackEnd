namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class Venda
    {
        public Produto[] Produtos { get; set; } = Array.Empty<Produto>();
        public Entrega? Entrega { get; set; }
        public Envio? Envio { get; set; }
        public Pagamento? Pagamento { get; set; }
    }
}

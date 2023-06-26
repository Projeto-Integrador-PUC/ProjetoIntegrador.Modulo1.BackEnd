namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class ResumoVenda
    {
        public int IdPagamento { get; set; }
        public int IdFrete { get; set; }
        public int PrazoFrete { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal SubTotal { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public List<StatusPedido> Status { get; set; } = new();
        public List<ProdutoVendido> Produtos { get; set; } = new();
}
}

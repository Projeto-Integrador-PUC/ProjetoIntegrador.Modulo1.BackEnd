namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class ProdutoVendido
    {
        public int IdProduto { get; set; }
        public string? Imagem { get; set; }
        public string? Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}

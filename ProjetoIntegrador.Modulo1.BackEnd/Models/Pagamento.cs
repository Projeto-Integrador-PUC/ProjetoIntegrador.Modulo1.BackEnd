namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public CartaoCredito? CartaoCredito { get; set; }
    }
}

namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class StatusPedido
    {
        public Enums.SituacaoPedido Situacao { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataStatus { get; set; }
    }
}

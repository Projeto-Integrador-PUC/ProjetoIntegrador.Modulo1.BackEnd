namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class Resposta<T> : Resposta
    {
        public Resposta(T dados)
        {
            Dados = dados;
        }
        public T Dados { get; set; }

    }

    public class Resposta
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
    }
}

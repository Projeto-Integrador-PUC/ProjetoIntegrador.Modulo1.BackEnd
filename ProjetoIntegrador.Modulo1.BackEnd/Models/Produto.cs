using ProjetoIntegrador.Modulo1.BackEnd.Enums;

namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public bool Destaque { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public string? NomeCategoria { get; set; }
        public string Imagem { get; set; }

    }
}

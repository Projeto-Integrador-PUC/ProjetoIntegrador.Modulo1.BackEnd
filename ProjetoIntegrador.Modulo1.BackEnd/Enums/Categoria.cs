using System.ComponentModel;

namespace ProjetoIntegrador.Modulo1.BackEnd.Enums
{
    public enum CategoriaProduto
    {
        [Description("Alimentação")]
        Alimentacao = 1,

        [Description("Higiene e cuidados")]
        Higiene,

        [Description("Roupa e acessórios")]
        Roupa,

        [Description("Brinquedos e atividades")]
        Brinquedo,

        [Description("Segurança")]
        Seguranca
    }
}

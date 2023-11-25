using System.Xml.Serialization;

namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    [XmlRoot("Servicos")]
    public class CorreiosResponse
    {
        [XmlElement("cServico")]
        public List<Prazo> Prazos { get; set; } = new List<Prazo>();
    }

    public class Prazo
    {
        [XmlElement("Codigo")]
        public string? Codigo { get; set; }
        
        [XmlElement("Valor")]
        public string? Valor { get; set; }
        
        [XmlElement("PrazoEntrega")]
        public string? PrazoEntrega { get; set; }

        [XmlElement("ValorSemAdicionais")]
        public string? ValorSemAdicionais { get; set; }
        
        [XmlElement("ValorMaoPropria")]
        public string? ValorMaoPropria { get; set; }

        [XmlElement("ValorAvisoRecebimento")]
        public string? ValorAvisoRecebimento { get; set; }
        
        [XmlElement("ValorValorDeclarado")]
        public string? ValorValorDeclarado { get; set; }

        [XmlElement("EntregaDomiciliar")]
        public string? EntregaDomiciliar { get; set; }
        
        [XmlElement("EntregaSabado")]
        public string? EntregaSabado { get; set; }
        
        [XmlElement("Erro")]
        public string? Erro { get; set; }

        [XmlElement("MsgErro")]
        public string? MsgErro { get; set; }

    }
}

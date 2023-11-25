namespace ProjetoIntegrador.Modulo1.BackEnd.Models
{
    public class DB
    {
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? ServerAddress { get; set; }
        public string? Database { get; set; }
        public string ConnectionString => $"Server={ServerAddress};Database={Database};User Id={User};Password={Password};";
    }
}

using Dapper;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using System.Data.SqlClient;

namespace ProjetoIntegrador.Modulo1.BackEnd.Repositorios
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _stringDeConexao;
        public AdminRepository(DB dbConfig)
        {
            _stringDeConexao = dbConfig.ConnectionString;
        }

        public async Task<bool> VerificarCredenciais(string usuario, string senha)
        {
            using var conexao = new SqlConnection(_stringDeConexao);
            var sql = $@"
                SELECT CASE WHEN EXISTS (
                    SELECT 1
                    FROM administrador
                    WHERE usuario = @usuario
                    AND senha = HASHBYTES('SHA2_512', '{senha}')
                )
                THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT) END
            ";


            return await conexao.QueryFirstAsync<bool>(sql, new { usuario });
        }
    }
}

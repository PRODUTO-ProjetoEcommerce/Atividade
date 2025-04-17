using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");
    }
}

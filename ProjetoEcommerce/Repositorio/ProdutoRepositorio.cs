using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public void CadastrarProduto(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Insert into Produto (Nome, Descricao, Quantidade, Preco) values (@nome, @descricao, @quantidade, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var conexao = new MySqlConnection (_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update Produto set Nome = @nome, Descricao = @descricao, Quantidade = @quantidade, Preco = @preco where CodProd=@codigo", conexao);
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    cmd.Parameters.Add("@descricao",MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false;
            }
        }

        public IEnumerable <Produto> TodosProdutos()
        {
            List <Produto> ProdList = new List<Produto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from Produto", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach(DataRow dr in dt.Rows)
                {
                    ProdList.Add(
                        new Produto
                        {
                            CodProd = Convert.ToInt32(dr["CodProd"]),
                            Nome = ((string)dr["Nome"]),
                            Descricao = ((string)dr["Descricao"]),
                            Quantidade = Convert.ToInt32(dr["Quantidade"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                        }
                        );
                }
                return ProdList;
            }
        }

        public Produto ObterProduto (int codigo)
        {
            using (var conexao = new MySqlConnection (_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Produto where CodProd = @codigo", conexao);

                cmd.Parameters.AddWithValue("@codigo", codigo);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Produto produto = new Produto();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    produto.CodProd = Convert.ToInt32(dr["CodProd"]);
                    produto.Nome = (string)(dr["Nome"]);
                    produto.Descricao = (string)(dr["Descricao"]);
                    produto.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                    produto.Preco = Convert.ToDecimal(dr["Preco"]);
                }
                return produto;
            }
        }

        public void Excluir (int id)
        {
            using (var conexao = new MySqlConnection (_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from Produto where CodProd = @codigo", conexao);

                cmd.Parameters.AddWithValue("@codigo", id);
                int i = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}

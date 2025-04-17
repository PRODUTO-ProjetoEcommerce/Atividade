namespace ProjetoEcommerce.Models
{

    public class Cliente
    {
        //CRIANDO O ENCAPSULAMENTO DO OBJETO COM GET E SET
        public int CodCli { get; set; } //Acessores
        // Ao usar ?, você está explicitamente dizendo que a propriedade pode intencionalmente ter um valor nulo.
        public string? NomeCli { get; set; }
        public string? TelCli { get; set; }
        public string? EmailCli { get; set; }
        public List<Cliente>? ListaCliente { get; set; }

    }
}

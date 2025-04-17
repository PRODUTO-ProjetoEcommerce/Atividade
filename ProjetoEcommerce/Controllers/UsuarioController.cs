using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Repositorio;

// Define o nome e onde a classe LoginController está localizada. Namespaces ajudam a organizar o código e evitar conflitos de nomes.
namespace ProjetoEcommerce.Controllers
{
    // Criando a classe LoginController e herdando  da classe controller
    public class UsuarioController : Controller
    {
        /* Declara uma variável privada somente leitura do tipo UsuarioRepositorio chamada _usuarioRepositorio.
        O "readonly" indica que o valor desta variável só pode ser atribuído no construtor da classe.
        UsuarioRepositorio é uma classe do repositorio responsável por interagir com a camada de dados para gerenciar informações de usuários.*/
        private readonly UsuarioRepositorio _usuarioRepositorio;

        /*Define o construtor da classe LoginController. 
         Recebe uma instância de UsuarioRepositorio como parâmetro (injeção de dependência).*/
        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            // O construtor é chamado quando uma nova instância de LoginController é criada.
            _usuarioRepositorio = usuarioRepositorio;
        }


        /* Define uma action (método público) chamada Login que retorna um IActionResult.
         IActionResult é uma interface que representa o resultado de uma action.*/
        public IActionResult Login()
        {
            // retorna a página Login
            return View();
        }

        /* Define outra action chamada Login, mas desta vez ela responde a requisições HTTP POST ([HttpPost]).
        que recebe dois parâmetros do formulário enviado: email e senha (ambos do tipo string).*/

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            /* Chama o método ObterUsuario do _usuarioRepositorio, passando o email fornecido pelo usuário.
             Isso buscará um usuário no banco de dados com o email correspondente.*/

            var usuario = _usuarioRepositorio.ObterUsuario(email);
            // Verifica se um usuário foi encontrado for diferente de vazio e se a senha fornecida corresponde à senha do usuário encontrado.
            if (usuario != null && usuario.Senha == senha)
            {
                // Autenticação bem-sucedida
                // Redireciona o usuário para a action "Index" do Controller "Cliente".
                return RedirectToAction("Index", "Cliente");
            }
            /* Se a autenticação falhar (usuário não encontrado ou senha incorreta):
             Adiciona um erro ao ModelState. ModelState armazena o estado do modelo e erros de validação.
             O primeiro argumento ("") indica um erro
             O segundo argumento é a mensagem de erro que será exibida ao usuário.*/

            ModelState.AddModelError("", "Email ou senha inválidos.");
            //retorna view Login 
            return View();
        }
    }
}

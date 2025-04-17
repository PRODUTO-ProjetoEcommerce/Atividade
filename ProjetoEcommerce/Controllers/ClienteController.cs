using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        /* Declara uma variável privada somente leitura do tipo ClienteRepositorio chamada _clienteRepositorio.
         O "readonly" indica que o valor desta variável só pode ser atribuído no construtor da classe.
         ClienteRepositorio é uma classe do repositorio responsável por interagir com a camada de dados para gerenciar informações de usuários.*/
        private readonly ClienteRepositorio _clienteRepositorio;

        /* Define o construtor da classe LoginController.
        Recebe uma instância de UsuarioRepositorio como parâmetro (injeção de dependência)*/
        public ClienteController(ClienteRepositorio clienteRepositorio)
        {
            /* O construtor é chamado quando uma nova instância de LoginController é criada.*/
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            /* Retorna a View padrão associada a esta Action,
             passando como modelo a lista de todos os clientes obtida do repositório.*/
            return View(_clienteRepositorio.TodosClientes());
        }


        /* Action para exibir o formulário de cadastro de cliente (via Requisição GET)*/
        public IActionResult CadastrarCliente()
        {
            //retorna a Página
            return View();
        }

        // Action que recebe e processa os dados que serão enviados pelo formulário de cadastro de cliente (via Requisição POST)
        [HttpPost]
        public IActionResult CadastrarCliente(Cliente cliente)
        {

            /* O parâmetro 'cliente' recebe os dados enviados pelo formulário,
             que são automaticamente mapeados para as propriedades da classe Cliente.
             Chama o método no repositório para cadastrar o novo cliente no sistema.*/
            _clienteRepositorio.Cadastrar(cliente);

            //redireciona para pagina Index 'nameof(Index)' garante que o nome da Action seja usado corretamente,
            return RedirectToAction(nameof(Index));
        }

        /* Action para exibir o formulário de edição de um cliente específico (via Requisição GET)
         Este método recebe o 'id' do cliente a ser editado como parâmetro.*/
        public IActionResult EditarCliente(int id)
        {
            // Obtém o cliente específico do repositório usando o ID fornecido.
            var cliente = _clienteRepositorio.ObterCliente(id);

            // Verifica se o cliente foi encontrado. É uma boa prática tratar casos onde o ID é inválido.
            if (cliente == null)
            {
                // Você pode retornar um NotFound (código de status 404) ou outra resposta apropriada.
                return NotFound();
            }

            // Retorna a View associada a esta Action (EditarCliente.cshtml),
            return View(cliente);
        }


        // Carrega a liista de Cliente que envia a alteração(post)

        [HttpPost]
        [ValidateAntiForgeryToken] // Essencial para segurança contra ataques CSRF
        /*[Bind] para especificar explicitamente quais propriedades do objeto Cliente podem ser vinculadas a partir dos dados do formulário.
        Isso é uma boa prática de segurança para evitar o overposting (onde um usuário malicioso pode enviar dados para propriedades
        que você não pretendia que fossem alteradas)*/
        public IActionResult EditarCliente(int id, [Bind("CodCli, NomeCli,TelCli,EmailCli")] Cliente cliente)
        {
            // Verifica se o ID fornecido na rota corresponde ao ID do cliente no modelo.
            if (id != cliente.CodCli)
            {
                return BadRequest(); // Retorna um erro 400 se os IDs não corresponderem.
            }
            if (ModelState.IsValid)
            {
                //try /catch = tratamento de erros 
                try
                {
                    // Verifica se o cliente com o Codigo fornecido existe no repositório.
                    if (_clienteRepositorio.Atualizar(cliente))
                    {
                        //redireciona para a pagina index quando alterar
                        return RedirectToAction(nameof(Index));                    
                    }
                }
                catch (Exception)
                {
                    // Adiciona um erro ao ModelState para exibir na View.
                    ModelState.AddModelError("", "Ocorreu um erro ao Editar.");
                    // Retorna a View com o modelo para exibir a mensagem de erro e os dados do formulário.
                    return View(cliente);
                }
            }
            // Se o ModelState não for válido, retorna a View com os erros de validação.
            return View(cliente);
        }


        public IActionResult ExcluirCliente(int id)
        {
            // Obtém o cliente específico do repositório usando o Codigo fornecido.
            _clienteRepositorio.Excluir(id);
            // Retorna a View de confirmação de exclusão, passando o cliente como modelo.
            return RedirectToAction(nameof(Index));
        }
    }
}

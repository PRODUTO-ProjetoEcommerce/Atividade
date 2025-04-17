using Microsoft.AspNetCore.Mvc;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

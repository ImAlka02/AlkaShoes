using Microsoft.AspNetCore.Mvc;

namespace AlkaShoes.Areas.Admin.Controllers
{
    public class ProductosController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agregar()
        {
            return View();
        }

        public IActionResult Editar()
        {
            return View();
        }

        public IActionResult Eliminar()
        {
            return View();
        }

    }
}

using AlkaShoes.Areas.Admin.Models;
using AlkaShoes.Models.Entities;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AlkaShoes.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductosController : Controller
    {
		public Repo<Marca> RepoM { get; }
		public RepoProductos RepoP { get; }

		public ProductosController(Repo<Marca> repoM, RepoProductos repoP)
        {
			RepoM = repoM;
			RepoP = repoP;
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agregar()
        {
            AdminProductoViewModel vm = new();
            vm.Marcas = RepoM.GetAll().Select(x=> new MarcaModel
            {
                Id = x.Id,
                Nombre = x.NombreMarca
            }); 
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AdminProductoViewModel vm)
        {
            ModelState.Clear();

			if (vm.Archivo != null)
			{
				if (vm.Archivo.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("", "Solo se permiten archivos JPG");
				}

				if (vm.Archivo.Length > 500 * 1024)//500kb
				{
					ModelState.AddModelError("", "Solo se permiten archivos no mayores a 500Kb");
				}
			}

            if (ModelState.IsValid)
            {
                RepoP.Insert(vm);
            }

			vm.Marcas = RepoM.GetAll().Select(x => new MarcaModel
			{
				Id = x.Id,
				Nombre = x.NombreMarca
			});

			return View(vm);
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

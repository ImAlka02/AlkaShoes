using AlkaShoes.Areas.Admin.Models;
using AlkaShoes.Models.Entities;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace AlkaShoes.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    public class MarcasController : Controller
    {
        public Repo<Marca> ReposM { get; }
        public MarcasController(Repo<Marca> reposM)
        {
            ReposM = reposM;
        }

        public IActionResult Index()
        {
            AdminMarcasViewModel vm = new();
            vm.Marcas = ReposM.GetAll().Select(m => new MarcaModel
            {
                IdMarca = m.Id,
                NombreMarca = m.NombreMarca
            });

            return View(vm);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Marca m)
        {
            if (m != null)
            {
                if (m.NombreMarca != null)
                {
                    if (string.IsNullOrEmpty(m.NombreMarca))
                    {
                        ModelState.AddModelError("", "El nombre de la marca es obligatoria.");
                    }

                    if (m.NombreMarca.Length > 45)
                    {
                        ModelState.AddModelError("", "El nombre de la marca ha superado los caracteres permitidos.");
                    }

                    if (ReposM.GetAll().Any(x => x.NombreMarca == m.NombreMarca))
                    {
                        ModelState.AddModelError("", "Esta marca ya ha sido registrada.");
                    }

                    if (ModelState.IsValid)
                    {
                        ReposM.Insert(m);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ingrese el nombre de la marca.");

                }
            }
            else
            {
                ModelState.AddModelError("", "Ingrese el nombre de la marca.");
            }
            return View(m);
        }

        public IActionResult Editar(int id)
        {
            var marca = ReposM.Get(id);
            if (marca == null)
            {
                return RedirectToAction("Index");
            }
            return View(marca);
        }

        [HttpPost]
        public IActionResult Editar(Marca m)
        {
            var marcaBDD = ReposM.Get(m.Id);

            if(marcaBDD == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (string.IsNullOrEmpty(m.NombreMarca))
                {
                    ModelState.AddModelError("", "El nombre de la marca es obligatoria.");
                }

                if (m.NombreMarca.Length > 45)
                {
                    ModelState.AddModelError("", "El nombre de la marca ha superado los 45 caracteres permitidos.");
                }

                if (ReposM.GetAll().Any(x => x.NombreMarca.ToUpper() == m.NombreMarca.ToUpper()))
                {
                    ModelState.AddModelError("", "Esta marca ya ha sido registrada.");
                }

                if (ModelState.IsValid)
                {
                    marcaBDD.NombreMarca = m.NombreMarca;
                    ReposM.Update(marcaBDD);
                    return RedirectToAction("Index");
                }
            }
            return View(m);
        }

        public IActionResult Eliminar(int id)
        {
            var marca = ReposM.Get(id);
            if (marca == null)
            {
                return RedirectToAction("Index");
            }
            return View(marca);
        }

        [HttpPost]
        public IActionResult Eliminar(Marca m)
        {
            var marcaBDD = ReposM.Get(m.Id);
            if (marcaBDD == null)
            {
                return RedirectToAction("Index");
            }

            ReposM.Delete(marcaBDD);
            return RedirectToAction("Index");
        }

    }
}

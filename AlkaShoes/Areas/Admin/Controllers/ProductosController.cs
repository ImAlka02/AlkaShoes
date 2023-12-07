using AlkaShoes.Areas.Admin.Models;
using AlkaShoes.Models.Entities;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AlkaShoes.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductosController : Controller
    {
		public Repo<Marca> RepoM { get; }
		public RepoProductos RepoP { get; }
        public Repo<Talla> RepoT { get; }
        public RepoTallas RepoTallasProducto { get; }

        public ProductosController(Repo<Marca> repoM, RepoProductos repoP, Repo<Talla> repoT, RepoTallas repoTallas)
        {
			RepoM = repoM;
			RepoP = repoP;
            RepoT = repoT;

            RepoTallasProducto = repoTallas;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(AdminProductosViewModel vm)
        {
            vm.Marcas = RepoM.GetAll().Select(x=> new MarcaModel
            {
                IdMarca = x.Id,
                NombreMarca = x.NombreMarca
            });

            if(vm.IdMarcaSeleccionada==0)
            {
                vm.Productos = RepoP.GetAll().Select(p=> new ProductoModel
                {
                    IdProducto= p.Id,
                    NombreProducto = p.Nombre,
                    Sku = p.Sku
                });
            }
            else
            {
                vm.Productos = RepoP.ProductosXMarca(vm.IdMarcaSeleccionada).Select(p => new ProductoModel
                {
                    IdProducto = p.Id,
                    NombreProducto = p.Nombre,
                    Sku = p.Sku
                });
            }
            return View(vm);
        }


        public IActionResult Agregar()
        {
            AdminAgregarProductoViewModel vm = new();
            vm.Marcas = RepoM.GetAll().Select(x => new MarcaModel
            {
                IdMarca = x.Id,
                NombreMarca = x.NombreMarca
            });

            return View(vm);
		}

        [HttpPost]
        public IActionResult Agregar(AdminAgregarProductoViewModel vm)
        {
            ModelState.Clear();
            //Validaciones      
            if (string.IsNullOrEmpty(vm.Producto.Nombre))
            {
                ModelState.AddModelError("","El nombre del producto es obligatorio.");
            }
            if (vm.Producto.Nombre.Length>50)
            {
                ModelState.AddModelError("", "El nombre del producto ha superado los 50 caracteres permitidos.");
            }
            
            if (RepoP.GetAll().Any(x=>x.Nombre.ToUpper() ==vm.Producto.Nombre.ToUpper()))
            {
                ModelState.AddModelError("", "Ya se ha registrado un producto con este nombre.");
            }

            if (string.IsNullOrEmpty(vm.Producto.Sku))
            {
                ModelState.AddModelError("", "El SKU del producto es obligatorio.");
            }
            if (vm.Producto.Sku.Length >= 10)
            {
                ModelState.AddModelError("", "SKU del producto ha superado los 10 caracteres permitidos.");
            }
            
            if (RepoP.GetAll().Any(x => x.Sku.ToUpper() == vm.Producto.Sku.ToUpper()))
            {
                ModelState.AddModelError("", "Ya se ha registrado un producto con este SKU.");
            }

            if (vm.Producto.Precio <= 0)
            {
                ModelState.AddModelError("","El precio del producto debe ser mayor a 0.");
            }
            if (string.IsNullOrEmpty(vm.Producto.Descripcion))
            {
                ModelState.AddModelError("", "La descripción del producto es obligatorio.");
            }

            if (vm.Producto.IdMarca == 0)
            {
                ModelState.AddModelError("", "Selecciona una marca.");
            }

            if (vm.Archivo != null)
            {
                //Validar tipo de archivo:  MIMETYPE
                if (vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Solo se permiten archivos png.");
                }

                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten archivos no mayores a 500Kb");
                }
            }



            if(ModelState.IsValid)
            {
                RepoP.Insert(vm.Producto);

                if (vm.Archivo == null)
                {
                    System.IO.File.Copy("wwwroot/img_tenis/0.png", $"wwwroot/img_tenis/{vm.Producto.Id}.png");
                }
                else
                {
                    FileStream fs = System.IO.File.Create($"wwwroot/img_tenis/{vm.Producto.Id}.png");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Marcas = RepoM.GetAll().Select(x => new MarcaModel
            {
                IdMarca = x.Id,
                NombreMarca = x.NombreMarca
            });

            return View(vm);
        }


        public IActionResult Editar(int id)
        {
            AdminAgregarProductoViewModel vm = new();
            var producto = RepoP.GetById(id);
            if(producto == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                vm.Producto = producto;
                vm.Marcas = RepoM.GetAll().Select(m => new MarcaModel
                {
                    IdMarca = m.Id,
                    NombreMarca = m.NombreMarca
                });
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(AdminAgregarProductoViewModel vm)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(vm.Producto.Nombre))
            {
                ModelState.AddModelError("", "El nombre del producto es obligatorio.");
            }
            if (vm.Producto.Nombre.Length > 50)
            {
                ModelState.AddModelError("", "El nombre del producto ha superado los 50 caracteres permitidos.");
            }

            if (RepoP.GetAll().Any(x => x.Nombre.ToUpper() == vm.Producto.Nombre.ToUpper() && x.Id!=vm.Producto.Id))
            {
                ModelState.AddModelError("", "Ya se ha registrado un producto con este nombre.");
            }

            if (string.IsNullOrEmpty(vm.Producto.Sku))
            {
                ModelState.AddModelError("", "El SKU del producto es obligatorio.");
            }
            if (vm.Producto.Sku.Length >= 10)
            {
                ModelState.AddModelError("", "SKU del producto ha superado los 10 caracteres permitidos.");
            }

            if (RepoP.GetAll().Any(x => x.Sku.ToUpper() == vm.Producto.Sku.ToUpper() && x.Id != vm.Producto.Id))
            {
                ModelState.AddModelError("", "Ya se ha registrado un producto con este SKU.");
            }

            if (vm.Producto.Precio <= 0)
            {
                ModelState.AddModelError("", "El precio del producto debe ser mayor a 0.");
            }
            if (string.IsNullOrEmpty(vm.Producto.Descripcion))
            {
                ModelState.AddModelError("", "La descripción del producto es obligatorio.");
            }

            if (vm.Producto.IdMarca == 0)
            {
                ModelState.AddModelError("", "Selecciona una marca.");
            }

            if (vm.Archivo != null)
            {
                //Validar tipo de archivo:  MIMETYPE
                if (vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Solo se permiten archivos png.");
                }

                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten archivos no mayores a 500Kb");
                }
            }

            if (ModelState.IsValid)
            {
                var productoBDD = RepoP.Get(vm.Producto.Id);
                if (productoBDD == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    productoBDD.Nombre = vm.Producto.Nombre;
                    productoBDD.Sku = vm.Producto.Sku;
                    productoBDD.Precio = vm.Producto.Precio;
                    productoBDD.Descripcion = vm.Producto.Descripcion;
                    productoBDD.IdMarca = vm.Producto.IdMarca;

                    RepoP.Update(productoBDD);

                    if (vm.Archivo != null)
                    {
                        System.IO.FileStream fs = System.IO.File.Create($"wwwroot/img_tenis/{vm.Producto.Id}.png");
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                    return RedirectToAction("Index");
                }
            }

            vm.Marcas = RepoM.GetAll().Select(m => new MarcaModel
            {
                IdMarca = m.Id,
                NombreMarca = m.NombreMarca
            });

            return View(vm);
        }

        public IActionResult Eliminar(int id)
        {
            var producto = RepoP.GetById(id);

            if (producto == null)
            {
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpPost]
        public IActionResult Eliminar(Producto p)
        {
            var producto = RepoP.Get(p.Id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }

            RepoP.Delete(producto);

            var ruta = $"wwwroot/img_tenis/{p.Id}.png";

            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }

            return RedirectToAction("Index");
        }
    }
}

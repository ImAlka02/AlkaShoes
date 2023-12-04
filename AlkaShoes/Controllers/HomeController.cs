using AlkaShoes.Helpers;
using AlkaShoes.Models.Entities;
using AlkaShoes.Models.ViewModels;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;

namespace AlkaShoes.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepoProductos repoProducto;
        private readonly RepoTallas repoTallas;
        private readonly Repo<User> userRepo;
        private readonly Repo<Carrito> repoCarrito;

        public HomeController(RepoProductos repoProducto, RepoTallas repoTallas, Repo<User> userRepo, Repo<Carrito> repoCarrito)
        {
            this.repoProducto = repoProducto;
            this.repoTallas = repoTallas;
            this.userRepo = userRepo;
            this.repoCarrito = repoCarrito;
        }
        public IActionResult Index(string Id)
        {
            if (Id != null) 
            {
                Id = Id.Replace("-", " ");

                ProductoViewModel vm = new ProductoViewModel()
                {
                    Productos = repoProducto.ProductosXMarca(Id).Select(x => new ProductoModel()
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Precio = x.Precio,
                        FechaModificacion = new FileInfo($"wwwroot/img_tenis/{x.Id}.jpg").LastWriteTime.ToString("ddMMyyyyHHmm")
                    })
                };
                return View(vm);
            }
            else
            {
                ProductoViewModel vm = new ProductoViewModel()
                {
                    Productos = repoProducto.GetAll().Select(x => new ProductoModel()
                    {
                        Id= x.Id,
                        Nombre = x.Nombre,
                        Precio = x.Precio,
                        FechaModificacion = new FileInfo($"wwwroot/img_tenis/{x.Id}.jpg").LastWriteTime.ToString("ddMMyyyyHHmm")
                    })
                };
                return View(vm);
            }
        }
        public IActionResult Ver(string Id)
        {
            Id = Id.Replace("-", " ");
            var prop = repoProducto.GetByNombre(Id);
            VerProductoViewModel vm = new VerProductoViewModel()
            {
                Id = prop.Id,
                Sku = prop.Sku,
                Nombre = prop.Nombre,
                Descripcion = prop.Descripcion,
                FechaModificacion = new FileInfo($"wwwroot/img_tenis/{prop.Id}.jpg").LastWriteTime.ToString("ddMMyyyyHHmm"),
                Marca = prop.IdMarcaNavigation.NombreMarca,
                Precio = prop.Precio,
                Tallas = repoTallas.GetAllXTenis(Id).Select(x=> new TallasModel()
                {
                    Id = x.Id,
                    Talla = x.IdTallaNavigation.Talla1,
                    Cantidad = x.Cantidad
                })
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Ver(VerProductoViewModel vm)
        {
            var prop = repoProducto.Get(vm.Id);
            var Carrito = new Carrito()
            {
                IdProducto = vm.Id,
                Cantidad = vm.Cantidad,
                IdUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Fecha = DateTime.Now,
                PrecioCadaUno = prop.Precio
            };

            repoCarrito.Insert(Carrito);

             vm = new VerProductoViewModel()
            {
                Id = prop.Id,
                Sku = prop.Sku,
                Nombre = prop.Nombre,
                Descripcion = prop.Descripcion,
                FechaModificacion = new FileInfo($"wwwroot/img_tenis/{prop.Id}.jpg").LastWriteTime.ToString("ddMMyyyyHHmm"),
                Marca = prop.IdMarcaNavigation.NombreMarca,
                Precio = prop.Precio,
                Tallas = repoTallas.GetAllXTenis(prop.Nombre).Select(x => new TallasModel()
                {
                    Id = x.Id,
                    Talla = x.IdTallaNavigation.Talla1,
                    Cantidad = x.Cantidad
                })
            };
            return View(vm);
        }
        public IActionResult Carrito()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Correo))
            {
                ModelState.AddModelError("", "Escriba el correo del usuario.");
            }

            if (string.IsNullOrWhiteSpace(vm.Contraseña))
            {
                ModelState.AddModelError("", "Escriba la contraseña del usuario.");
            }

            if(ModelState.IsValid)
            {
                var user = userRepo.GetAll().FirstOrDefault(x=> x.Correo == vm.Correo 
                && x.Contraseña == Encriptacion.StringToSha512(vm.Contraseña));

                if(user == null)
                {
                    ModelState.AddModelError("", "La contraseña o correo son incorrectos.");
                } 
                else
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol == 1 ? "Admin" : "Supervisor"));
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true
                    });
                    if(user.Rol == 1)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(vm);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }
    }
}

using AlkaShoes.Helpers;
using AlkaShoes.Models.Entities;
using AlkaShoes.Models.ViewModels;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AlkaShoes.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly RepoProductos repoProducto;
        private readonly RepoTallas repoTallas;
        private readonly Repo<User> userRepo;
        private readonly RepoCarrito repoCarrito;

        public HomeController(RepoProductos repoProducto, RepoTallas repoTallas, Repo<User> userRepo, RepoCarrito repoCarrito)
        {
            this.repoProducto = repoProducto;
            this.repoTallas = repoTallas;
            this.userRepo = userRepo;
            this.repoCarrito = repoCarrito;
        }
        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult Inicio(string Id)
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
        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult Ver(string Id)
        {
            Id = Id.Replace("-", " ");
            var prop = repoProducto.GetByNombre(Id);
            if(prop != null)
            {
                VerProductoViewModel vm = new VerProductoViewModel()
                {
                    Id = prop.Id,
                    Sku = prop.Sku,
                    Nombre = prop.Nombre,
                    Descripcion = prop.Descripcion,
                    FechaModificacion = new FileInfo($"wwwroot/img_tenis/{prop.Id}.jpg").LastWriteTime.ToString("ddMMyyyyHHmm"),
                    Marca = prop.IdMarcaNavigation.NombreMarca,
                    Precio = prop.Precio,
                    Tallas = repoTallas.GetAllXTenis(Id).Select(x => new TallasModel()
                    {
                        Id = x.IdTallaNavigation.Id,
                        Talla = x.IdTallaNavigation.Talla1,
                        Cantidad = x.Cantidad
                    })
                };
                return View(vm);
            }
            return RedirectToAction("Inicio");
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult Ver(VerProductoViewModel vm)
        {
            var prop = repoProducto.GetById(vm.Id);
            var cantidadTalla = repoTallas.GetTallaByIdProducto(vm.Id, vm.IdTalla);
            if (prop != null)
            {
                if (cantidadTalla != null)
                {


                    ModelState.Clear();

                    if (vm.IdTalla == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Eliga una talla para continuar.");
                    }

                    if (vm.Cantidad == 0)
                    {
                        ModelState.AddModelError(string.Empty, "La cantidad no puede ser menor que 0.");
                    }

                    if (vm.Cantidad >= cantidadTalla?.Cantidad)
                    {
                        ModelState.AddModelError(string.Empty, "No tenemos la cantidad solicitada, elija otra por favor.");
                    }

                    var Carrito = new Carrito()
                    {
                        IdProducto = vm.Id,
                        Cantidad = vm.Cantidad,
                        IdUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"),
                        IdTalla = vm.IdTalla,
                        Fecha = DateTime.Now,
                        PrecioCadaUno = prop.Precio
                    };



                    if (ModelState.IsValid)
                    {
                        repoCarrito.Insert(Carrito);
                        cantidadTalla.Cantidad = cantidadTalla.Cantidad - vm.Cantidad;
                        repoTallas.Update(cantidadTalla);

                    }

                    vm.Id = prop.Id;
                    vm.Sku = prop.Sku;
                    vm.Nombre = prop.Nombre;
                    vm.Descripcion = prop.Descripcion;
                    vm.FechaModificacion = new FileInfo($"wwwroot/img_tenis/{prop.Id}.jpg").LastWriteTime.ToString("ddMMyyyyHHmm");
                    vm.Marca = prop.IdMarcaNavigation.NombreMarca;
                    vm.Precio = prop.Precio;
                    vm.Tallas = repoTallas.GetAllXTenis(prop.Nombre).Select(x => new TallasModel()
                    {
                        Id = x.Id,
                        Talla = x.IdTallaNavigation.Talla1,
                        Cantidad = x.Cantidad
                    });
                }
                return View(vm);
            }
            return View(vm);
        }
        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult Carrito()
        {

            CarritoViewModel vm = new CarritoViewModel()
            {
                ListaCompra = repoCarrito.GetAll().Where(x => x.IdUser == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)??"0"))
                .Select(x => new Carrito()
                {
                    IdProducto = x.IdProducto,
                    IdTalla = x.IdTalla,
                    IdUser = x.IdUser,
                    Cantidad = x.Cantidad,
                    Fecha = x.Fecha,
                    PrecioCadaUno = x.PrecioCadaUno,
                    IdProductoNavigation = x.IdProductoNavigation,
                    IdTallaNavigation = x.IdTallaNavigation,
                    IdUserNavigation = x.IdUserNavigation,
                    Id = x.Id
                })
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult Carrito(CarritoViewModel vm)
        {
            
            var prop = repoCarrito.GetById(vm.IdSeleccion);
            var par = repoProducto.GetById(prop.IdProducto);
            var cantidadTalla = repoTallas.GetTallaByIdProducto(par.Id, prop.IdTallaNavigation.Id );
            cantidadTalla.Cantidad = cantidadTalla.Cantidad + prop.Cantidad;
            repoTallas.Update(cantidadTalla);
            repoCarrito.Delete(vm.IdSeleccion);
            
            //par. = prop.Cantidad + vm.Cantidad;
            //repoTallas.Update(cantidadTalla);

            vm.ListaCompra = repoCarrito.GetAll().Where(x => x.IdUser == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"))
            .Select(x => new Carrito()
            {
                IdProducto = x.IdProducto,
                IdTalla = x.IdTalla,
                IdUser = x.IdUser,
                Cantidad = x.Cantidad,
                Fecha = x.Fecha,
                PrecioCadaUno = x.PrecioCadaUno,
                IdProductoNavigation = x.IdProductoNavigation,
                IdTallaNavigation = x.IdTallaNavigation,
                IdUserNavigation = x.IdUserNavigation,
                Id = x.Id
            });
            
            return View(vm);
        }
        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult CompraRealizada()
        {
            var compras = repoCarrito.GetAll().Where(x => x.IdUser == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"));

            return View();
        }

        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginViewModel vm)
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
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol == 1 ? "Admin" : "Cliente"));
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true
                    });
                    if(user.Rol == 1)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    return RedirectToAction("Inicio", "Home");
                }
            }
            return View(vm);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Denied()
        {
            HttpContext.SignOutAsync();
            return View();
        }

       
    }
}

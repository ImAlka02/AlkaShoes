﻿using AlkaShoes.Areas.Admin.Models;
using AlkaShoes.Models.Entities;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlkaShoes.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TallasController : Controller
    {
        public Repo<Marca> RepoM { get; }
        public RepoProductos RepoP { get; }
        public Repo<Talla> RepoT { get; }
        public RepoTallas RepoTallasProducto { get; }
        public TallasController(Repo<Marca> repoM, RepoProductos repoP, Repo<Talla> repoT, RepoTallas repoTallas)
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
            vm.Marcas = RepoM.GetAll().Select(x => new MarcaModel
            {
                IdMarca = x.Id,
                NombreMarca = x.NombreMarca
            });

            if (vm.IdMarcaSeleccionada == 0)
            {
                vm.Productos = RepoP.GetAll().Select(p => new ProductoModel
                {
                    IdProducto = p.Id,
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

        public IActionResult Editar(int id) 
        {
            AdminTallasViewModel vm = new()
            {
                IdProducto = id,
                TallasDisponibles = RepoT.GetAll().Select(x => new TallasModel()
                {
                    Id = x.Id,
                    Nombre = x.Talla1
                }),
                TallasDelProducto = RepoTallasProducto.GetTallasByIdProducto(id),
                
            };
            
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(AdminTallasViewModel vm)
        {
            var prop = RepoTallasProducto.GetTallaByIdProducto(vm.IdProducto, vm.tallaProducto.IdTalla);
            var tallas = RepoTallasProducto.GetTallasByIdProducto(vm.IdProducto);
            if (tallas.Any(x=>x.IdTalla ==vm.tallaProducto.IdTalla))
            {
                prop.Cantidad = vm.tallaProducto.Cantidad;
                
                RepoTallasProducto.Update(prop);
            }
            else
            {
                vm.tallaProducto.IdProducto = vm.IdProducto;
                RepoTallasProducto.Insert(vm.tallaProducto);
            }
            
            vm.TallasDisponibles = RepoT.GetAll().Select(x => new TallasModel()
            {
                Id = x.Id,
                Nombre = x.Talla1
            });
            vm.TallasDelProducto = tallas;

            return View(vm);
        }
    }
}
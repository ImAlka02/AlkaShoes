using AlkaShoes.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlkaShoes.Repositories
{
    public class RepoProductos : Repo<Producto>
    {
        public RepoProductos(AlkashoesContext context) : base(context)
        {
            
        }
        public Producto? GetById(int id)
        {
            return Context.Producto.Include(x=> x.IdMarcaNavigation).FirstOrDefault(x=> x.Id == id);
        }
        public Producto? GetByNombre (string Nombre)
        {
            return Context.Producto.Include(x => x.IdMarcaNavigation).FirstOrDefault(x => x.Nombre == Nombre);
        }
        public IEnumerable<Producto> ProductosXMarca (string Categoria)
        {
            return Context.Producto.Include(x=>x.IdMarcaNavigation)
                .Where(x=> x.IdMarcaNavigation != null && x.IdMarcaNavigation.NombreMarca == Categoria)
                .OrderBy(x => x.Nombre);
        }
        public IEnumerable<Producto> ProductosXMarca(int marca)
        {
            return Context.Producto.Include(x => x.IdMarcaNavigation)
                .Where(x => x.IdMarcaNavigation != null && x.IdMarcaNavigation.Id == marca)
                .OrderBy(x => x.Nombre);
        }
    }
}

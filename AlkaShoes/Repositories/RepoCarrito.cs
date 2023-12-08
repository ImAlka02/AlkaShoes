using AlkaShoes.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AlkaShoes.Repositories
{
    public class RepoCarrito : Repo<Carrito>
    {
        public RepoCarrito(AlkashoesContext context) : base(context)
        {
            
        }
        public Carrito? GetById( int id)
        {
            return Context.Carrito.Include(x=>x.IdProductoNavigation)
                .Include(x=>x.IdTallaNavigation).First(x=>x.Id == id);
        }
        public override IEnumerable<Carrito> GetAll()
        {
            return Context.Carrito.Include(x => x.IdTallaNavigation)
                .Include(x => x.IdProductoNavigation)
                .OrderBy(x => x.Fecha);
        }
        public void DeleteAll(List<Carrito> Lista)
        {
            if(Lista.Count()!=0)
            {
                Context.RemoveRange(Lista);
                Context.SaveChanges();
            }
        }
    }
}

﻿using AlkaShoes.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace AlkaShoes.Repositories
{
    public class RepoTallas : Repo<Tallasproducto>
    {
        public RepoTallas(AlkashoesContext context):base(context)
        {
            
        }

        public IEnumerable<Tallasproducto> GetAllXTenis(string Nombre)
        {
            return Context.Tallasproducto.Include(x=>x.IdTallaNavigation)
                .Where(x=> x.IdProductoNavigation.Nombre ==  Nombre);
        }
    }
}
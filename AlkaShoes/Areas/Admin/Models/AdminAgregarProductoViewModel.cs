using AlkaShoes.Models.Entities;
using AlkaShoes.Models.ViewModels;
using Microsoft.Extensions.FileProviders;

namespace AlkaShoes.Areas.Admin.Models
{
    public class AdminAgregarProductoViewModel
    {
        public Producto Producto { get; set; } = new();
        public IEnumerable<MarcaModel> Marcas { get; set; } = null!;
        public IFormFile Archivo { get; set; } = null!;
       
    }

}


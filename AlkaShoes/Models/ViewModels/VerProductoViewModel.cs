using AlkaShoes.Models.Entities;

namespace AlkaShoes.Models.ViewModels
{
    public class VerProductoViewModel
    {
        public int Id { get; set; }

        public string Sku { get; set; } = null!;
        public string Marca { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public string Descripcion { get; set; } = null!;
        public string FechaModificacion { get; set; } = null!;
        public int IdTalla { get; set; }
        public int Cantidad { get; set; }

        public IEnumerable<TallasModel> Tallas { get; set; }

    }

    public class TallasModel
    {
        public int Id { get; set; }
        public string Talla { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}

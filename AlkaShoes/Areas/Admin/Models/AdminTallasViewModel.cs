using AlkaShoes.Models.Entities;

namespace AlkaShoes.Areas.Admin.Models
{
    public class AdminTallasViewModel
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public Tallasproducto tallaProducto { get; set; } = null!;
        public IEnumerable<TallasModel> TallasDisponibles { get; set; } = null!;
        public IEnumerable<Tallasproducto>? TallasDelProducto { get; set; } 
    }

    public class TallasModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}

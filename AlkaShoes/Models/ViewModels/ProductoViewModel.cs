namespace AlkaShoes.Models.ViewModels
{
    public class ProductoViewModel
    {
        public IEnumerable<ProductoModel> Productos { get; set; } = null!;
    }

    public class ProductoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public string FechaModificacion { get; set; } = null!;
    }
}

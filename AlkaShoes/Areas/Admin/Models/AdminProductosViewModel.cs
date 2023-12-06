namespace AlkaShoes.Areas.Admin.Models
{
	public class AdminProductosViewModel
	{
		public int IdMarcaSeleccionada { get; set; }
		public IEnumerable<ProductoModel> Productos { get; set; } = null!;
		public IEnumerable<MarcaModel> Marcas { get; set; } = null!;
	}

	public class ProductoModel
	{
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
		public string Sku { get; set; } = null!;
    }

	public class MarcaModel
	{
		public int IdMarca { get; set; }
		public string NombreMarca { get; set; } = null!;
	}
}

using AlkaShoes.Models.Entities;

namespace AlkaShoes.Areas.Admin.Models
{
	public class AdminProductoViewModel
	{
		public string Sku { get; set; } = null!;

		public string Nombre { get; set; } = null!;

		public decimal Precio { get; set; }

		public string Descripcion { get; set; } = null!;
		public IFormFile? Archivo { get; set; }

		public IEnumerable<MarcaModel> Marcas { get; set; } = null!;

	}

	public class MarcaModel
	{
        public int Id { get; set; }
		public string Nombre { get; set; } = null!;
    }
}

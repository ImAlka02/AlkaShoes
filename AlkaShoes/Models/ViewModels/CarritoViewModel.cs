using AlkaShoes.Models.Entities;

namespace AlkaShoes.Models.ViewModels
{
    public class CarritoViewModel
    {
        public int IdSeleccion { get; set; } = 0;
        public IEnumerable<Carrito>? ListaCompra {  get; set; } 

    }
}

using System;
using System.Collections.Generic;

namespace AlkaShoes.Models.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdMarca { get; set; }

    public virtual ICollection<Carrito> Carrito { get; } = new List<Carrito>();

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual ICollection<Tallasproducto> Tallasproducto { get; } = new List<Tallasproducto>();
}

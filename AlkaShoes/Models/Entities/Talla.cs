using System;
using System.Collections.Generic;

namespace AlkaShoes.Models.Entities;

public partial class Talla
{
    public int Id { get; set; }

    public string Talla1 { get; set; } = null!;

    public virtual ICollection<Carrito> Carrito { get; } = new List<Carrito>();

    public virtual ICollection<Tallasproducto> Tallasproducto { get; } = new List<Tallasproducto>();
}

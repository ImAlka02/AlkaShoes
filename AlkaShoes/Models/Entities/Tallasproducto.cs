using System;
using System.Collections.Generic;

namespace AlkaShoes.Models.Entities;

public partial class Tallasproducto
{
    public int Id { get; set; }

    public int IdProducto { get; set; }

    public int IdTalla { get; set; }

    public int Cantidad { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Talla IdTallaNavigation { get; set; } = null!;
}

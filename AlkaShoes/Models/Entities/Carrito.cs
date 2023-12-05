using System;
using System.Collections.Generic;

namespace AlkaShoes.Models.Entities;

public partial class Carrito
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdProducto { get; set; }

    public decimal PrecioCadaUno { get; set; }

    public int Cantidad { get; set; }

    public DateTime? Fecha { get; set; }

    public int IdTalla { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Talla IdTallaNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}

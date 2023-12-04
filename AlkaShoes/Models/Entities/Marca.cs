using System;
using System.Collections.Generic;

namespace AlkaShoes.Models.Entities;

public partial class Marca
{
    public int Id { get; set; }

    public string NombreMarca { get; set; } = null!;

    public virtual ICollection<Producto> Producto { get; } = new List<Producto>();
}

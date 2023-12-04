using System;
using System.Collections.Generic;

namespace AlkaShoes.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int Rol { get; set; }

    public virtual ICollection<Carrito> Carrito { get; } = new List<Carrito>();
}

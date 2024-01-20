using System;
using System.Collections.Generic;

namespace Web_Service_.Net_Core.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    // public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

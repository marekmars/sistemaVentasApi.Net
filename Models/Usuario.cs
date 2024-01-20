using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Service_.Net_Core.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int IdRol { get; set; }
    [ForeignKey("IdRol")]
    public Rol Rol { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Dni { get; set; } = null!;

    // public virtual Rol IdRolNavigation { get; set; } = null!;
}

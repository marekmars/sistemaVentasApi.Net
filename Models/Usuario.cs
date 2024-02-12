using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Service_.Net_Core.Models;

public partial class Usuario
{
    public long Id { get; set; }

    public int IdRol { get; set; }
    [ForeignKey("IdRol")]
    public Rol Rol { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;
    public bool Estado { get; set; } = true;

    public string Dni { get; set; } = null!;

    

}

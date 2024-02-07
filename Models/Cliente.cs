using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Service_.Net_Core.Models;

public partial class Cliente
{

    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string Correo { get; set; } = null!;
    public bool Estado { get; set; } = true;



    // public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}

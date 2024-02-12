using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Venta
{
    public long Id { get; set; }

    public long IdCliente { get; set; }

    [ForeignKey(nameof(IdCliente))]
    public Cliente? Cliente { get; set; }
    public DateTime Fecha { get; set; }

    public decimal? Total { get; set; }
   

    public bool Estado {get;set;}
    
    public virtual List<Concepto> Conceptos { get; set; } = new List<Concepto>();


    // public Cliente IdClienteNavigation { get; set; } = null!;
}

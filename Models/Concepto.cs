using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Concepto
{
    public long Id { get; set; }

    public long IdVenta { get; set; }
    [JsonIgnore]
    [ForeignKey("IdVenta")]
    public Venta? Venta { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Importe { get; set; }

    public long IdProducto { get; set; }

    [ForeignKey("IdProducto")]
    public Producto? Producto { get; set; }
    public bool Estado {get;set;}



    // public virtual Venta IdVentaNavigation { get; set; } = null!;
}

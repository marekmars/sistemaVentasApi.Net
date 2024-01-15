using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Concepto
{
    public long Id { get; set; }

    public long IdVenta { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Importe { get; set; }

    public long IdProducto { get; set; }
 [JsonIgnore]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
 [JsonIgnore]
    public virtual Venta IdVentaNavigation { get; set; } = null!;
}

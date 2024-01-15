using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Producto
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal PrecioUnitario { get; set; }

    public decimal Costo { get; set; }
 [JsonIgnore]
    public virtual ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();
}

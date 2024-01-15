using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Venta
{
    public long Id { get; set; }

    public long IdCliente { get; set; }

    public DateTime Fecha { get; set; }

    public decimal? Total { get; set; }
    [JsonIgnore]
    public virtual ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();
    [JsonIgnore]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}

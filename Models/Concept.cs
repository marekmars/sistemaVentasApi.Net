using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Concept
{
    public long Id { get; set; }

    public long IdSale { get; set; }

    public int Quantity { get; set; }

    public decimal UnitaryPrice { get; set; }

    public decimal Import { get; set; }

    public long IdProduct { get; set; }

    public byte State { get; set; }
    [ForeignKey("IdProduct")]
    public virtual Product? Product { get; set; } = null!;
    [JsonIgnore]
    [ForeignKey("IdSale")]
    public virtual Sale? Sale { get; set; } = null!;
}

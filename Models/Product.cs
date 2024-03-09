using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web_Service_.Net_Core.Models;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal UnitaryPrice { get; set; }

    public decimal Cost { get; set; }

    public int Stock { get; set; }

    public byte State { get; set; }

    public string? Description { get; set; }

    
    public List<Image>? Images { get; set; }

}

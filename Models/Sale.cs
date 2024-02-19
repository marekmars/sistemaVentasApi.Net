using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Service_.Net_Core.Models;

public partial class Sale
{
    public long Id { get; set; }

    public long IdClient { get; set; }

    public DateTime Date { get; set; }

    public decimal? Total { get; set; }

    public byte State { get; set; }

    public virtual List<Concept> Concepts { get; set; } = new();
    [ForeignKey(nameof(IdClient))]
    public virtual Client? Client { get; set; } = null!;
}

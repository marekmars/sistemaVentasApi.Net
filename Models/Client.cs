using System;
using System.Collections.Generic;

namespace Web_Service_.Net_Core.Models;

public partial class Client
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string IdCard { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public byte State { get; set; }

}

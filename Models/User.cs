using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Service_.Net_Core.Models;

public partial class User
{
    public long Id { get; set; }

    public int IdRole { get; set; }

    public string Mail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string IdCard { get; set; } = null!;

    public byte State { get; set; }
    [ForeignKey("IdRole")]
    public virtual Role? Role { get; set; } = null!;

    public Image? Avatar { get; set; }
}

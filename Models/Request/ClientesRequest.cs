using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class ClientesRequest
    {
     public long Id { get; set; }

    public string Nombre { get; set; }= null!;

    public string Apellido { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string Correo { get; set; } = null!;   
    }
}
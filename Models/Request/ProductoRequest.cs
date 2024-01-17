using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class ProductoRequest
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal PrecioUnitario { get; set; }

        public decimal Costo { get; set; }

        public int Stock { get; set; }
    }
}
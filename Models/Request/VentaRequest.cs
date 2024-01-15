using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class VentaRequest
    {
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public List<ConceptoRequest> Conceptos { get; set; }

        public VentaRequest()
        {
            this.Conceptos = new List<ConceptoRequest>();
        }
    }
    public class ConceptoRequest
    {
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Importe { get; set; }

        public long IdProducto { get; set; }
    }
}
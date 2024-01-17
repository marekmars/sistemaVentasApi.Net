using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class VentaRequest
    {
        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "IdCliente debe ser mayor a 0")]
        [ExisteCliente(ErrorMessage = "El cliente no existe")]
        public long IdCliente { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe agregar por lo menos un concepto")]
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
    #region Validaciones
    public class ExisteCliente : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            long IdCliente = (long)value;
            using (DBContext db = new())
            {
                if (db.Clientes.Find(IdCliente) == null) return false;
            };
 
            return true;
        }
    }

    #endregion
}
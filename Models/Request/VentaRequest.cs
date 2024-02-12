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
        [Key]
        public long Id { get; set; }
        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "IdCliente debe ser mayor a 0")]
        [ExisteCliente(ErrorMessage = "El cliente no existe")]
        public long IdCliente { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe agregar por lo menos un concepto")]
        public List<ConceptoRequest> Conceptos { get; set; } = [];
        public bool Estado { get; set; }
    }
    public class ConceptoRequest
    {
        [Range(1, Double.MaxValue, ErrorMessage = "la cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El importe debe ser mayor a 0")]
        public decimal Importe { get; set; }
        [Required]
        [ExisteProducto(ErrorMessage = "El producto no existe")]
        [Range(1, Double.MaxValue, ErrorMessage = "IdProducto debe ser mayor a 0")]
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
    public class ExisteProducto : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            long IdProducto = (long)value;
            using (DBContext db = new())
            {
                if (db.Productos.Find(IdProducto) == null) return false;
            };

            return true;
        }
    }

    #endregion
}
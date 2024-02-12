using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class ProductoRequest
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tenes un minimo de 3 caracteres")]
        public string Nombre { get; set; } = null!;
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El precio unitario debe ser mayor a 0")]

        public decimal PrecioUnitario { get; set; }
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El costo debe ser mayor a 0")]
        public decimal Costo { get; set; }
        [RegularExpression(@"^[0-9]\d*$", ErrorMessage = "El stock debe ser un número entero no negativo")]
        public int Stock { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
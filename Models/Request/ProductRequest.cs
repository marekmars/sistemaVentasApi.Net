using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class ProductRequest
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tenes un minimo de 3 caracteres")]
        public string Name { get; set; } = null!;
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El precio unitario debe ser mayor a 0")]

        public decimal UnitaryPrice { get; set; }
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El costo debe ser mayor a 0")]
        public decimal Cost { get; set; }
        [RegularExpression(@"^[0-9]\d*$", ErrorMessage = "El stock debe ser un número entero no negativo")]
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

    }
}
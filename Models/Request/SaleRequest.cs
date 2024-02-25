using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class SaleRequest
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "IdCliente debe ser mayor a 0")]
        [ExisteCliente(ErrorMessage = "El cliente no existe")]
        public long IdClient { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Debe agregar por lo menos un concepto")]

        public List<ConceptRequest> Concepts { get; set; } = [];
        public DateTime? Date { get; set; }
        public decimal? Total { get; set; }

    }
    public class ConceptRequest
    {
        [Range(1, Double.MaxValue, ErrorMessage = "Id debe ser mayor a 0")]
        public long? Id { get; set; }
        [Range(1, Double.MaxValue, ErrorMessage = "la cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal UnitaryPrice { get; set; }
        [Required]
        [Range(0, 9999999999999999.99, ErrorMessage = "El importe debe ser mayor a 0")]
        public decimal Import { get; set; }
        [Required]
        [ExisteProducto(ErrorMessage = $"El producto no existe o esta fuera de stock")]
        [Range(1, Double.MaxValue, ErrorMessage = "IdProducto debe ser mayor a 0")]
        public long IdProduct { get; set; }
        public byte State { get; set; }
    }
    #region Validaciones
    public class ExisteCliente : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            long IdClient = (long)value;
            var context = validationContext.GetService<DataContext>();

            if (context.Clients.Find(IdClient) == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

    }
    public class ExisteProducto : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            long idProduct = (long)value;
            var oConceptRequest = (ConceptRequest)validationContext.ObjectInstance;
            var context = validationContext.GetService<DataContext>();
            Product? product = context.Products.Find(idProduct);
            if (product == null || product.State == 0 || (product.Id == oConceptRequest.IdProduct && product.Stock < oConceptRequest.Quantity)) return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
        
    }

    #endregion
}
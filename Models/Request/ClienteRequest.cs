using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Web_Service_.Net_Core.Models.Request
{
    public class ClienteRequest
    {
[Key]
        public long Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tenes un minimo de 3 caracteres")]
        public string Nombre { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "El apellido debe tenes un minimo de 3 caracteres")]
        public string Apellido { get; set; } = null!;
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string Dni { get; set; } = null!;
        [Required]
        [EmailAddress]
        [EmailValid(ErrorMessage = "El correo ya existe")]
        public string Correo { get; set; } = null!;


        #region Validations
        private class EmailValid() : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string nuevoCorreo = value + "";
                using (DBContext db = new())
                {
                    var clienteRequest = (ClienteRequest)validationContext.ObjectInstance;
                   
                    if (db.Clientes.Any(x => x.Correo == nuevoCorreo && x.Id != clienteRequest.Id && x.Estado))
                    {
                        return new ValidationResult(ErrorMessage);
                    }

                    return ValidationResult.Success;
                }
            }
        }
        #endregion
    }


}
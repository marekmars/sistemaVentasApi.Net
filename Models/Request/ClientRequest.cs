using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Web_Service_.Net_Core.Models.Request
{
    public class ClientRequest
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tenes un minimo de 3 caracteres")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "El apellido debe tenes un minimo de 3 caracteres")]
        public string LastName { get; set; } = null!;
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string IdCard { get; set; } = null!;
        [Required]
        [EmailAddress]
        [EmailValid(ErrorMessage = "El correo ya existe")]
        public string Mail { get; set; } = null!;


        #region Validations
        private class EmailValid() : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string newMail = value + "";
                using (DBContext db = new())
                {
                    var clientRequest = (ClientRequest)validationContext.ObjectInstance;

                    if (db.Clients.Any(x => x.Mail == newMail && x.Id != clientRequest.Id && x.State!=0))
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
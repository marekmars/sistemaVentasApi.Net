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
        [MinLength(3, ErrorMessage = "The Name must have at least 3 characters")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "The LastName must have at least 3 characters")]
        public string LastName { get; set; } = null!;
        [RegularExpression(@"^\d{8}$", ErrorMessage = "The IdCard must have 8 digits")]
        [IdCardValid(ErrorMessage = "The IdCard already exists")]
        public string IdCard { get; set; } = null!;
        [Required]
        [EmailAddress]
        [EmailValid(ErrorMessage = "The Email already exists")]
        public string Mail { get; set; } = null!;


        #region Validations
        private class EmailValid : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string newMail = value + "";
                var clientRequest = (ClientRequest)validationContext.ObjectInstance;
                
                var context = validationContext.GetService<DataContext>();
                
                if (context.Clients.Any(x => x.Mail == newMail && x.Id != clientRequest.Id && x.State != 0))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }
        private class IdCardValid : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string newIdCard = value + "";
                var clientRequest = (ClientRequest)validationContext.ObjectInstance;
                
                var context = validationContext.GetService<DataContext>();
                
                if (context.Clients.Any(x => x.IdCard == newIdCard && x.Id != clientRequest.Id && x.State != 0))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }
        #endregion
    }


}
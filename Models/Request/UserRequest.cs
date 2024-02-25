
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_Service_.Net_Core.Models.Request
{
    public class UserRequest
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "El rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El rol tiene que ser mayor a 0")]
        [RoleValid(ErrorMessage = "El rol no existe")]
        public int IdRole { get; set; }
        [ForeignKey("IdRole")]
        public Role? Role { get; set; } = null!;
        [Required]
        [EmailAddress]
        [EmailValid(ErrorMessage = "El correo ya existe")]
        public string Mail { get; set; } = null!;
        [MinLength(6, ErrorMessage = "La clave debe tener un minimo de 6 caracteres")]
        public string Password { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tenes un minimo de 3 caracteres")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "El apellido debe tenes un minimo de 3 caracteres")]
        public string LastName { get; set; } = null!;
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string IdCard { get; set; } = null!;

        #region Validations
        private class EmailValid() : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string newMail = value + "";
                var oUserRequest = (UserRequest)validationContext.ObjectInstance;
                var context = validationContext.GetService<DataContext>();

                if (context.Users.Any(x => x.Mail == newMail && x.Id != oUserRequest.Id && x.State == 1))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;

            }
        }

        private class RoleValid : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                int newRole = (int)value;
                var context = validationContext.GetService<DataContext>();

                if (!context.Roles.Any(r => r.Id == newRole)) 
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }

        #endregion
    }
}
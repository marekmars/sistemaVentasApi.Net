
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_Service_.Net_Core.Models.Request
{
    public class UsuarioRequest
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "El rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El rol es requerido")]
        public int IdRol { get; set; }
        [ForeignKey("IdRol")]
        public Rol? Rol { get; set; } = null!;
        [Required]
        [EmailAddress]
        [EmailValid(ErrorMessage = "El correo ya existe")]
        public string Correo { get; set; } = null!;
        [MinLength(6, ErrorMessage = "La clave debe tener un minimo de 6 caracteres")]
        public string Clave { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tenes un minimo de 3 caracteres")]
        public string Nombre { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "El apellido debe tenes un minimo de 3 caracteres")]
        public string Apellido { get; set; } = null!;
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string Dni { get; set; } = null!;

        #region Validations
        private class EmailValid() : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string nuevoCorreo = value + "";
                using (DBContext db = new())
                {
                    var oUsuarioRequest = (UsuarioRequest)validationContext.ObjectInstance;

                    if (db.Usuarios.Any(x => x.Correo == nuevoCorreo && x.Id != oUsuarioRequest.Id && x.Estado))
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
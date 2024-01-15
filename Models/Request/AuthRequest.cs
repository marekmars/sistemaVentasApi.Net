using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
    public class AuthRequest
    {
        [Required]
        public string? User {get;set;}
        [Required] 
        public string? Clave {get;set;}
    }
}
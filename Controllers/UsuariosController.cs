using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService? _iUserService;

        public UsuariosController(IUserService userService)
        {
            _iUserService = userService;
        }
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthRequest oModel)
        {
            Response response = new();

            try
            {
                var oUserResponse = _iUserService.Auth(oModel);
                
                if (oUserResponse == null)
                {
                    response.Success = 0;
                    response.Message = "Usuario y/o contraseña incorrecta";
                  
                }else{ 
                response.Success=1;
                response.Message = $"Bienvenido {oUserResponse.Correo}";
                response.Data=oUserResponse;
                }
                
               
            }
            catch (System.Exception)
            {

                throw;
            }


            return Ok(response);
        }
    }
}
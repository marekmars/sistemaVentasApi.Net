using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService userService)
        {
            _usuarioService = userService;
        }
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthRequest oModel)

        {
            Response response = new();

            try
            {
                var oUserResponse = _usuarioService.Auth(oModel);

                if (oUserResponse == null)
                {
                    response.Success = 0;
                    response.Message = "Usuario y/o contraseña incorrecta";

                }
                else
                {
                    response.Success = 1;
                    response.Message = $"Bienvenido {oUserResponse.Correo}";
                    response.Data = oUserResponse;
                }


            }
            catch (System.Exception)
            {

                throw;
            }


            return Ok(response);
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult GetAll()
        {
            Response oResponse = new();
            try
            {

                oResponse.Data = _usuarioService.GetAll();
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando los usuarios {ex.Message}"; ;
            }
            return Ok(oResponse);
        }
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{Id}")]
        public IActionResult Delete(int id)
        {
            Response oResponse = new Response();
            try
            {
                _usuarioService.Delete(id);
                oResponse.Success = 1;
                oResponse.Message = "Se elimino correctamente";
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(UsuarioRequest oUsuarioRequest)
        {
            Response oResponse = new Response();
            try
            {
                _usuarioService.Edit(oUsuarioRequest);
                oResponse.Success = 1;
                oResponse.Message = "Se edito correctamente";
                oResponse.Data = oUsuarioRequest;

            }
            catch (Exception ex)
            {

                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Add(UsuarioRequest oUsuarioRequest)
        {
            Response oResponse = new Response();
            try
            {

                _usuarioService.Add(oUsuarioRequest);
                oResponse.Success = 1;
                oResponse.Message = "Se agrego correctamente";
                oResponse.Data = oUsuarioRequest;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }
        [HttpGet("GetAllP")]
        public IActionResult GetAllP([FromQuery] ParametrosPaginado oParametrosPaginado)
        {
            Response oResponse = new Response();
            try
            {
                oResponse.Success = 1;
                var result = _usuarioService.GetAllP(oParametrosPaginado);

                oResponse.Data = new
                {
                    Data = result.Data,
                    TotalElements = result.TotalElements
                };
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrió un error buscando los usuarios {ex.Message}";
            }
            return Ok(oResponse);
        }
    }

}
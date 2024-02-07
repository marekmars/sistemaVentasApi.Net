
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RolsController : ControllerBase
    {
        private readonly IRolService _rolService;
        public RolsController(IRolService rolService)
        {
            _rolService = rolService;
        }
        [HttpGet]
        public IActionResult GetRols()
        {
            ApiResponse<Rol> oResponse = new();
            try
            {
                oResponse.Data = _rolService.GetRols();
                oResponse.Success = 1;
                oResponse.Message = $"Se encontraron todos los roles";
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando los roles {ex.Message}"; 
            }
            return Ok(oResponse);
        }
    }
}
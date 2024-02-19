
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RolesController : ControllerBase
    {
        private readonly IRoleService _rolService;
        public RolesController(IRoleService rolService)
        {
            _rolService = rolService;
        }
        [HttpGet]
        public IActionResult GetRols()
        {
            ApiResponse<Role> oResponse = new();
            try
            {
                oResponse.Data = _rolService.GetRoles();
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
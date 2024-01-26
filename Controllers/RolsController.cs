using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models.Response;
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
        public IActionResult GetRols(){
             Response oResponse = new();
            try
            {

                oResponse.Data = _rolService.GetRols();
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando los roles {ex.Message}"; ;
            }
            return Ok(oResponse);
        }
    }
}
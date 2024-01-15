using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Services;
namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }
        [HttpPost]
        public IActionResult Add(VentaRequest oVentaRequest)
        {
            Response response = new();
            try
            {
               
                _ventaService.Add(oVentaRequest);
                response.Success = 1;
                response.Message = "Se agrego correctamente";
                response.Data = oVentaRequest;
            }
            catch (Exception ex)
            {
                response.Success = 0;
                response.Message = ex.Message;
            }

            return Ok(response);

        }


    }
}
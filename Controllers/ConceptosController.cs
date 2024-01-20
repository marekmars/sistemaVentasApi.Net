using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConceptosController : ControllerBase
    {
        private readonly IConceptoService _conceptoService;
        public ConceptosController(IConceptoService conceptoService)
        {
            _conceptoService = conceptoService;
        }
        [HttpGet("{idVenta}")]
        public IActionResult GetAllbyVenta(long idVenta)
        {

            Response response = new();
            try
            {
                var conceptos = _conceptoService.GetAllbyVenta(idVenta);
                response.Data = conceptos;
                response.Success = 1;

            }
            catch (Exception ex)
            {

                response.Success = 0;
                response.Message = $"Ocurrio un error buscando los conceptos: {ex.Message}";
            }

            return Ok(response);
        }
    }
}
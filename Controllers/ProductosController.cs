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
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            Response oResponse = new Response();
            try
            {

                List<ProductoRequest> oProductoRequest = _productoService.GetAll();
                oResponse.Data = oProductoRequest;
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }
        [HttpGet("filter")]
        public IActionResult FiltrarProductos([FromQuery] string searchTerm, [FromQuery] int limite = 5)
        {
            Response response = new();
            try
            {
                var productosFiltrados = _productoService.FiltrarProductos(searchTerm, limite);
                if (productosFiltrados != null && productosFiltrados.Any())
                {
                    response.Success = 1;
                    response.Data = productosFiltrados;
                }
                else
                {
                    response.Success=0;
                    response.Message = "No se encontraron productos filtrados";
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
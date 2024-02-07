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
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }
        // [HttpGet]
        // public IActionResult Get()
        // {
        //     Response oResponse = new Response();
        //     try
        //     {

        //         oResponse.Data =  _productoService.GetAll();;
        //         oResponse.Success = 1;
        //     }
        //     catch (Exception ex)
        //     {
        //         oResponse.Message = ex.Message;
        //     }
        //     return Ok(oResponse);

        // }
        // [HttpGet("filter")]
        // public IActionResult FiltrarProductos([FromQuery] string searchTerm, [FromQuery] int limite = 5)
        // {
        //     Response response = new();
        //     try
        //     {
        //         var productosFiltrados = _productoService.FiltrarProductos(searchTerm, limite);
        //         if (productosFiltrados != null && productosFiltrados.Any())
        //         {
        //             response.Success = 1;
        //             response.Data = productosFiltrados;
        //         }
        //         else
        //         {
        //             response.Success = 0;
        //             response.Message = "No se encontraron productos filtrados";
        //         }
        //     }
        //     catch (System.Exception)
        //     {

        //         throw;
        //     }


        //     return Ok(response);
        // }

        // [HttpGet("GetAllP")]
        // public IActionResult GetAllP([FromQuery] ParametrosPaginado oParametrosPaginado)
        // {
        //     Response oResponse = new Response();
        //     try
        //     {
        //         oResponse.Success = 1;
        //         var result = _productoService.GetAllP(oParametrosPaginado);

        //         oResponse.Data = new
        //         {
        //             Data = result.Data,
        //             TotalElements = result.TotalElements
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         oResponse.Success = 0;
        //         oResponse.Message = $"Ocurrió un error buscando los productos {ex.Message}";
        //     }
        //     return Ok(oResponse);
        // }
        // [HttpPost]
        // public IActionResult Add(ProductoRequest oProductoRequest)
        // {
        //     Response response = new();
        //     try
        //     {

        //         _productoService.Add(oProductoRequest);
        //         response.Success = 1;
        //         response.Message = "Se agrego correctamente";
        //         response.Data = oProductoRequest;
        //     }
        //     catch (Exception ex)
        //     {
        //         response.Success = 0;
        //         response.Message = ex.Message;
        //     }

        //     return Ok(response);
        // }

        // [HttpPut]
        // public IActionResult Edit(ProductoRequest oProductoRequest)
        // {
        //     Response oResponse = new Response();
        //     try
        //     {
        //         _productoService.Edit(oProductoRequest);
        //         oResponse.Success = 1;
        //         oResponse.Message = "Se edito correctamente";


        //     }
        //     catch (Exception ex)
        //     {

        //         oResponse.Success = 0;
        //         oResponse.Message = ex.Message;
        //     }
        //     return Ok(oResponse);
        // }

        // [HttpDelete("{Id}")]
        // [Authorize(Roles = "Administrador")]
        // public IActionResult Delete(long id)
        // {
        //     Response response = new();
        //     try
        //     {
        //         _productoService.Delete(id);
        //         response.Success = 1;
        //         response.Message = "Se elimino correctamente";
        //     }
        //     catch (Exception ex)
        //     {
        //         response.Success = 0;
        //         response.Message = ex.Message;
        //     }

        //     return Ok(response);

        // }

    }
}
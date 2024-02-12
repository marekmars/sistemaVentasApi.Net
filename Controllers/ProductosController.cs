using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
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
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            ApiResponse<Producto> oResponse = new();
            try
            {

                oResponse = _productoService.GetProducto(id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el producto {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;

            }
            return Ok(oResponse);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<Producto> oResponse = new();
            try
            {
                oResponse = _productoService.GetProductos(oQueryParameters);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el producto {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }

        [HttpPost]
        public IActionResult Add(ProductoRequest oProductoRequest)
        {

            ApiResponse<Producto> oResponse = new ApiResponse<Producto>();
            try
            {

                oResponse = _productoService.AddProducto(oProductoRequest);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error agregando el producto {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpPatch]
        [Authorize(Roles = "Administrador")]

        public IActionResult Udpate(ProductoRequest oCliente)
        {

            ApiResponse<Producto> oResponse = new();
            try
            {
                oResponse = _productoService.UpdateProducto(oCliente);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error actualizando el producto {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(long Id)
        {
            ApiResponse<Producto> oResponse = new ApiResponse<Producto>();
            try
            {
                oResponse = _productoService.DeleteProducto(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el producto {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpDelete("fulldelete/{Id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult FullDeleteProducto(long Id)
        {
            ApiResponse<Producto> oResponse = new ApiResponse<Producto>();
            try
            {
                oResponse = _productoService.FullDeleteProducto(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando el producto {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

    }
}
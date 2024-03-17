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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            ApiResponse<Product> oResponse = new();
            try
            {

                oResponse = _productService.GetProduct(id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el producto {ex}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;

            }
            return Ok(oResponse);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<Product> oResponse = new();
            try
            {
                oResponse = _productService.GetProducts(oQueryParameters);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el producto {ex}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }

        [HttpPost]
        public IActionResult Add(ProductRequest oProductoRequest)
        {

            ApiResponse<Product> oResponse = new ApiResponse<Product>();
            try
            {

                oResponse = _productService.AddProduct(oProductoRequest);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error agregando el producto {ex}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]

        public IActionResult Udpate(ProductRequest oCliente)
        {

            ApiResponse<Product> oResponse = new();
            try
            {
                oResponse = _productService.UpdateProduct(oCliente);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error actualizando el producto {ex}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long Id)
        {
            ApiResponse<Product> oResponse = new ApiResponse<Product>();
            try
            {
                oResponse = _productService.DeleteProduct(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el producto {ex}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpDelete("fulldelete/{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FullDeleteProducto(long Id)
        {
            ApiResponse<Product> oResponse = new ApiResponse<Product>();
            try
            {
                oResponse = _productService.FullDeleteProduct(Id);

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
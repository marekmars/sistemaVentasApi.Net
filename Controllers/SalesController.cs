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
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            ApiResponse<Sale> oApiResponse = new();
            try
            {
                oApiResponse = _saleService.GetSale(Id); ;
            }
            catch (Exception ex)
            {
                oApiResponse = new()
                {
                    Success = 1,
                    Message = $"No se encontraron sales: {ex.Message}",
                    Data = null,
                    TotalCount = 1
                };
            }

            return Ok(oApiResponse);
        }
        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<Sale> oResponse = new();
            try
            {
                oResponse = _saleService.GetSales(oQueryParameters);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando la sale {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }
        [HttpPost]
        public IActionResult Add(SaleRequest oSaleRequest)
        {
            ApiResponse<Sale> oApiResponse = new();
            try
            {
                // Call the service to add the new user
                oApiResponse = _saleService.AddSale(oSaleRequest);
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions
                oApiResponse.Success = 0;
                oApiResponse.Message = $"An error occurred while adding the sale: {ex.Message}";
                oApiResponse.Data = [];
                oApiResponse.TotalCount = 0;
            }
            return Ok(oApiResponse);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long Id)
        {
            ApiResponse<Sale> oResponse = new();
            try
            {
                oResponse = _saleService.DeleteSale(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando la sale {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpDelete("fulldelete/{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FullDeleteSale(long Id)
        {
            ApiResponse<Sale> oResponse = new ApiResponse<Sale>();
            try
            {
                oResponse = _saleService.FullDeleteSale(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando la sale {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]

        public IActionResult Udpate(SaleRequest oSaleRequest)
        {

            ApiResponse<Sale> oApiResponse = new();
            try
            {
                oApiResponse = _saleService.UpdateSale(oSaleRequest);

            }
            catch (Exception ex)
            {
                oApiResponse.Success = 0;
                oApiResponse.Message = $"Ocurrio un error actualizando el cliente {ex.Message}";
                oApiResponse.Data = [];
                oApiResponse.TotalCount = 0;
            }
            return Ok(oApiResponse);

        }


    }
}
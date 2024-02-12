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
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }
        //     [HttpPost]
        //     public IActionResult Add(VentaRequest oVentaRequest)
        //     {
        //         Response response = new();
        //         try
        //         {

        //             _ventaService.Add(oVentaRequest);
        //             response.Success = 1;
        //             response.Message = "Se agrego correctamente";
        //             response.Data = oVentaRequest;
        //         }
        //         catch (Exception ex)
        //         {
        //             response.Success = 0;
        //             response.Message = ex.Message;
        //         }

        //         return Ok(response);

        //     }
        //     [Authorize(Roles = "Administrador")]
        //     [HttpDelete("{Id}")]
        //     public IActionResult Delete(long Id)
        //     {
        //         Response response = new();
        //         try
        //         {
        //             _ventaService.Delete(Id);
        //             response.Success = 1;
        //             response.Message = "Se elimino correctamente";
        //         }
        //         catch (Exception ex)
        //         {
        //             response.Success = 0;
        //             response.Message = ex.Message;
        //         }

        //         return Ok(response);

        //     }

        //     [HttpGet("GetAllP")]
        //     public IActionResult GetAllP([FromQuery] ParametrosPaginado oParametrosPaginado)
        //     {
        //         Response oResponse = new Response();
        //         try
        //         {
        //             oResponse.Success = 1;
        //             var result = _ventaService.GetAllP(oParametrosPaginado);

        //             oResponse.Data = new
        //             {
        //                 Data = result.Data,
        //                 TotalElements = result.TotalElements
        //             };
        //         }
        //         catch (Exception ex)
        //         {
        //             oResponse.Success = 0;
        //             oResponse.Message = $"Ocurrió un error buscando las ventas {ex.Message}";
        //         }
        //         return Ok(oResponse);
        //     }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            ApiResponse<Venta> oApiResponse = new();
            try
            {
                oApiResponse = _ventaService.GetVenta(Id); ;
            }
            catch (Exception ex)
            {
                oApiResponse = new()
                {
                    Success = 1,
                    Message = $"No se encontraron ventas: {ex.Message}",
                    Data = null,
                    TotalCount = 1
                };
            }

            return Ok(oApiResponse);
        }
        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<Venta> oResponse = new();
            try
            {
                oResponse = _ventaService.GetVentas(oQueryParameters);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando la venta {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }
        [HttpPost]
        public IActionResult Add(VentaRequest oVentaRequest)
        {
            ApiResponse<Venta> oApiResponse = new();
            try
            {
                // Call the service to add the new user
                oApiResponse = _ventaService.AddVenta(oVentaRequest);
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

        //     [HttpGet("filter")]
        //     public IActionResult FiltrarVentas([FromQuery] string searchTerm, [FromQuery] int limite = 5)
        //     {

        //         Response response = new();
        //         try
        //         {
        //             var ventasFiltradas = _ventaService.FiltrarVentas(searchTerm, limite);
        //             response.Success = 1;
        //             response.Data = ventasFiltradas;

        //         }
        //         catch (System.Exception)
        //         {
        //             response.Success = 0;
        //             response.Message = "No se encontraron ventas filtradas";

        //         }
        //         return Ok(response);
        //     }
        //       [HttpGet("filterDate")]
        //     public IActionResult FiltrarVentasDate([FromQuery] string fecha, [FromQuery] int limite = 5)
        //     {

        //         ApiResponse response = new();
        //         try
        //         {
        //             var ventasFiltradas = _ventaService.FiltrarVentasFecha(fecha, limite);
        //             response.Success = 1;
        //             response.Data = ventasFiltradas;

        //         }
        //         catch (System.Exception)
        //         {
        //             response.Success = 0;
        //             response.Message = "No se encontraron ventas filtradas";

        //         }
        //         return Ok(response);
        //     }

    }
}
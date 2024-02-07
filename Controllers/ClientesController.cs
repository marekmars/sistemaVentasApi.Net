
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            ApiResponse<Cliente> oResponse = new();
            try
            {

                oResponse = _clienteService.GetCliente(id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;

            }
            return Ok(oResponse);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<Cliente> oResponse = new();
            try
            {
                oResponse = _clienteService.GetClientes(oQueryParameters);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest oClienteRequest)
        {

            ApiResponse<Cliente> oResponse = new ApiResponse<Cliente>();
            try
            {

                oResponse = _clienteService.AddCliente(oClienteRequest);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error agregando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpPatch]
        [Authorize(Roles = "Administrador")]

        public IActionResult Udpate(ClienteRequest oCliente)
        {

            ApiResponse<Cliente> oResponse = new();
            try
            {
                oResponse = _clienteService.UpdateCliente(oCliente);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error actualizando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(long Id)
        {
            ApiResponse<Cliente> oResponse = new ApiResponse<Cliente>();
            try
            {
                oResponse = _clienteService.DeleteCliente(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

    }


}
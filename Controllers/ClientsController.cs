
using System.Text.Json.Serialization;
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
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            ApiResponse<Client> oResponse = new();
            try
            {

                oResponse = _clientService.GetClient(id);

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
            ApiResponse<Client> oResponse = new();
            try
            {
                oResponse = _clientService.GetClients(oQueryParameters);
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
        public IActionResult Add(ClientRequest oClienteRequest)
        {

            ApiResponse<Client> oResponse = new ApiResponse<Client>();
            try
            {

                oResponse = _clientService.AddClient(oClienteRequest);

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
        [Authorize(Roles = "Admin")]

        public IActionResult Udpate(ClientRequest oCliente)
        {

            ApiResponse<Client> oResponse = new();
            try
            {
                oResponse = _clientService.UpdateClient(oCliente);

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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long Id)
        {
            ApiResponse<Client> oResponse = new ApiResponse<Client>();
            try
            {
                oResponse = _clientService.DeleteClient(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

        [HttpGet("check")]
        [Authorize(Roles = "Admin")]
        public IActionResult IsEmailValid([FromForm] string Correo)
        {
            ApiResponse<Client> oResponse = new ApiResponse<Client>();
            try
            {
                oResponse = _clientService.MailExist(Correo);
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

        [HttpDelete("fulldelete/{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FullDeleteClient(long Id)
        {
            ApiResponse<Client> oResponse = new ApiResponse<Client>();
            try
            {
                oResponse = _clientService.FullDeleteClient(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando el cliente {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }


    }


}
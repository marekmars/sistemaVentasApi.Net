using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
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
            Response oResponse = new();
            try
            {

                oResponse.Success = 1;
                oResponse.Data = _clienteService.Get(id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el cliente {ex.Message}";
                throw;
            }
            return Ok(oResponse);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            Response oResponse = new Response();
            try
            {
                
                oResponse.Data = _clienteService.GetAll();
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando los clientes {ex.Message}"; ;
            }
            return Ok(oResponse);

        }
        [HttpGet("GetAllP")]
        public IActionResult GetAllP([FromQuery] ParametrosPaginado oParametrosPaginado)
        {
            Response oResponse = new Response();
            try
            {
                oResponse.Success = 1;
                var result = _clienteService.GetAllP(oParametrosPaginado);

                oResponse.Data = new
                {
                    Data = result.Data,
                    TotalElements = result.TotalElements
                };
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrió un error buscando los clientes {ex.Message}";
            }
            return Ok(oResponse);
        }

        [HttpPost]
        public IActionResult Add(ClientesRequest oClienteRequest)
        {

            Response oResponse = new Response();
            try
            {

                _clienteService.Add(oClienteRequest);
                oResponse.Success = 1;
                oResponse.Message = "Se agrego correctamente";
                oResponse.Data = oClienteRequest;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpPut]
        public IActionResult Edit(ClientesRequest oCliente)
        {

            Response oResponse = new Response();
            try
            {
                _clienteService.Edit(oCliente);
                oResponse.Success = 1;
                oResponse.Message = "Se edito correctamente";
                oResponse.Data = oCliente;

            }
            catch (Exception ex)
            {

                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(long Id)
        {
            Response oResponse = new Response();
            try
            {
                _clienteService.Delete(Id);
                oResponse.Success = 1;
                oResponse.Message = "Se elimino correctamente";
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }
        [HttpGet("filter")]
        public IActionResult Filtrarclientes([FromQuery] string searchTerm, [FromQuery] int limite = 5)
        {

            Response response = new();
            try
            {
                var clientesFiltrados = _clienteService.FiltrarClientes(searchTerm, limite);
                response.Success = 1;
                response.Data = clientesFiltrados;

            }
            catch (System.Exception)
            {
                response.Success = 0;
                response.Message = "No se encontraron clientes filtrados";

            }


            return Ok(response);
        }
    }


}
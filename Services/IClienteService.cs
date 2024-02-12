
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;
namespace Web_Service_.Net_Core.Services
{
    public interface IClienteService
    {
        public ApiResponse<Cliente> GetClientes(QueryParameters queryParameters);
        public ApiResponse<Cliente> GetCliente(long id);
        public ApiResponse<Cliente> AddCliente(ClienteRequest oClienteRequest);
        public ApiResponse<Cliente> UpdateCliente(ClienteRequest oClienteRequest);
        public ApiResponse<Cliente> DeleteCliente(long Id);
        public ApiResponse<Cliente> CorreoExiste(string correo);
        public ApiResponse<Cliente> FullDeleteCliente(long Id);
        
    }
}
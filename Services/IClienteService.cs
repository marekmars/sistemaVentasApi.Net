
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;
namespace Web_Service_.Net_Core.Services
{
    public interface IClienteService
    {
        // public Cliente Get(long Id);
        // public IEnumerable<Cliente> Get(QueryParameters queryParameters);
        // // public (IEnumerable<Cliente> Data, int TotalElements) GetAllP(ProductQueryParameters oParametrosPaginado);
        // public void Edit(ClientesRequest oClientesRequest);
        // public void Add(ClientesRequest oClientesRequest);
        // public void Delete(long Id);
        // public IEnumerable<Cliente> FiltrarClientes(string searchTerm, int limite);

        public ApiResponse<Cliente> GetClientes(QueryParameters queryParameters);
        public ApiResponse<Cliente> GetCliente(long id);
        public ApiResponse<Cliente> AddCliente(ClienteRequest oClienteRequest);
        public ApiResponse<Cliente> UpdateCliente(ClienteRequest oClienteRequest);
        public ApiResponse<Cliente> DeleteCliente(long Id);
    }
}
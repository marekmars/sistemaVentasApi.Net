
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;
namespace Web_Service_.Net_Core.Services
{
    public interface IClientService
    {
        public ApiResponse<Client> GetClients(QueryParameters queryParameters);
        public ApiResponse<Client> GetClient(long id);
        public ApiResponse<Client> AddClient(ClientRequest oClientRequest);
        public ApiResponse<Client> UpdateClient(ClientRequest oClientRequest);
        public ApiResponse<Client> DeleteClient(long Id);
        public ApiResponse<Client> MailExist(string correo);
        public ApiResponse<Client> FullDeleteClient(long Id);
        
    }
}
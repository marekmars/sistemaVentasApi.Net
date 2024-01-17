using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public interface IClienteService
    {
        public ClientesRequest Get();
        public List<ClientesRequest> GetAll();
        public void Edit(ClientesRequest oProductoRequest);
        public void Add(ClientesRequest oProductoRequest);
        public IEnumerable<Cliente> FiltrarClientes(string searchTerm, int limite);
    }
}
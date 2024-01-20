using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public interface IClienteService
    {
        public Cliente Get(long Id);
        public IEnumerable<Cliente> GetAll();
        public (IEnumerable<Cliente> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado);

        public void Edit(ClientesRequest oClientesRequest);
        public void Add(ClientesRequest oClientesRequest);
        public void Delete(long Id);
        public IEnumerable<Cliente> FiltrarClientes(string searchTerm, int limite);
    }
}
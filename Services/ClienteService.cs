using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public class ClienteService : IClienteService
    {
        public void Add(ClientesRequest oProductoRequest)
        {
            throw new NotImplementedException();
        }

        public void Edit(ClientesRequest oProductoRequest)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cliente> FiltrarClientes(string searchTerm, int limite)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Cliente>(); // O podrías lanzar una excepción si prefieres
            }
            using (DBContext db = new())
            {
                var clientesFiltrados = db.Clientes
                     .Where(p => EF.Functions.Like(p.Nombre, $"%{searchTerm}%") ||
                                 EF.Functions.Like(p.Apellido, $"%{searchTerm}%") ||
                                 EF.Functions.Like(p.Dni, $"%{searchTerm}%"))
                     .Take(limite)
                     .ToList();
                return clientesFiltrados;
            }
        }

        public ClientesRequest Get()
        {
            throw new NotImplementedException();
        }

        public List<ClientesRequest> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
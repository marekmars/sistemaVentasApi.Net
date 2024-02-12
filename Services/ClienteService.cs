using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{

    public class ClienteService : IClienteService
    {
        private readonly DBContext _context;
        public ClienteService(DBContext? context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ApiResponse<Cliente> GetClientes(QueryParameters queryParameters)
        {
            IQueryable<Cliente> query = _context.Clientes;

            Console.WriteLine(queryParameters.Filter);

            var totalElements = _context.Clientes.Count();

            // Add a condition to filter clients with State equal to 1
            query = query.Where(p => p.Estado == true);

            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "name" => query.OrderBy(c => c.Nombre),
                    "lastname" => query.OrderBy(c => c.Apellido),
                    "dni" => query.OrderBy(c => c.Dni),
                    "mail" => query.OrderBy(c => c.Correo),
                    _ => query.OrderBy(c => c.Id),
                };
                if (queryParameters.Desc)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
            }


            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                string[] filters = filter.Split(' ');

                query = query.AsEnumerable().Where(c =>
                    filters.All(f =>
                        c.Nombre.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        c.Apellido.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        c.Dni.Contains(f, StringComparison.CurrentCultureIgnoreCase)
                    )
                ).AsQueryable();
            }

            if (queryParameters.Skip.HasValue)
            {
                query = query.Skip(queryParameters.Skip.Value);
            }

            if (queryParameters.Limit.HasValue)
            {
                query = query.Take(queryParameters.Limit.Value);
            }



            var clientes = query.ToList();

            if (clientes.Count == 0) throw new Exception("No se encontraron clientes");

            return new ApiResponse<Cliente>
            {
                Success = 1,
                Message = "Clientes obtenidos correctamente",
                Data = clientes,
                TotalCount = totalElements
            };
        }


        public ApiResponse<Cliente> GetCliente(long Id)
        {
            Cliente? oCliente = _context.Clientes
                               .Where(c => c.Id == Id && c.Estado == true)
                               .FirstOrDefault()
                               ?? throw new Exception("No se encontro un cliente activo con ese ID");

            return new ApiResponse<Cliente>
            {
                Success = 1,
                Message = "Cliente obtenido correctamente",
                Data = [oCliente],
                TotalCount = 1
            };
        }

        public ApiResponse<Cliente> AddCliente(ClienteRequest oClienteRequest)
        {
            Cliente oCliente = new Cliente
            {
                Nombre = oClienteRequest.Nombre,
                Apellido = oClienteRequest.Apellido,
                Dni = oClienteRequest.Dni,
                Correo = oClienteRequest.Correo
            };

            _context.Add(oCliente);
            _context.SaveChanges();

            return new ApiResponse<Cliente>
            {
                Success = 1,
                Message = "Cliente creado correctamente",
                Data = [oCliente],
                TotalCount = 1
            };
        }

        public ApiResponse<Cliente> UpdateCliente(ClienteRequest oClienteRequest)
        {

            Cliente? oCliente = _context.Clientes
                               .Where(c => c.Id == oClienteRequest.Id && c.Estado == true)
                               .FirstOrDefault()
                               ?? throw new Exception("No se encontro un cliente activo con ese ID");

            oCliente.Nombre = !string.IsNullOrEmpty(oClienteRequest.Nombre) ? oClienteRequest.Nombre : oCliente.Nombre;
            oCliente.Apellido = !string.IsNullOrEmpty(oClienteRequest.Apellido) ? oClienteRequest.Apellido : oCliente.Apellido;
            oCliente.Dni = !string.IsNullOrEmpty(oClienteRequest.Dni) ? oClienteRequest.Dni : oCliente.Dni;
            oCliente.Correo = !string.IsNullOrEmpty(oClienteRequest.Correo) ? oClienteRequest.Correo : oCliente.Correo;


            _context.Entry(oCliente).State = EntityState.Modified;
            _context.SaveChanges();

            return new ApiResponse<Cliente>
            {
                Success = 1,
                Message = "Cliente actualizado correctamente",
                Data = [oCliente],
                TotalCount = 1
            };


        }

        public ApiResponse<Cliente> DeleteCliente(long Id)
        {
            Cliente? oCliente = _context.Clientes
                                .Where(c => c.Id == Id && c.Estado == true)
                                .FirstOrDefault()
                                ?? throw new Exception("No se encontro un cliente activo con ese ID");

            oCliente.Estado = false;
            _context.Entry(oCliente).State = EntityState.Modified;
            _context.SaveChanges();
            return new ApiResponse<Cliente>
            {
                Success = 1,
                Message = "Cliente eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }
        public ApiResponse<Cliente> FullDeleteCliente(long Id)
        {
            Cliente? oCliente = _context.Clientes.Find(Id) ?? throw new Exception("No se encontro el cliente");
            _context.Remove(oCliente);
            _context.SaveChanges();

            return new ApiResponse<Cliente>
            {
                Success = 1,
                Message = "Cliente eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Cliente> CorreoExiste(string correo)
        {
            Cliente? oCliente = _context.Clientes.Where(c => c.Correo == correo && c.Estado == true).FirstOrDefault();


            if (oCliente == null)
            {
                return new ApiResponse<Cliente>
                {
                    Success = 1,
                    Message = "Correo válido",
                    Data = [],
                    TotalCount = 1
                };
            }
            return new ApiResponse<Cliente>
            {
                Success = 0,
                Message = "El correo ya existe",
                Data = [oCliente],
                TotalCount = 1
            };
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{

    public class ClientService : IClientService
    {
        private readonly DataContext _context;
        public ClientService(DataContext? context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ApiResponse<Client> GetClients(QueryParameters queryParameters)
        {
            IQueryable<Client> query = _context.Clients;

            // Add a condition to filter clients with State equal to 1
            query = query.Where(p => p.State == 1);

            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                string[] filters = filter.Split(' ');

                query = query.AsEnumerable().Where(c =>
                    filters.All(f =>
                        c.Id.ToString().Contains(f) ||
                        c.Name.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        c.LastName.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        c.Mail.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        c.IdCard.Contains(f, StringComparison.CurrentCultureIgnoreCase)
                    )
                ).AsQueryable();
            }


            var totalElements = query.Count();

            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "id" => query.OrderBy(c => c.Id),
                    "name" => query.OrderBy(c => c.Name),
                    "lastname" => query.OrderBy(c => c.LastName),
                    "idcard" => query.OrderBy(c => c.IdCard),
                    "mail" => query.OrderBy(c => c.Mail),
                    _ => query.OrderBy(c => c.Id),
                };
                if (queryParameters.Desc == 1)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
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

            return new ApiResponse<Client>
            {
                Success = 1,
                Message = "Clientes obtenidos correctamente",
                Data = clientes,
                TotalCount = totalElements
            };
        }


        public ApiResponse<Client> GetClient(long Id)
        {
            Client? oClient = _context.Clients
                               .Where(c => c.Id == Id && c.State == 1)
                               .FirstOrDefault()
                               ?? throw new Exception("No se encontro un cliente activo con ese ID");

            return new ApiResponse<Client>
            {
                Success = 1,
                Message = "Client obtenido correctamente",
                Data = [oClient],
                TotalCount = 1
            };
        }

        public ApiResponse<Client> AddClient(ClientRequest oClientRequest)
        {
            Client oClient = new Client
            {
                Name = oClientRequest.Name,
                LastName = oClientRequest.LastName,
                IdCard = oClientRequest.IdCard,
                Mail = oClientRequest.Mail,
                State = 1
            };

            _context.Add(oClient);
            _context.SaveChanges();

            return new ApiResponse<Client>
            {
                Success = 1,
                Message = "Client creado correctamente",
                Data = [oClient],
                TotalCount = 1
            };
        }

        public ApiResponse<Client> UpdateClient(ClientRequest oClientRequest)
        {

            Client? oClient = _context.Clients
                               .Where(c => c.Id == oClientRequest.Id && c.State == 1)
                               .FirstOrDefault()
                               ?? throw new Exception("No se encontro un cliente activo con ese ID");

            oClient.Name = !string.IsNullOrEmpty(oClientRequest.Name) ? oClientRequest.Name : oClient.Name;
            oClient.LastName = !string.IsNullOrEmpty(oClientRequest.LastName) ? oClientRequest.LastName : oClient.LastName;
            oClient.IdCard = !string.IsNullOrEmpty(oClientRequest.IdCard) ? oClientRequest.IdCard : oClient.IdCard;
            oClient.Mail = !string.IsNullOrEmpty(oClientRequest.Mail) ? oClientRequest.Mail : oClient.Mail;


            _context.Entry(oClient).State = EntityState.Modified;
            _context.SaveChanges();

            return new ApiResponse<Client>
            {
                Success = 1,
                Message = "Client actualizado correctamente",
                Data = [oClient],
                TotalCount = 1
            };


        }

        public ApiResponse<Client> DeleteClient(long Id)
        {
            Client? oClient = _context.Clients
                                .Where(c => c.Id == Id && c.State == 1)
                                .FirstOrDefault()
                                ?? throw new Exception("No se encontro un cliente activo con ese ID");

            oClient.State = 0;
            _context.Entry(oClient).State = EntityState.Modified;
            _context.SaveChanges();
            return new ApiResponse<Client>
            {
                Success = 1,
                Message = "Client eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }
        public ApiResponse<Client> FullDeleteClient(long Id)
        {
            Client? oClient = _context.Clients.Find(Id) ?? throw new Exception("No se encontro el cliente");
            _context.Remove(oClient);
            _context.SaveChanges();

            return new ApiResponse<Client>
            {
                Success = 1,
                Message = "Client eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Client> MailExist(string correo)
        {
            Client? oClient = _context.Clients.Where(c => c.Mail == correo && c.State == 1).FirstOrDefault();


            if (oClient == null)
            {
                return new ApiResponse<Client>
                {
                    Success = 1,
                    Message = "Mail válido",
                    Data = [],
                    TotalCount = 1
                };
            }
            return new ApiResponse<Client>
            {
                Success = 0,
                Message = "El correo ya existe",
                Data = [oClient],
                TotalCount = 1
            };
        }
    }
}
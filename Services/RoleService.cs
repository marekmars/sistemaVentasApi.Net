using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;

namespace Web_Service_.Net_Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly DBContext _context;
        public RoleService(DBContext dBContext)
        {
            _context = dBContext;
        }
        public List<Role> GetRoles()
        {
            List<Role> oRoles = new();

            oRoles = _context.Roles.OrderBy(d => d.Id).ToList();
            if (oRoles.Count != 0)
            {
                return oRoles;
            }
            else
            {
                throw new Exception("No se encontraron roles");
            }
        }
    }
}
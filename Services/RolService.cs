using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;

namespace Web_Service_.Net_Core.Services
{
    public class RolService : IRolService
    {
        private readonly DBContext _context;
        public RolService(DBContext dBContext)
        {
            _context = dBContext;
        }
        public List<Rol> GetRols()
        {
            List<Rol> oRols = new();

            oRols = _context.Rols.OrderBy(d => d.Id).ToList();
            if (oRols.Count != 0)
            {
                return oRols;
            }
            else
            {
                throw new Exception("No se encontraron roles");
            }
        }
    }
}
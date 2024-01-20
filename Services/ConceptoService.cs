using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;

namespace Web_Service_.Net_Core.Services
{
    public class ConceptoService : IConceptoService
    {
        private DBContext _context;
        public ConceptoService(DBContext dBContext)
        {
            _context = dBContext;
        }
        public IEnumerable<Concepto> GetAllbyVenta(long idVenta)
        {
            var conceptos = _context.Conceptos
            .Where(c => idVenta == c.IdVenta)
            .Include(c => c.Producto)
            .ToList()
            ;
            if (conceptos.Count == 0)
            {
                throw new Exception("No hay conceptos");
            }
            else
            {
                return conceptos;
            }
        }
    }
}
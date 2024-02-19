using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;

namespace Web_Service_.Net_Core.Services
{
    public class ConceptService : IConceptService
    {
        private DBContext _context;
        public ConceptService(DBContext dBContext)
        {
            _context = dBContext;
        }
        public IEnumerable<Concept> GetAllbySale(long idSale)
        {
            var concepts = _context.Concepts
            .Where(c => idSale == c.IdSale)
            .Include(c => c.Product)
            .ToList()
            ;
            if (concepts.Count == 0)
            {
                throw new Exception("No hay conceptos");
            }
            else
            {
                return concepts;
            }
        }
    }
}
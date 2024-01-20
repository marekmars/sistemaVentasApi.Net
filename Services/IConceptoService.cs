using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;

namespace Web_Service_.Net_Core.Services
{
    public interface IConceptoService
    {
        public IEnumerable<Concepto> GetAllbyVenta(long idVenta);
    }
}
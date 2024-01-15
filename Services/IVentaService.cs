using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public interface IVentaService
    {
        public void Add(VentaRequest oVentaRequest);
    }
}
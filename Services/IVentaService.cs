using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public interface IVentaService
    {
        public void Add(VentaRequest oVentaRequest);
        public IEnumerable<Venta> GetAll();
        public (IEnumerable<Venta> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado);
        public Venta Get(int id);
        public void Edit(VentaRequest oVentaRequest);
        public void Delete(long id);



    }
}
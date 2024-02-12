using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public interface IVentaService
    {
        public ApiResponse<Venta> GetVentas(QueryParameters queryParameters);
        public ApiResponse<Venta> GetVenta(long id);
        public ApiResponse<Venta> AddVenta(VentaRequest oVentaRequest);
        public ApiResponse<Venta> UpdateVenta(VentaRequest oVentaRequest);
        public ApiResponse<Venta> DeleteVenta(long Id);
        public ApiResponse<Venta> FullDeleteVenta(long Id);


    }
}
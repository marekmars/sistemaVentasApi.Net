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
    public interface ISaleService
    {
        public ApiResponse<Sale> GetSales(QueryParameters queryParameters);
        public ApiResponse<Sale> GetSale(long id);
        public ApiResponse<Sale> AddSale(SaleRequest oSaleRequest);
        public ApiResponse<Sale> UpdateSale(SaleRequest oSaleRequest);
        public ApiResponse<Sale> DeleteSale(long Id);
        public ApiResponse<Sale> FullDeleteSale(long Id);


    }
}
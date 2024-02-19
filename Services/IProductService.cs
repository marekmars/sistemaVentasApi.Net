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
    public interface IProductService
    {
        public ApiResponse<Product> GetProducts(QueryParameters queryParameters);
        public ApiResponse<Product> GetProduct(long id);
        public ApiResponse<Product> AddProduct(ProductRequest oProductRequest);
        public ApiResponse<Product> UpdateProduct(ProductRequest oProductRequest);
        public ApiResponse<Product> DeleteProduct(long Id);
        public ApiResponse<Product> FullDeleteProduct(long Id);
       
    }
}
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
    public interface IProductoService
    {
        public ApiResponse<Producto> GetProductos(QueryParameters queryParameters);
        public ApiResponse<Producto> GetProducto(long id);
        public ApiResponse<Producto> AddProducto(ProductoRequest oProductoRequest);
        public ApiResponse<Producto> UpdateProducto(ProductoRequest oProductoRequest);
        public ApiResponse<Producto> DeleteProducto(long Id);
        public ApiResponse<Producto> FullDeleteProducto(long Id);
       
    }
}
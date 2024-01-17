using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public interface IProductoService
    {
        public Producto Get();
        public List<ProductoRequest> GetAll();
        public void Edit(ProductoRequest oProductoRequest);
        public void Add(ProductoRequest oProductoRequest);
        public void Delete(int id);
        public List<Producto> FiltrarProductos(string searchTerm, int limite);
    }
}
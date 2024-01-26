using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public interface IProductoService
    {
        public Producto Get();
        public List<Producto> GetAll();
        public void Edit(ProductoRequest oProductoRequest);
        public void Add(ProductoRequest oProductoRequest);
        public void Delete(long id);
        public List<Producto> FiltrarProductos(string searchTerm, int limite);
        public (IEnumerable<Producto> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado);
    }
}
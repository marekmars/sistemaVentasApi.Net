using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public class ProductoService : IProductoService
    {
        public Producto Get()
        {
            throw new NotImplementedException();
        }
        public void Add(ProductoRequest oProductoRequest)
        {
            throw new NotImplementedException();
        }

        public void Edit(ProductoRequest oProductoRequest)
        {
            throw new NotImplementedException();
        }

        public List<Producto> FiltrarProductos(string searchTerm, int limite)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Producto>(); // O podrías lanzar una excepción si prefieres
            }
            using (DBContext db = new())
            {
                var productosFiltrados = db.Productos
                                .Where(p => EF.Functions.Like(p.Nombre, $"%{searchTerm}%"))
                                .Take(limite)
                                .ToList();
                return productosFiltrados;
            }
        }

        public List<ProductoRequest> GetAll()
        {
            using (DBContext db = new DBContext())
            {
                List<Producto>? productos = [.. db.Productos.OrderByDescending(x => x.Id)];
                List<ProductoRequest> productosRequest = [];
                if (productos.Count > 0)
                {
                    foreach (var producto in productos)
                    {
                        productosRequest.Add(new ProductoRequest()
                        {
                            Id = producto.Id,
                            Nombre = producto.Nombre,
                            PrecioUnitario = producto.PrecioUnitario,
                            Costo = producto.Costo
                        });
                    }


                }
                return productosRequest;
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
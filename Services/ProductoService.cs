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
        private readonly DBContext _context;
        public ProductoService(DBContext dBContext)
        {
            _context = dBContext;
        }
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

            var productosFiltrados = _context.Productos
                            .Where(p => EF.Functions.Like(p.Nombre, $"%{searchTerm}%")&&p.Stock>0)
                            .Take(limite)
                            .ToList();
            return productosFiltrados;

        }

        public List<ProductoRequest> GetAll()
        {

            List<Producto>? productos = [.. _context.Productos.OrderByDescending(x => x.Id)];
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

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
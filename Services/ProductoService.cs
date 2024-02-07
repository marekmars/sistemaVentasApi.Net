using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

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
            try
            {
                var producto = new Producto
                {
                    Nombre = oProductoRequest.Nombre,
                    PrecioUnitario = oProductoRequest.PrecioUnitario,
                    Costo = oProductoRequest.Costo,
                    Stock = oProductoRequest.Stock
                };
                _context.Productos.Add(producto);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("No se pudo agregar el producto: " + e);
            }
        }

        public void Edit(ProductoRequest oProductoRequest)
        {


            Producto? oProducto = _context.Productos.Find(oProductoRequest.Id);
            if (oProducto != null)
            {
                oProducto.Nombre = oProductoRequest.Nombre;
                oProducto.PrecioUnitario = oProductoRequest.PrecioUnitario;
                oProducto.Costo = oProductoRequest.Costo;
                oProducto.Stock = oProductoRequest.Stock;
                _context.Entry(oProducto).State = EntityState.Modified;
                _context.SaveChanges();
            }

        }
        public void Delete(long id)
        {
            Producto? oProducto = _context.Productos.Find(id);
            if (oProducto != null)
            {
                _context.Remove(oProducto);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No se encontro el producto");
            }
        }

        public List<Producto> FiltrarProductos(string searchTerm, int limite)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Producto>(); // O podrías lanzar una excepción si prefieres
            }

            var productosFiltrados = _context.Productos
                            .Where(p => EF.Functions.Like(p.Nombre, $"%{searchTerm}%") && p.Stock > 0)
                            .Take(limite)
                            .ToList();
            return productosFiltrados;

        }

        public List<Producto> GetAll()
        {

            List<Producto>? productos = [.. _context.Productos.OrderByDescending(x => x.Id)];

            if (productos.Count > 0)
            {
                return productos;

            }
            else
            {
                throw new Exception("No se encontraron productos");
            }


        }

        public void Delete(int id)
        {
            Producto? oProducto = _context.Productos.Find(id);
            if (oProducto != null)
            {
                _context.Remove(oProducto);
                _context.SaveChanges();
            }
        }

        // public (IEnumerable<Producto> Data, int TotalElements) GetAllP(QueryParameters oParametrosPaginado)
        // {
        //     List<Producto> oProductos = new();

        //     var totalElements = _context.Productos.Count(); // Obtener el total de elementos

        //     oProductos = _context.Productos
        //              .OrderBy(d => d.Id)
        //              .Skip(oParametrosPaginado.Skip * oParametrosPaginado.Limit)
        //              .Take(oParametrosPaginado.Limit)
        //              .ToList();

        //     if (oProductos.Count != 0)
        //     {
        //         return (oProductos, totalElements);
        //     }
        //     else
        //     {
        //         throw new Exception("No se encontraron productos");
        //     }
        //     throw new Exception("No Implmentado");
        // }

        public Producto Get(long id)
        {
            throw new NotImplementedException();
        }

        List<Producto> IProductoService.Get()
        {
            throw new NotImplementedException();
        }
    }
}
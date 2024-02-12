using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
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

        public ApiResponse<Producto> AddProducto(ProductoRequest oProductoRequest)
        {
            Producto oProducto = new()
            {
                Nombre = oProductoRequest.Nombre,
                PrecioUnitario = oProductoRequest.PrecioUnitario,
                Costo = oProductoRequest.Costo,
                Stock = oProductoRequest.Stock

            };

            _context.Add(oProducto);
            _context.SaveChanges();

            return new ApiResponse<Producto>
            {
                Success = 1,
                Message = "Producto creado correctamente",
                Data = [oProducto],
                TotalCount = 1
            };
        }

        public ApiResponse<Producto> DeleteProducto(long Id)
        {
            Producto? oProducto = _context.Productos
                                .Where(p => p.Id == Id && p.Estado == true)
                                .FirstOrDefault()
                                ?? throw new Exception("No se encontro un producto activo con ese ID");
            oProducto.Estado = false;
            _context.Entry(oProducto).State = EntityState.Modified;
            _context.SaveChanges();
            return new ApiResponse<Producto>
            {
                Success = 1,
                Message = "Producto eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Producto> FullDeleteProducto(long Id)
        {
            Producto? oProducto = _context.Productos.Find(Id) ?? throw new Exception("No se encontro el producto");
            _context.Remove(oProducto);
            _context.SaveChanges();

            return new ApiResponse<Producto>
            {
                Success = 1,
                Message = "Producto eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Producto> GetProducto(long Id)
        {
            Producto? oProducto = _context.Productos
                              .Where(p => p.Id == Id && p.Estado == true)
                              .FirstOrDefault()
                              ?? throw new Exception("No se encontro un producto activo con ese ID");

            return new ApiResponse<Producto>
            {
                Success = 1,
                Message = "Producto obtenido correctamente",
                Data = [oProducto],
                TotalCount = 1
            };
        }

        public ApiResponse<Producto> GetProductos(QueryParameters queryParameters)
        {
            IQueryable<Producto> query = _context.Productos;

            Console.WriteLine(queryParameters.Filter);

            var totalElements = _context.Productos.Count();

            // Add a condition to filter clients with State equal to 1
            query = query.Where(p => p.Estado == true);

            // Add an OrderBy clause to make the query predictable
            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "name" => query.OrderBy(p => p.Nombre),
                    "price" => query.OrderBy(p => p.PrecioUnitario),
                    "cost" => query.OrderBy(p => p.Costo),
                    "stock" => query.OrderBy(p => p.Stock),
                    _ => query.OrderBy(p => p.Id),
                };
                if (queryParameters.Desc)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
            }

            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                string[] filters = filter.Split(' ');

                query = query.AsEnumerable().Where(p =>
                    filters.All(f =>
                        p.Nombre.Contains(f, StringComparison.CurrentCultureIgnoreCase)
                    )
                ).AsQueryable();
            }


            if (queryParameters.Skip.HasValue)
            {
                query = query.Skip(queryParameters.Skip.Value);
            }

            if (queryParameters.Limit.HasValue)
            {
                query = query.Take(queryParameters.Limit.Value);
            }
            var productos = query.ToList();

            if (productos.Count == 0) throw new Exception("No se encontraron productos");

            return new ApiResponse<Producto>
            {
                Success = 1,
                Message = "Productos obtenidos correctamente",
                Data = productos,
                TotalCount = totalElements
            };
        }

        public ApiResponse<Producto> UpdateProducto(ProductoRequest oProductoRequest)
        {
            Producto? oProducto = _context.Productos
                                          .Where(c => c.Id == oProductoRequest.Id && c.Estado == true)
                                          .FirstOrDefault()
                                          ?? throw new Exception("No se encontro un cliente activo con ese ID");

            oProducto.Nombre = (!string.IsNullOrEmpty(oProductoRequest.Nombre)) ? oProductoRequest.Nombre : oProducto.Nombre;
            oProducto.PrecioUnitario = (oProductoRequest.PrecioUnitario > 0) ? oProductoRequest.PrecioUnitario : oProducto.PrecioUnitario;
            oProducto.Costo = (oProductoRequest.Costo > 0) ? oProductoRequest.Costo : oProducto.Costo;
            oProducto.Stock = (oProductoRequest.Stock > 0) ? oProductoRequest.Stock : oProducto.Stock;

            _context.Entry(oProducto).State = EntityState.Modified;
            _context.SaveChanges();

            return new ApiResponse<Producto>
            {
                Success = 1,
                Message = "Producto actualizado correctamente",
                Data = [oProducto],
                TotalCount = 1
            };
        }

















        // public void Add(ProductoRequest oProductoRequest)
        // {
        //     try
        //     {
        //         var producto = new Producto
        //         {
        //             Nombre = oProductoRequest.Nombre,
        //             PrecioUnitario = oProductoRequest.PrecioUnitario,
        //             Costo = oProductoRequest.Costo,
        //             Stock = oProductoRequest.Stock
        //         };
        //         _context.Productos.Add(producto);
        //         _context.SaveChanges();
        //     }
        //     catch (Exception e)
        //     {

        //         throw new Exception("No se pudo agregar el producto: " + e);
        //     }
        // }

        // public void Edit(ProductoRequest oProductoRequest)
        // {


        //     Producto? oProducto = _context.Productos.Find(oProductoRequest.Id);
        //     if (oProducto != null)
        //     {
        //         oProducto.Nombre = oProductoRequest.Nombre;
        //         oProducto.PrecioUnitario = oProductoRequest.PrecioUnitario;
        //         oProducto.Costo = oProductoRequest.Costo;
        //         oProducto.Stock = oProductoRequest.Stock;
        //         _context.Entry(oProducto).State = EntityState.Modified;
        //         _context.SaveChanges();
        //     }

        // }
        // public void Delete(long id)
        // {
        //     Producto? oProducto = _context.Productos.Find(id);
        //     if (oProducto != null)
        //     {
        //         _context.Remove(oProducto);
        //         _context.SaveChanges();
        //     }
        //     else
        //     {
        //         throw new Exception("No se encontro el producto");
        //     }
        // }

        // public List<Producto> FiltrarProductos(string searchTerm, int limite)
        // {
        //     if (string.IsNullOrEmpty(searchTerm))
        //     {
        //         return new List<Producto>(); // O podrías lanzar una excepción si prefieres
        //     }

        //     var productosFiltrados = _context.Productos
        //                     .Where(p => EF.Functions.Like(p.Nombre, $"%{searchTerm}%") && p.Stock > 0)
        //                     .Take(limite)
        //                     .ToList();
        //     return productosFiltrados;

        // }

        // public List<Producto> GetAll()
        // {

        //     List<Producto>? productos = [.. _context.Productos.OrderByDescending(x => x.Id)];

        //     if (productos.Count > 0)
        //     {
        //         return productos;

        //     }
        //     else
        //     {
        //         throw new Exception("No se encontraron productos");
        //     }


        // }

        // public void Delete(int id)
        // {
        //     Producto? oProducto = _context.Productos.Find(id);
        //     if (oProducto != null)
        //     {
        //         _context.Remove(oProducto);
        //         _context.SaveChanges();
        //     }
        // }

        // // public (IEnumerable<Producto> Data, int TotalElements) GetAllP(QueryParameters oParametrosPaginado)
        // // {
        // //     List<Producto> oProductos = new();

        // //     var totalElements = _context.Productos.Count(); // Obtener el total de elementos

        // //     oProductos = _context.Productos
        // //              .OrderBy(d => d.Id)
        // //              .Skip(oParametrosPaginado.Skip * oParametrosPaginado.Limit)
        // //              .Take(oParametrosPaginado.Limit)
        // //              .ToList();

        // //     if (oProductos.Count != 0)
        // //     {
        // //         return (oProductos, totalElements);
        // //     }
        // //     else
        // //     {
        // //         throw new Exception("No se encontraron productos");
        // //     }
        // //     throw new Exception("No Implmentado");
        // // }


    }
}
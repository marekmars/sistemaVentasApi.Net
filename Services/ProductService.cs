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
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext dBContext)
        {
            _context = dBContext;
        }

        public ApiResponse<Product> AddProduct(ProductRequest oProductRequest)
        {
            Product oProduct = new()
            {
                Name = oProductRequest.Name,
                UnitaryPrice = oProductRequest.UnitaryPrice,
                Cost = oProductRequest.Cost,
                Stock = oProductRequest.Stock,
                State = 1
            };

            _context.Add(oProduct);
            _context.SaveChanges();

            return new ApiResponse<Product>
            {
                Success = 1,
                Message = "Product creado correctamente",
                Data = [oProduct],
                TotalCount = 1
            };
        }

        public ApiResponse<Product> DeleteProduct(long Id)
        {
            Product? oProduct = _context.Products
                                .Where(p => p.Id == Id && p.State == 1)
                                .FirstOrDefault()
                                ?? throw new Exception("No se encontro un producto activo con ese ID");
            oProduct.State = 0;
            _context.Entry(oProduct).State = EntityState.Modified;
            _context.SaveChanges();
            return new ApiResponse<Product>
            {
                Success = 1,
                Message = "Product eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Product> FullDeleteProduct(long Id)
        {
            Product? oProduct = _context.Products.Find(Id) ?? throw new Exception("No se encontro el producto");
            _context.Remove(oProduct);
            _context.SaveChanges();

            return new ApiResponse<Product>
            {
                Success = 1,
                Message = "Product eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Product> GetProduct(long Id)
        {
            Product? oProduct = _context.Products
                              .Where(p => p.Id == Id && p.State == 1)
                              .FirstOrDefault()
                              ?? throw new Exception("No se encontro un producto activo con ese ID");

            return new ApiResponse<Product>
            {
                Success = 1,
                Message = "Product obtenido correctamente",
                Data = [oProduct],
                TotalCount = 1
            };
        }

        public ApiResponse<Product> GetProducts(QueryParameters queryParameters)
        {
            IQueryable<Product> query = _context.Products;

            Console.WriteLine(queryParameters.Filter);

            var totalElements = _context.Products.Count();

            // Add a condition to filter clients with State equal to 1
            query = query.Where(p => p.State == 1);

            // Add an OrderBy clause to make the query predictable
            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "name" => query.OrderBy(p => p.Name),
                    "price" => query.OrderBy(p => p.UnitaryPrice),
                    "cost" => query.OrderBy(p => p.Cost),
                    "stock" => query.OrderBy(p => p.Stock),
                    _ => query.OrderBy(p => p.Id),
                };
                if (queryParameters.Desc == 1)
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
                        p.Name.Contains(f, StringComparison.CurrentCultureIgnoreCase)
                    )
                ).AsQueryable();
            }
            if (queryParameters.Max > 0)
            {
                query = query.AsEnumerable().Where(p =>
                          p.UnitaryPrice <= queryParameters.Max).AsQueryable();

            }
            if (queryParameters.Min > 0)
            {
                query = query.AsEnumerable().Where(p =>
                       queryParameters.Min <= p.UnitaryPrice).AsQueryable();
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

            return new ApiResponse<Product>
            {
                Success = 1,
                Message = "Products obtenidos correctamente",
                Data = productos,
                TotalCount = totalElements
            };
        }

        public ApiResponse<Product> UpdateProduct(ProductRequest oProductRequest)
        {
            Product? oProduct = _context.Products
                                          .Where(c => c.Id == oProductRequest.Id && c.State == 1)
                                          .FirstOrDefault()
                                          ?? throw new Exception("No se encontro un cliente activo con ese ID");

            oProduct.Name = (!string.IsNullOrEmpty(oProductRequest.Name)) ? oProductRequest.Name : oProduct.Name;
            oProduct.UnitaryPrice = (oProductRequest.UnitaryPrice > 0) ? oProductRequest.UnitaryPrice : oProduct.UnitaryPrice;
            oProduct.Cost = (oProductRequest.Cost > 0) ? oProductRequest.Cost : oProduct.Cost;
            oProduct.Stock = (oProductRequest.Stock > 0) ? oProductRequest.Stock : oProduct.Stock;

            _context.Entry(oProduct).State = EntityState.Modified;
            _context.SaveChanges();

            return new ApiResponse<Product>
            {
                Success = 1,
                Message = "Product actualizado correctamente",
                Data = [oProduct],
                TotalCount = 1
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public class VentaService : IVentaService
    {
        public readonly DBContext _context;

        public VentaService(DBContext dBContext)
        {
            _context = dBContext;
        }

        public ApiResponse<Venta> AddVenta(VentaRequest oVentaRequest)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var venta = new Venta
                    {
                        Total = oVentaRequest.Conceptos.Sum(x => x.Cantidad * x.PrecioUnitario),
                        Fecha = DateTime.Now,
                        IdCliente = oVentaRequest.IdCliente,
                        Estado = true
                    };
                    _context.Ventas.Add(venta);
                    _context.SaveChanges();
                    foreach (var item in oVentaRequest.Conceptos)
                    {
                        Concepto concepto = new()
                        {
                            IdVenta = venta.Id,
                            IdProducto = item.IdProducto,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.PrecioUnitario,
                            Importe = item.Cantidad * item.PrecioUnitario
                        };
                        _context.Conceptos.Add(concepto);
                    }
                    _context.SaveChanges();
                    dbTransaction.Commit();

                    Venta? oVenta = _context.Ventas
                              .Include(x => x.Cliente)
                              .Include(x => x.Conceptos)
                              .FirstOrDefault(x => x.Id == venta.Id);
                    return new ApiResponse<Venta>
                    {
                        Success = 1,
                        Message = "Se agrego correctamente la venta",
                        Data = oVenta != null ? [oVenta] : [],
                        TotalCount = 1
                    };
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    throw new Exception("No se pudo realizar la venta" + e);
                }
            }
        }

        public ApiResponse<Venta> DeleteVenta(long Id)
        {
            Venta? oVenta = _context.Ventas
                                .Where(v => v.Id == Id && v.Estado == true)
                                .FirstOrDefault()
                                ?? throw new Exception("No se encontro un Venta activo con ese ID");

            oVenta.Estado = false;
            _context.Entry(oVenta).State = EntityState.Modified;
            _context.SaveChanges();
            return new ApiResponse<Venta>
            {
                Success = 1,
                Message = "Venta eliminada correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Venta> FullDeleteVenta(long Id)
        {
            Venta? oVenta = _context.Ventas.Find(Id) ?? throw new Exception("No se encontro la Venta");
            _context.Remove(oVenta);
            _context.SaveChanges();

            return new ApiResponse<Venta>
            {
                Success = 1,
                Message = "Venta eliminada correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Venta> GetVenta(long Id)
        {
            Venta? oVenta = _context.Ventas
                              .Include(x => x.Cliente)
                              .Include(x => x.Conceptos)
                              .ThenInclude(c => c.Producto)
                              .Where(v => v.Id == Id && v.Estado == true)
                              .FirstOrDefault()
                              ?? throw new Exception("No se encontro una venta activa con ese ID");
            //   oVenta.Conceptos= [.. _context.Conceptos.Where(c => c.IdVenta == Id)];

            return new ApiResponse<Venta>
            {
                Success = 1,
                Message = "Venta obtenida correctamente",
                Data = [oVenta],
                TotalCount = 1
            };
        }

        public ApiResponse<Venta> GetVentas(QueryParameters queryParameters)
        {
            IQueryable<Venta> query = _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Conceptos)
            .ThenInclude(c => c.Producto);

            Console.WriteLine(queryParameters.Filter);

            var totalElements = _context.Ventas.Count();

            // Add a condition to filter clients with State equal to 1
            query = query.Where(p => p.Estado == true);

            // Add an OrderBy clause to make the query predictable
            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "name" => query.OrderBy(v => v.Cliente.Nombre),
                    "lastname" => query.OrderBy(v => v.Cliente.Apellido),
                    "date" => query.OrderBy(v => v.Fecha),
                    "total" => query.OrderBy(v => v.Total),
                    _ => query.OrderBy(v => v.Id),
                };
                if (queryParameters.Desc)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
            }
            if (queryParameters.Skip.HasValue)
            {
                query = query.Skip(queryParameters.Skip.Value);
            }

            if (queryParameters.Limit.HasValue)
            {
                query = query.Take(queryParameters.Limit.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                DateTime.TryParse(queryParameters.Filter, out DateTime filterDate);
                query = query
                        .Where(v =>
                            v.Fecha.Date == filterDate.Date ||
                            v.Cliente.Nombre.ToLower().Contains(filter) ||
                            v.Cliente.Apellido.ToLower().Contains(filter) ||
                            v.Conceptos.Any(c => c.Producto.Nombre.ToLower().Contains(filter))
                  );
            }

            Console.WriteLine("Paso");

            var clientes = query.ToList();

            if (clientes.Count == 0) throw new Exception("No se encontraron clientes");

            return new ApiResponse<Venta>
            {
                Success = 1,
                Message = "Clientes obtenidos correctamente",
                Data = clientes,
                TotalCount = totalElements
            };
        }

        public ApiResponse<Venta> UpdateVenta(VentaRequest oVentaRequest)
        {
            throw new NotImplementedException();
        }
        // public void Add(VentaRequest oVentaRequest)
        // {

        //     using (var dbTransaction = _context.Database.BeginTransaction())
        //     {
        //         Console.WriteLine("ENTROO");
        //         try
        //         {
        //             var venta = new Venta
        //             {
        //                 Total = oVentaRequest.Conceptos.Sum(x => x.Cantidad * x.PrecioUnitario),
        //                 Fecha = DateTime.Now,
        //                 IdVenta = oVentaRequest.IdVenta
        //             };
        //             _context.Ventas.Add(venta);
        //             _context.SaveChanges();
        //             foreach (var item in oVentaRequest.Conceptos)
        //             {
        //                 Concepto concepto = new()
        //                 {
        //                     IdVenta = venta.Id,
        //                     IdProducto = item.IdProducto,
        //                     Cantidad = item.Cantidad,
        //                     PrecioUnitario = item.PrecioUnitario,
        //                     Importe = item.Cantidad * item.PrecioUnitario
        //                 };
        //                 _context.Conceptos.Add(concepto);

        //             }
        //             _context.SaveChanges();
        //             dbTransaction.Commit();
        //         }
        //         catch (Exception e)
        //         {
        //             dbTransaction.Rollback();
        //             throw new Exception("No se pudo realizar la venta" + e);
        //         }
        //     }

        // }

        // public void Delete(long Id)
        // {

        //     Venta oVenta = _context.Ventas.Find(Id);
        //     if (oVenta != null)
        //     {
        //         _context.Remove(oVenta);
        //         _context.SaveChanges();
        //     }
        //     else
        //     {
        //         throw new Exception("No se encontro la venta");
        //     }
        // }

        // public Venta Get(int id)
        // {
        //     Venta? oVenta = _context.Ventas.Find(id);

        //     if (oVenta != null)
        //     {
        //         return oVenta;
        //     }
        //     else
        //     {
        //         throw new Exception("No se encontro la venta");
        //     }
        // }
        // public IEnumerable<Venta> GetAll()
        // {
        //     throw new NotImplementedException();
        // }

        // public (IEnumerable<Venta> Data, int TotalElements) GetAllP(ProductQueryParameters oParametrosPaginado)
        // {
        //     List<Venta> oVentas = new();

        //     var totalElements = _context.Ventas.Count(); // Obtener el total de elementos

        //     oVentas = _context.Ventas
        //              .OrderByDescending(d => d.Id)
        //              .Include(v => v.Venta) // Assuming Venta is the navigation property in Venta
        //              .Skip(oParametrosPaginado.PageIndex * oParametrosPaginado.ItemsPerPage)
        //              .Take(oParametrosPaginado.ItemsPerPage)
        //              .ToList();

        //     if (oVentas.Count != 0)
        //     {
        //         return (oVentas, totalElements);
        //     }
        //     else
        //     {
        //         throw new Exception("No se encontraron ventas");
        //     }
        // }

        // public IEnumerable<Venta> FiltrarVentas(string searchTerm, int limite)
        // {
        //     if (string.IsNullOrEmpty(searchTerm))
        //     {
        //         throw new Exception("La búsqueda no puede ser vacía");
        //     }

        //     List<Venta> oVentas = _context.Ventas.Include(v => v.Venta)
        //         .Where(p => EF.Functions.Like(p.Venta.Nombre, $"%{searchTerm}%") ||
        //                     EF.Functions.Like(p.Id.ToString(), $"%{searchTerm}%") ||
        //                     EF.Functions.Like(p.Venta.Apellido, $"%{searchTerm}%"))
        //         .Take(limite)
        //         .ToList();

        //     if (oVentas.Count != 0)
        //     {
        //         return oVentas;
        //     }
        //     else
        //     {
        //         throw new Exception("Venta no encontrada");
        //     }
        // }

        // public IEnumerable<Venta> FiltrarVentasFecha(string date, int limite)
        // {

        //     if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime fechaParseada))
        //     {
        //         throw new Exception("La búsqueda no puede ser vacía");
        //     }
        //     Console.WriteLine(date);

        //     DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateParsed);
        //     Console.WriteLine(dateParsed);
        //     List<Venta> oVentas = _context.Ventas.Include(v=>v.Venta)
        //         .Where(p => p.Fecha.Date == fechaParseada.Date)

        //         .Take(limite)
        //         .ToList();

        //     if (oVentas.Count != 0)
        //     {
        //         return oVentas;
        //     }
        //     else
        //     {
        //         throw new Exception("Venta no encontrada");
        //     }
        // }
    }
}

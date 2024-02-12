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
                            Importe = item.Cantidad * item.PrecioUnitario,
                            Estado = true
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Find the Venta
                    Venta? oVenta = _context.Ventas
                                        .Where(v => v.Id == Id && v.Estado == true)
                                        .FirstOrDefault()
                                        ?? throw new Exception("No se encontró una Venta activa con ese ID");

                    // Set the Estado to false for the Venta
                    oVenta.Estado = false;
                    _context.Entry(oVenta).State = EntityState.Modified;

                    // Find related objects (assuming ObjetoConcepto with IdVenta property)
                    var conceptos = _context.Conceptos
                                                       .Where(c => c.IdVenta == Id && c.Estado == true)
                                                       .ToList();

                    // Set the Estado to false for each related ObjetoConcepto
                    foreach (var concepto in conceptos)
                    {
                        concepto.Estado = false;
                        _context.Entry(concepto).State = EntityState.Modified;
                    }

                    // Save changes to the database
                    _context.SaveChanges();

                    // Commit the transaction
                    transaction.Commit();

                    return new ApiResponse<Venta>
                    {
                        Success = 1,
                        Message = "Venta y objetos relacionados eliminados correctamente",
                        Data = null,
                        TotalCount = 1
                    };
                }
                catch (Exception)
                {
                    // An error occurred, rollback the transaction
                    transaction.Rollback();
                    throw; // Re-throw the exception to propagate it
                }
            }
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

//TODO: Probar bien este metodo que esta raro xD
        public ApiResponse<Venta> UpdateVenta(VentaRequest oVentaRequest)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Venta? oVenta = _context.Ventas
                       .Include(v => v.Conceptos)  // Include Conceptos for updating
                       .FirstOrDefault(v => v.Id == oVentaRequest.Id)
                       ?? throw new Exception("No se encontró una Venta con ese ID");

                    oVenta.Total = oVentaRequest.Conceptos.Count > 0 ? oVentaRequest.Conceptos.Sum(x => x.Cantidad * x.PrecioUnitario) : oVenta.Total;
                    oVenta.Fecha = oVentaRequest.Fecha ?? oVenta.Fecha;
                    oVenta.IdCliente = oVentaRequest.IdCliente != 0 ? oVentaRequest.IdCliente : oVenta.IdCliente;
                    oVenta.Estado = true;

                    _context.Entry(oVenta).State = EntityState.Modified;
                    _context.SaveChanges();

                    foreach (var updatedConcepto in oVentaRequest.Conceptos)
                    {
                        var existingConcepto = _context.Conceptos.FirstOrDefault(ec => ec.Id == updatedConcepto.Id);

                        if (existingConcepto != null)
                        {
                            // Update properties of the existing Concepto
                            existingConcepto.IdProducto = updatedConcepto.IdProducto;
                            existingConcepto.Cantidad = updatedConcepto.Cantidad;
                            existingConcepto.PrecioUnitario = updatedConcepto.PrecioUnitario;
                            existingConcepto.Importe = existingConcepto.Cantidad * existingConcepto.PrecioUnitario;

                            _context.Entry(existingConcepto).State = EntityState.Modified;
                        }
                        else
                        {
                            Concepto concepto = new()
                            {
                                IdVenta = oVentaRequest.Id,
                                IdProducto = updatedConcepto.IdProducto,
                                Cantidad = updatedConcepto.Cantidad,
                                PrecioUnitario = updatedConcepto.PrecioUnitario,
                                Importe = updatedConcepto.Cantidad * updatedConcepto.PrecioUnitario,
                                Estado = true
                            };
                            _context.Entry(concepto).State = EntityState.Added;
                        }
                    }
                    _context.SaveChanges();
                    dbTransaction.Commit();

                    Venta? oVentaRes = _context.Ventas
                              .Include(x => x.Cliente)
                              .Include(x => x.Conceptos)
                              .FirstOrDefault(x => x.Id == oVenta.Id);
                    return new ApiResponse<Venta>
                    {
                        Success = 1,
                        Message = "Se edito correctamente la venta",
                        Data = oVentaRes != null ? [oVentaRes] : [],
                        TotalCount = 1
                    };
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    throw new Exception("No se pudo editar la venta" + e);
                }
            }
        }
    }
}

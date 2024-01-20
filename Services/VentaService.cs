using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
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
        public  void Add(VentaRequest oVentaRequest)
        {

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                Console.WriteLine("ENTROO");
                try
                {
                    var venta = new Venta
                    {
                        Total = oVentaRequest.Conceptos.Sum(x => x.Cantidad * x.PrecioUnitario),
                        Fecha = DateTime.Now,
                        IdCliente = oVentaRequest.IdCliente
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
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    throw new Exception("No se pudo realizar la venta" + e);
                }
            }

        }

        public void Delete(long Id)
        {

            Venta oVenta = _context.Ventas.Find(Id);
            if (oVenta != null)
            {
                _context.Remove(oVenta);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No se encontro la venta");
            }


        }

        public void Edit(VentaRequest oVentaRequest)
        {

        }
        public Venta Get(int id)
        {
            Venta? oVenta = _context.Ventas.Find(id);
                    
            if (oVenta != null)
            {
                return oVenta;
            }
            else
            {
                throw new Exception("No se encontro la venta");
            }
        }
        public IEnumerable<Venta> GetAll()
        {
            throw new NotImplementedException();
        }

        public (IEnumerable<Venta> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado)
        {
            List<Venta> oVentas = new();

            var totalElements = _context.Ventas.Count(); // Obtener el total de elementos

            oVentas = _context.Ventas
                     .OrderByDescending(d => d.Id)
                     .Include(v => v.Cliente) // Assuming Cliente is the navigation property in Venta
                     .Skip(oParametrosPaginado.PageIndex * oParametrosPaginado.ItemsPerPage)
                     .Take(oParametrosPaginado.ItemsPerPage)
                     .ToList();
                 
            if (oVentas.Count != 0)
            {
                return (oVentas, totalElements);
            }
            else
            {
                throw new Exception("No se encontraron ventas");
            }
        }
    }
}

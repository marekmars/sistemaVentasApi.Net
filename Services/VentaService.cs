using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public class VentaService : IVentaService
    {
        public readonly DBContext _context;
        public VentaService(DBContext dBContext)
        {
            _context = dBContext;
        }
        public void Add(VentaRequest oVentaRequest)
        {

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
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
            throw new NotImplementedException();
        }

        public Venta Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VentaRequest> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

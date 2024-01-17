using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;

namespace Web_Service_.Net_Core.Services
{
    public class VentaService : IVentaService
    {
        public void Add(VentaRequest oVentaRequest)
        {
            using (DBContext db = new())

            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var venta = new Venta
                        {
                            Total = oVentaRequest.Conceptos.Sum(x => x.Cantidad * x.PrecioUnitario),
                            Fecha = DateTime.Now,
                            IdCliente = oVentaRequest.IdCliente
                        };
                        db.Ventas.Add(venta);
                        db.SaveChanges();
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
                            db.Conceptos.Add(concepto);

                        }
                        db.SaveChanges();
                        dbTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        throw new Exception("No se pudo realizar la venta" + e);
                    }
                }
            }
        }

        public void Delete(long Id)
        {
            using (DBContext db = new DBContext())
            {
                Venta oVenta = db.Ventas.Find(Id);
                if (oVenta != null)
                {
                    db.Remove(oVenta);
                    db.SaveChanges();
                }else{
                    throw new Exception("No se encontro la venta");
                }

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

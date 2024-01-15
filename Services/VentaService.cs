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
                                Importe = item.Importe
                            };
                            db.Conceptos.Add(concepto);

                        }
                        db.SaveChanges();
                        dbTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw new Exception("No se pudo realizar la venta");
                    }
                }
            }
        }

    }
}

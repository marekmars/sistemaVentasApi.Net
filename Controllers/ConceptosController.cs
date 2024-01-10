using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConceptoController : ControllerBase
    {
        [HttpGet("Get")]
        public IActionResult Get()
        {
            Response oResponse = new Response();
            try
            {
                using (DBContext db = new DBContext())
                {
                    oResponse.Data = db.Conceptos.ToList();
                    oResponse.Success = 1;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpPost("Add")]
        public IActionResult Add(ConceptosRequest oModel)
        {

            Response oResponse = new Response();
            try
            {

                using (DBContext db = new DBContext())
                {
                    Concepto oConcepto = new()
                    {
                        IdVenta = oModel.IdVenta,
                        Cantidad = oModel.Cantidad,
                        PrecioUnitario = oModel.PrecioUnitario,
                        Importe = oModel.Importe,
                        IdProducto = oModel.IdProducto

                    };
                    db.Add(oConcepto);
                    db.SaveChanges();
                    oResponse.Success = 1;
                    oResponse.Message = "Se agrego correctamente";
                    oResponse.Data = oConcepto;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpPut("Edit")]
        public IActionResult Edit(ConceptosRequest oModel)
        {

            Response oResponse = new Response();
            try
            {

                using (DBContext db = new DBContext())
                {
                    Concepto oConcepto = db.Conceptos.Find(oModel.Id);
                    if (oConcepto != null)
                    {
                        oConcepto.IdVenta = oModel.IdVenta;
                        oConcepto.Cantidad = oModel.Cantidad;
                        oConcepto.PrecioUnitario = oModel.PrecioUnitario;
                        oConcepto.Importe = oModel.Importe;
                        oConcepto.IdProducto = oModel.IdProducto;
                        db.Entry(oConcepto).State = EntityState.Modified;
                        db.SaveChanges();
                        oResponse.Success = 1;
                        oResponse.Message = "Se edito correctamente";
                        oResponse.Data = oConcepto;
                    }
                    else
                    {

                        oResponse.Message = "No se encontro un elemento con esa id";
                        oResponse.Data = null;
                    }


                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete(long Id)
        {

            Response oResponse = new Response();
            try
            {

                using (DBContext db = new DBContext())
                {
                    Concepto oConcepto = db.Conceptos.Find(Id);
                    if (oConcepto != null)
                    {

                        db.Remove(oConcepto);
                        db.SaveChanges();
                        oResponse.Success = 1;
                        oResponse.Message = "Se elimino correctamente";
                        oResponse.Data = oConcepto;
                    }
                    else
                    {

                        oResponse.Message = "No se encontro un elemento con esa id";
                        oResponse.Data = null;
                    }
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }
    }
}

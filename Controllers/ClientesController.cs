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
    public class ClientesController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            Response oResponse = new Response();
            try
            {
                using (DBContext db = new DBContext())
                {
                    oResponse.Data = db.Clientes.OrderByDescending(d=>d.Id).ToList();
                    oResponse.Success = 1;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpPost]
        public IActionResult Add(ClientesRequest oModel)
        {

            Response oResponse = new Response();
            try
            {

                using (DBContext db = new DBContext())
                {
                    Cliente oCliente = new Cliente
                    {
                        Nombre = oModel.Nombre,
                        Apellido = oModel.Apellido,
                        Dni = oModel.Dni,
                        Correo = oModel.Correo
                    };
                    db.Add(oCliente);
                    db.SaveChanges();
                    oResponse.Success = 1;
                    oResponse.Message = "Se agrego correctamente";
                    oResponse.Data = oCliente;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);

        }

        [HttpPut]
        public IActionResult Edit(ClientesRequest oModel)
        {

            Response oResponse = new Response();
            try
            {

                using (DBContext db = new DBContext())
                {
                    Cliente oCliente = db.Clientes.Find(oModel.Id);
                    if (oCliente != null)
                    {
                        oCliente.Nombre = oModel.Nombre;
                        oCliente.Apellido = oModel.Apellido;
                        oCliente.Dni = oModel.Dni;
                        oCliente.Correo = oModel.Correo;
                        db.Entry(oCliente).State = EntityState.Modified;
                        db.SaveChanges();
                        oResponse.Success = 1;
                        oResponse.Message = "Se edito correctamente";
                        oResponse.Data = oCliente;
                    }
                    else
                    {

                        oResponse.Message = "No se encontro un usuario con esa id";
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

        [HttpDelete("{Id}")]
        public IActionResult Delete(long Id)
        {

            Response oResponse = new Response();
            try
            {

                using (DBContext db = new DBContext())
                {
                    Cliente oCliente = db.Clientes.Find(Id);
                    if (oCliente != null)
                    {
                        
                       db.Remove(oCliente);
                        db.SaveChanges();
                        oResponse.Success = 1;
                        oResponse.Message = "Se elimino correctamente";
                        oResponse.Data = oCliente;
                    }
                    else
                    {

                        oResponse.Message = "No se encontro un usuario con esa id";
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
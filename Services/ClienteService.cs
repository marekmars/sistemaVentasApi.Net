using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public class ClienteService : IClienteService
    {
        private readonly DBContext _context;
        public ClienteService(DBContext? context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void Add(ClientesRequest oClientesRequest)
        {

            Cliente oCliente = new Cliente
            {
                Nombre = oClientesRequest.Nombre,
                Apellido = oClientesRequest.Apellido,
                Dni = oClientesRequest.Dni,
                Correo = oClientesRequest.Correo
            };
            _context.Add(oCliente);
            _context.SaveChanges();


        }

        public void Delete(long Id)
        {

            Cliente? oCliente = _context.Clientes.Find(Id);
            if (oCliente != null)
            {
                _context.Remove(oCliente);
                _context.SaveChanges();
            }

        }

        public void Edit(ClientesRequest oClienteRequest)
        {

            Cliente? oCliente = _context.Clientes.Find(oClienteRequest.Id);
            if (oCliente != null)
            {
                oCliente.Nombre = oClienteRequest.Nombre;
                oCliente.Apellido = oClienteRequest.Apellido;
                oCliente.Dni = oClienteRequest.Dni;
                oCliente.Correo = oClienteRequest.Correo;
                _context.Entry(oCliente).State = EntityState.Modified;
                _context.SaveChanges();
            }



        }

        public IEnumerable<Cliente> FiltrarClientes(string searchTerm, int limite)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new Exception("La busqueda no puede ser vacia");
            }

            List<Cliente> oClientes = new();


            oClientes = _context.Clientes
                   .Where(p => EF.Functions.Like(p.Nombre, $"%{searchTerm}%") ||
                               EF.Functions.Like(p.Apellido, $"%{searchTerm}%") ||
                               EF.Functions.Like(p.Dni, $"%{searchTerm}%"))
                   .Take(limite)
                   .ToList();
            if (oClientes.Count != 0)
            {
                return oClientes;
            }
            else
            {
                throw new Exception("Cliente no encontrado");
            }




        }

        public Cliente Get(long id)
        {
            Cliente? oCliente = new();


            oCliente = _context.Clientes.FirstOrDefault();
            if (oCliente == null)
            {
                throw new Exception("Cliente no encontrado");
            }


            return oCliente;

        }

        public IEnumerable<Cliente> GetAll()
        {
            List<Cliente> oClientes = new();

            oClientes = _context.Clientes.OrderByDescending(d => d.Id).ToList();
            if (oClientes.Count != 0)
            {
                return oClientes;
            }
            else
            {
                throw new Exception("No se encontraron clientes");
            }




        }

        public (IEnumerable<Cliente> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado)
    {
        List<Cliente> oClientes = new();

        var totalElements = _context.Clientes.Count(); // Obtener el total de elementos

        oClientes = _context.Clientes.OrderByDescending(d => d.Id)
            .Skip((oParametrosPaginado.PageIndex) * oParametrosPaginado.ItemsPerPage)
            .Take(oParametrosPaginado.ItemsPerPage).ToList();

        if (oClientes.Count != 0)
        {
            return (oClientes, totalElements);
        }
        else
        {
            throw new Exception("No se encontraron clientes");
        }
    }
    }
}
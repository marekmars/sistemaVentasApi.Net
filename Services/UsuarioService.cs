using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web_Service_.Net_Core.Controllers;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Common;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;
using Web_Service_.Net_Core.Tools;

namespace Web_Service_.Net_Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppSetting _appSettings;
        private readonly DBContext _context;

        public UsuarioService(IOptions<AppSetting> appSetings, DBContext dBContext)
        {
            _appSettings = appSetings.Value;
            _context = dBContext;
        }
        public UserResponse Auth(AuthRequest oModel)
        {
            UserResponse userResponse = new();
            try
            {

                string encryptPassword = Encrypt.GetSHA256(oModel.Clave);
                var usuario = _context.Usuarios.Where(user => user.Correo == oModel.User && user.Clave == encryptPassword).FirstOrDefault();
                if (usuario != null)
                {
                    userResponse.Correo = usuario.Correo;
                    userResponse.Rol = getRol(usuario);

                    userResponse.Token = GetToken(usuario);
                }
                else
                {
                    userResponse = null;
                }

            }
            catch (System.Exception)
            {

                throw;
            }
            return userResponse;

        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            string? rol = "";

            rol = getRol(usuario);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                 new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                 new(ClaimTypes.Email, usuario.Correo),
                 new(ClaimTypes.Role, rol),
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string? getRol(Usuario usuario)
        {
            string? rol;
            return rol = _context.Rols.Where(x => x.Id == usuario.IdRol).Select(x => x.Nombre).FirstOrDefault();

        }

        public List<Usuario> GetAll()
        {
            List<Usuario>? usuarios = [.. _context.Usuarios.OrderBy(x => x.Apellido)];

            if (usuarios.Count > 0)
            {
                return usuarios;

            }
            else
            {
                throw new Exception("No se encontraron productos");
            }

        }

        public void Delete(int id)
        {
            Usuario? oUsuario = _context.Usuarios.Find(id);
            if (oUsuario != null)
            {
                _context.Remove(oUsuario);
                _context.SaveChanges();
            }
        }

        public void Edit(UsuarioRequest oUsuarioRequest)
        {

Console.WriteLine("Editando usuario: "+oUsuarioRequest.IdRol);
            Usuario? oUsuario = _context.Usuarios.Find(oUsuarioRequest.Id);
            if (oUsuario != null)
            {

                oUsuario.IdRol = oUsuarioRequest.IdRol != 0 ? oUsuarioRequest.IdRol : oUsuario.IdRol;
                oUsuario.Nombre = !string.IsNullOrEmpty(oUsuarioRequest.Nombre) ? oUsuarioRequest.Nombre : oUsuario.Nombre;
                oUsuario.Apellido = !string.IsNullOrEmpty(oUsuarioRequest.Apellido) ? oUsuarioRequest.Apellido : oUsuario.Apellido;
                oUsuario.Dni = !string.IsNullOrEmpty(oUsuarioRequest.Dni) ? oUsuarioRequest.Dni : oUsuario.Dni;
                oUsuario.Correo = !string.IsNullOrEmpty(oUsuarioRequest.Correo) ? oUsuarioRequest.Correo : oUsuario.Correo;
                oUsuario.Clave = oUsuario.Clave;

                if (oUsuarioRequest.Clave != null
                && oUsuarioRequest.Clave.Trim() != ""
                && Encrypt.GetSHA256(oUsuarioRequest.Clave) != oUsuario.Clave)
                {
                    oUsuario.Clave = Encrypt.GetSHA256(oUsuarioRequest.Clave);
                }
            }
            _context.Entry(oUsuario).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Add(UsuarioRequest oUsuarioRequest)
        {
            Usuario oUsuario = new()
            {
                Nombre = oUsuarioRequest.Nombre,
                IdRol = oUsuarioRequest.IdRol,
                Apellido = oUsuarioRequest.Apellido,
                Dni = oUsuarioRequest.Dni,
                Correo = oUsuarioRequest.Correo,
                Clave = Encrypt.GetSHA256(oUsuarioRequest.Clave)
            };
            _context.Add(oUsuario);
            _context.SaveChanges();
        }

        public (IEnumerable<Usuario> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado)
        {

            List<Usuario> oClientes = new();

            var totalElements = _context.Clientes.Count(); // Obtener el total de elementos

            oClientes = _context.Usuarios.OrderByDescending(d => d.Id)
            .Include(x => x.Rol)
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



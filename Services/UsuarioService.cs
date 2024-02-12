using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web_Service_.Net_Core.Controllers;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
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
        public UserResponse Authenticate(AuthRequest oAuthRequest)
        {
            UserResponse userResponse = new();
            if (string.IsNullOrEmpty(oAuthRequest.User) || string.IsNullOrEmpty(oAuthRequest.Clave))
            {
                throw new Exception("Falta el usuario o la clave");
            }

            string encryptPassword = Encrypt.GetSHA256(oAuthRequest.Clave);
            Usuario? oUsuario = _context.Usuarios.Where(user => user.Correo == oAuthRequest.User
            && user.Clave == encryptPassword
            && user.Estado == true).FirstOrDefault();

            if (oUsuario == null)
            {
                throw new Exception("Credenciales incorrectas");
            }
            userResponse.Correo = oUsuario.Correo;
            userResponse.Rol = GetRol(oUsuario);
            userResponse.Token = GetToken(oUsuario);
            return userResponse;
        }

        private string GetToken(Usuario usuario)
        {
            if (usuario == null) throw new("Usuario no encontrado");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            string? rol = GetRol(usuario);
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

        private string GetRol(Usuario usuario)
        {
            if (usuario == null) throw new("Usuario no encontrado");
            return _context.Rols.Where(x => x.Id == usuario.IdRol).Select(x => x.Nombre).FirstOrDefault();
        }


        public ApiResponse<Usuario> GetUsuarios(QueryParameters queryParameters)
        {
            IQueryable<Usuario> query = _context.Usuarios.Include(u => u.Rol);
            query = query.Where(u => u.Estado == true);
            var totalElements = _context.Clientes.Count();


            query = query.Where(p => p.Estado == true);

            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "name" => query.OrderBy(u => u.Nombre),
                    "lastname" => query.OrderBy(u => u.Apellido),
                    "dni" => query.OrderBy(u => u.Dni),
                    "rol" => query.OrderBy(u => u.Rol.Nombre),
                    "mail" => query.OrderBy(u => u.Correo),
                    _ => query.OrderBy(u => u.Id),
                };
                if (queryParameters.Desc)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
            }



            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                string[] filters = filter.Split(' ');

                query = query.AsEnumerable().Where(u =>
                    filters.All(f =>
                        u.Nombre.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        u.Apellido.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        u.Dni.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        u.Rol.Nombre.Contains(f, StringComparison.CurrentCultureIgnoreCase)
                    )
                ).AsQueryable();
            }

            if (queryParameters.Skip.HasValue)
            {
                query = query.Skip(queryParameters.Skip.Value);
            }

            if (queryParameters.Limit.HasValue)
            {
                query = query.Take(queryParameters.Limit.Value);
            }

            var usuarios = query.ToList();

            if (usuarios.Count == 0) throw new Exception("No se encontraron usuarios");

            return new ApiResponse<Usuario>
            {
                Success = 1,
                Message = "Usuarios obtenidos correctamente",
                Data = usuarios,
                TotalCount = totalElements
            };
        }

        public ApiResponse<Usuario> DeleteUsuario(long Id)
        {
            Usuario? oUsuario = _context.Usuarios.Find(Id) ?? throw new Exception("No se encontro el usuario");
            oUsuario.Estado = false;
            _context.Entry(oUsuario).State = EntityState.Modified;
            _context.SaveChanges();

            return new ApiResponse<Usuario>
            {
                Success = 1,
                Message = "Usuario eliminado correctamente",
                Data = [oUsuario],
                TotalCount = 1
            };
        }

        public ApiResponse<Usuario> UpdateUsuario(UsuarioRequest oUsuarioRequest)
        {
            Console.WriteLine("Editando usuario: " + oUsuarioRequest.IdRol);
            Usuario? oUsuario = _context.Usuarios
                               .Include(u => u.Rol)
                               .Where(c => c.Id == oUsuarioRequest.Id && c.Estado == true)
                               .FirstOrDefault()
                               ?? throw new Exception("No se encontro un usuario activo con ese ID");



            oUsuario.IdRol = oUsuarioRequest.IdRol != 0 ? oUsuarioRequest.IdRol : oUsuario.IdRol;
            oUsuario.Nombre = !string.IsNullOrEmpty(oUsuarioRequest.Nombre) ? oUsuarioRequest.Nombre : oUsuario.Nombre;
            oUsuario.Apellido = !string.IsNullOrEmpty(oUsuarioRequest.Apellido) ? oUsuarioRequest.Apellido : oUsuario.Apellido;
            oUsuario.Dni = !string.IsNullOrEmpty(oUsuarioRequest.Dni) ? oUsuarioRequest.Dni : oUsuario.Dni;
            oUsuario.Correo = !string.IsNullOrEmpty(oUsuarioRequest.Correo) ? oUsuarioRequest.Correo : oUsuario.Correo;
            oUsuario.Estado = true;
            oUsuario.Clave = oUsuario.Clave;

            if (oUsuarioRequest.Clave != null
            && oUsuarioRequest.Clave.Trim() != ""
            && Encrypt.GetSHA256(oUsuarioRequest.Clave) != oUsuario.Clave)
            {
                oUsuario.Clave = Encrypt.GetSHA256(oUsuarioRequest.Clave);
            }

            _context.Entry(oUsuario).State = EntityState.Modified;
            _context.SaveChanges();
            return new ApiResponse<Usuario>
            {
                Success = 1,
                Message = "Usuario actualizado correctamente",
                Data = [oUsuario],
                TotalCount = 1
            };

        }


        public ApiResponse<Usuario> AddUsuario(UsuarioRequest oUsuarioRequest)
        {
            Usuario oUsuario = new()
            {
                Nombre = oUsuarioRequest.Nombre,
                Apellido = oUsuarioRequest.Apellido,
                Dni = oUsuarioRequest.Dni,
                Correo = oUsuarioRequest.Correo,
                IdRol = oUsuarioRequest.IdRol,
                Clave = Encrypt.GetSHA256(oUsuarioRequest.Clave),
                Estado = true,
            };
            _context.Add(oUsuario);
            _context.SaveChanges();

            return new ApiResponse<Usuario>
            {
                Success = 1,
                Message = "Usuario creado correctamente",
                Data = [oUsuario],
                TotalCount = 1
            };
        }

        public ApiResponse<Usuario> GetUsuario(long id)
        {
            var oUsuario = _context.Usuarios
               .Include(u => u.Rol)
               .FirstOrDefault(u => u.Id == id);

            if (oUsuario == null)
            {
                throw new Exception("No se encontro el cliente");
            }

            return new ApiResponse<Usuario>
            {
                Success = 1,
                Message = "Usuario obtenido correctamente",
                Data = [oUsuario],
                TotalCount = 1
            };
        }

        public ApiResponse<Usuario> CorreoExiste(string correo)
        {
            Usuario? oUsuario = _context.Usuarios.Where(u => u.Correo == correo && u.Estado == true).FirstOrDefault();


            if (oUsuario == null)
            {
                return new ApiResponse<Usuario>
                {
                    Success = 1,
                    Message = "Correo válido",
                    Data = [],
                    TotalCount = 1
                };
            }
            return new ApiResponse<Usuario>
            {
                Success = 0,
                Message = "El correo ya existe",
                Data = [oUsuario],
                TotalCount = 1
            };
        }

        public ApiResponse<Usuario> FullDeleteUsuario(long Id)
        {
            Usuario? oUsuario = _context.Usuarios.Find(Id) ?? throw new Exception("No se encontro el usuario");
            _context.Remove(oUsuario);
            _context.SaveChanges();

            return new ApiResponse<Usuario>
            {
                Success = 1,
                Message = "Usuario eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }
    }
}



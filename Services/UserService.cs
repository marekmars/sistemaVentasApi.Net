using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web_Service_.Net_Core.Controllers;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Common;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Tools;

namespace Web_Service_.Net_Core.Services
{
    public class UserService : IUserService
    {
        private readonly AppSetting _appSettings;
        private readonly DBContext _context;

        public UserService(IOptions<AppSetting> appSetings, DBContext dBContext)
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
                    // userResponse.Rol =  getRol(usuario);

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
    }

}

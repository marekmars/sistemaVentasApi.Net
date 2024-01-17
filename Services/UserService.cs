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
        private readonly AppSetting? _appSettings;

        public UserService(IOptions<AppSetting> appSetings)
        {
            _appSettings = appSetings.Value;
        }
        public UserResponse Auth(AuthRequest oModel)
        {
            UserResponse userResponse = new();
            try
            {
                using (var db = new DBContext())
                {
                    string encryptPassword = Encrypt.GetSHA256(oModel.Clave);
                    var usuario = db.Usuarios.Where(user => user.Correo == oModel.User && user.Clave == encryptPassword).FirstOrDefault();
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
            var rol = "";
            using (DBContext db = new())
            {
                rol = getRol(usuario);
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
              new Claim(ClaimTypes.Email, usuario.Correo),
              new Claim(ClaimTypes.Role, rol),
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string getRol(Usuario usuario)
        {
            string rol;
            using (DBContext db = new())
            {
               return rol = db.Rols.Where(x => x.Id == usuario.IdRol).Select(x => x.Nombre).FirstOrDefault();
            };
            
        }
    }

}

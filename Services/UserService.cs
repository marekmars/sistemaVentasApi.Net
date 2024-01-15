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
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret); // Assuming _appSettings.Secret is a string containing your secret key

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
              new Claim(ClaimTypes.Email, usuario.Correo),
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}

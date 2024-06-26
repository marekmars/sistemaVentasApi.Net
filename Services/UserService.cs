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

using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;
using Web_Service_.Net_Core.Tools;

namespace Web_Service_.Net_Core.Services
{

    public class UserService : IUserService

    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;


        public UserService(IConfiguration configuration, DataContext dBContext)
        {
            _configuration = configuration;
            _context = dBContext;
        }
        public UserResponse Authenticate(AuthRequest oAuthRequest)
        {
            UserResponse userResponse = new();
            if (string.IsNullOrEmpty(oAuthRequest.User) || string.IsNullOrEmpty(oAuthRequest.Password))
            {
                throw new Exception("Falta el usuario o la clave");
            }

            string encryptPassword = Encrypt.GetSHA256(oAuthRequest.Password);
            User? oUser = _context.Users.Where(user => user.Mail == oAuthRequest.User
            && user.Password == encryptPassword
            && user.State == 1).FirstOrDefault();

            if (oUser == null)
            {
                throw new Exception("Credenciales incorrectas");
            }
            // userResponse.Mail = oUser.Mail;
            // userResponse.Role = GetRole(oUser);
            userResponse.Token = GetToken(oUser);
            return userResponse;
        }

        private string GetToken(User usuario)
        {
            if (usuario == null) throw new("User no encontrado");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);

            string? rol = GetRole(usuario);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                 new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                 new(ClaimTypes.Email, usuario.Mail),
                 new(ClaimTypes.Role, rol),
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GetRole(User usuario)
        {
            if (usuario == null) throw new("User no encontrado");
            return _context.Roles.Where(x => x.Id == usuario.IdRole).Select(x => x.Name).FirstOrDefault();
        }

        public ApiResponse<User> GetCurrentUser(string token)
        {

            string tokenString = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(tokenString);
            Console.WriteLine("Token: " + tokenString);
            var id = jsonToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            var user = _context.Users
            .Where(u => u.Id.ToString() == id.Value)
            .Include(u => u.Role)
            .Include(u => u.Avatar)
            .FirstOrDefault();
            // var user = _context.Users.Where(u => u.Id.ToString() == jsonToken.Claims.NameIdentifier).FirstOrDefault();
            return new ApiResponse<User>
            {
                Success = 1,
                Message = "Token correcto",
                Data = [user],
                TotalCount = 1
            };
        }

        public ApiResponse<User> GetUsers(QueryParameters queryParameters)
        {
            IQueryable<User> query = _context.Users
            .Include(u => u.Role)
            .Include(u => u.Avatar)
            .Where(u => u.State == 1);


            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                string[] filters = filter.Split(' ');

                query = query.AsEnumerable().Where(u =>
                    filters.All(f =>
                        u.Name.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        u.LastName.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        u.IdCard.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                        u.Role.Name.Contains(f, StringComparison.CurrentCultureIgnoreCase)
                    )
                ).AsQueryable();
            }

            var totalElements = query.Count();

            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "id" => query.OrderBy(u => u.Id),
                    "name" => query.OrderBy(u => u.Name),
                    "lastname" => query.OrderBy(u => u.LastName),
                    "idcard" => query.OrderBy(u => u.IdCard),
                    "role" => query.OrderBy(u => u.Role.Name),
                    "mail" => query.OrderBy(u => u.Mail),
                    _ => query.OrderBy(u => u.Id),
                };
                if (queryParameters.Desc == 1)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
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

            usuarios.ForEach(u => u.Password = "");
            return new ApiResponse<User>
            {
                Success = 1,
                Message = "Users obtenidos correctamente",
                Data = usuarios,
                TotalCount = totalElements
            };
        }

        public ApiResponse<User> DeleteUser(long Id)
        {
            Console.WriteLine("Eliminando usuario: " + Id);

            // Buscar el usuario en la base de datos
            User oUser = _context.Users.Where(u => u.Id == Id).FirstOrDefault();
            Console.WriteLine("Deleting user " + oUser.Name);
            // Verificar si el usuario existe
            if (oUser == null)
            {
                throw new Exception("No se encontró el usuario con el ID proporcionado.");
            }

            // Cambiar el estado del usuario
            oUser.State = 0;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            // Borrar la contraseña del usuario para no enviarla en la respuesta
            oUser.Password = "";

            Console.WriteLine("El estado final es: " + oUser.State);

            // Devolver una respuesta exitosa
            return new ApiResponse<User>
            {
                Success = 1,
                Message = "Usuario eliminado correctamente",
                Data = new List<User> { oUser },
                TotalCount = 1
            };
        }

        public ApiResponse<User> UpdateUser(UserRequest oUserRequest)
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine("Editando usuario: " + oUserRequest.Avatar.Url);
            Console.WriteLine("=====================================================");
            User? oUser = _context.Users
                               .Include(u => u.Role)
                               .Where(c => c.Id == oUserRequest.Id && c.State == 1)
                               .FirstOrDefault()
                               ?? throw new Exception("No se encontro un usuario activo con ese ID");

            oUser.IdRole = oUserRequest.IdRole != 0 ? oUserRequest.IdRole : oUser.IdRole;
            oUser.Name = !string.IsNullOrEmpty(oUserRequest.Name) ? oUserRequest.Name : oUser.Name;
            oUser.LastName = !string.IsNullOrEmpty(oUserRequest.LastName) ? oUserRequest.LastName : oUser.LastName;
            oUser.IdCard = !string.IsNullOrEmpty(oUserRequest.IdCard) ? oUserRequest.IdCard : oUser.IdCard;
            oUser.Mail = !string.IsNullOrEmpty(oUserRequest.Mail) ? oUserRequest.Mail : oUser.Mail;
            oUser.State = 1;
            oUser.Password = oUser.Password;

            if (oUserRequest.Password != null
            && oUserRequest.Password.Trim() != ""
            && Encrypt.GetSHA256(oUserRequest.Password) != oUser.Password)
            {
                oUser.Password = Encrypt.GetSHA256(oUserRequest.Password);
            }

            _context.Entry(oUser).State = EntityState.Modified;
            _context.SaveChanges();


            if (oUserRequest.Avatar != null)
            {
                var imageEntity = new Image
                {
                    DeleteHash = oUserRequest.Avatar.DeleteHash,
                    Name = oUserRequest.Avatar.Name,
                    Url = oUserRequest.Avatar.Url,
                    IdUser = oUser.Id  // Set the foreign key to the Id of the newly created User
                };

                // Add the Image entity to the context
                _context.Images.RemoveRange(_context.Images.Where(i => i.IdUser == oUser.Id));
                _context.Images.Add(imageEntity);

                _context.SaveChanges();
            }

            oUser.Password = "";
            return new ApiResponse<User>
            {
                Success = 1,
                Message = "User actualizado correctamente",
                Data = [oUser],
                TotalCount = 1
            };

        }


        public ApiResponse<User> AddUser(UserRequest oUserRequest)
        {
            User oUser = new()
            {
                Name = oUserRequest.Name,
                LastName = oUserRequest.LastName,
                IdCard = oUserRequest.IdCard,
                Mail = oUserRequest.Mail,
                IdRole = oUserRequest.IdRole,
                Password = Encrypt.GetSHA256(oUserRequest.Password),
                State = 1,
            };
            _context.Add(oUser);
            _context.SaveChanges();
            if (oUserRequest.Avatar != null)
            {
                var imageEntity = new Image
                {
                    DeleteHash = oUserRequest.Avatar.DeleteHash,
                    Name = oUserRequest.Avatar.Name,
                    Url = oUserRequest.Avatar.Url,
                    IdUser = oUser.Id  // Set the foreign key to the Id of the newly created User
                };

                // Add the Image entity to the context
                _context.Images.Add(imageEntity);
                _context.SaveChanges();
            }

            oUser.Password = "";

            return new ApiResponse<User>
            {
                Success = 1,
                Message = "User creado correctamente",
                Data = [oUser],
                TotalCount = 1
            };
        }

        public ApiResponse<User> GetUser(long id)
        {
            var oUser = _context.Users
               .Include(u => u.Role)
               .Include(u => u.Avatar)
               .FirstOrDefault(u => u.Id == id);

            if (oUser == null)
            {
                throw new Exception("No se encontro el cliente");
            }
            oUser.Password = "";
            return new ApiResponse<User>
            {
                Success = 1,
                Message = "User obtenido correctamente",
                Data = [oUser],
                TotalCount = 1
            };
        }

        public ApiResponse<User> MailExist(string correo)
        {
            User? oUser = _context.Users.Where(u => u.Mail == correo && u.State == 1).FirstOrDefault();


            if (oUser == null)
            {
                return new ApiResponse<User>
                {
                    Success = 1,
                    Message = "Mail válido",
                    Data = [],
                    TotalCount = 1
                };
            }
            return new ApiResponse<User>
            {
                Success = 0,
                Message = "El correo ya existe",
                Data = [oUser],
                TotalCount = 1
            };
        }

        public ApiResponse<User> FullDeleteUser(long Id)
        {
            User? oUser = _context.Users.Find(Id) ?? throw new Exception("No se encontro el usuario");
            _context.Remove(oUser);
            _context.SaveChanges();

            return new ApiResponse<User>
            {
                Success = 1,
                Message = "User eliminado correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<User> UpdateProfile(string token, UserRequest oUserRequest)
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine("Editando usuario: " + oUserRequest.Avatar.Url);
            Console.WriteLine("=====================================================");
            User? oUser = this.GetCurrentUser(token).Data.FirstOrDefault();

            oUser.Name = !string.IsNullOrEmpty(oUserRequest.Name) ? oUserRequest.Name : oUser.Name;
            oUser.LastName = !string.IsNullOrEmpty(oUserRequest.LastName) ? oUserRequest.LastName : oUser.LastName;
            oUser.IdCard = !string.IsNullOrEmpty(oUserRequest.IdCard) ? oUserRequest.IdCard : oUser.IdCard;
            oUser.Mail = !string.IsNullOrEmpty(oUserRequest.Mail) ? oUserRequest.Mail : oUser.Mail;
            oUser.State = 1;
            oUser.Password = oUser.Password;

            if (oUserRequest.Password != null
            && oUserRequest.Password.Trim() != ""
            && Encrypt.GetSHA256(oUserRequest.Password) != oUser.Password)
            {
                oUser.Password = Encrypt.GetSHA256(oUserRequest.Password);
            }

            _context.Entry(oUser).State = EntityState.Modified;
            _context.SaveChanges();


            if (oUserRequest.Avatar != null)
            {
                var imageEntity = new Image
                {
                    DeleteHash = oUserRequest.Avatar.DeleteHash,
                    Name = oUserRequest.Avatar.Name,
                    Url = oUserRequest.Avatar.Url,
                    IdUser = oUser.Id  // Set the foreign key to the Id of the newly created User
                };

                // Add the Image entity to the context
                _context.Images.RemoveRange(_context.Images.Where(i => i.IdUser == oUser.Id));
                _context.Images.Add(imageEntity);

                _context.SaveChanges();
            }

            oUser.Password = "";
            return new ApiResponse<User>
            {
                Success = 1,
                Message = "User actualizado correctamente",
                Data = [oUser],
                TotalCount = 1
            };

        }

    }
}



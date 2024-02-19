using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public interface IUserService
    {

        public ApiResponse<User> GetUsers(QueryParameters queryParameters);
        public ApiResponse<User> GetUser(long id);
        public ApiResponse<User> AddUser(UserRequest oUserRequest);
        public ApiResponse<User> UpdateUser(UserRequest oUserRequest);
        public ApiResponse<User> DeleteUser(long Id);
        public UserResponse Authenticate(AuthRequest oAuthRequest);
        public ApiResponse<User> MailExist(string correo);
        public ApiResponse<User> FullDeleteUser(long Id);

    }
}
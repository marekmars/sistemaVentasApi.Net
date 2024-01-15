using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;

namespace Web_Service_.Net_Core.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest oModel);
    }
}
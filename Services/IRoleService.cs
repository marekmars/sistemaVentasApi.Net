using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;

namespace Web_Service_.Net_Core.Services
{
    public interface IRoleService
    {
        public List<Role> GetRoles(); 
        
    }
}
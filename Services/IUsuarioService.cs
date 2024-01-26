using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public interface IUsuarioService
    {
        public void Add(UsuarioRequest oUsuarioRequest);
        public UserResponse Auth(AuthRequest oModel);
        public List<Usuario> GetAll();
        public void Delete(int id);
        public void Edit(UsuarioRequest oModel);
        public (IEnumerable<Usuario> Data, int TotalElements) GetAllP(ParametrosPaginado oParametrosPaginado);
        
            
    }
}
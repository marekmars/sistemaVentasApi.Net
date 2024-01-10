using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly DBContext _context = new DBContext();
       
        // [HttpGet("Get")]
        // public IActionResult Get()
        // {
       
        // }
    }
}
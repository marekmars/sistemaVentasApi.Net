using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.ApiResponse
{
    public class ApiResponse<T>
    {
        public int Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<T>? Data { get; set; }
        public int TotalCount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Response
{
    public class Response
    {
        public int Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }


        public Response()
        {
            Success = 0;

        }
    }
}
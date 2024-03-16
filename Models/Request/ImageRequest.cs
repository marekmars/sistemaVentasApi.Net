using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Request
{
  public class ImageRequest
    {
        public string DeleteHash {get;set;} ="";
        public string? Name { get; set; }
        public string? Url { get; set; }

    }
}
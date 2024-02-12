using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models.Tools
{
    public class QueryParameters
    {
        public string? Filter { get; set; }
        public int? Limit { get; set; }
        public int? Skip { get; set; }
        public string? OrderBy { get; set; }
        public bool Desc { get; set; }
    }
}
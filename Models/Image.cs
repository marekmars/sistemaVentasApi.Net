using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web_Service_.Net_Core.Models
{
    public class Image
    {
        public long Id { get; set; }
        public string DeleteHash { get; set; } = "";
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        [JsonIgnore]
        [ForeignKey("IdProduct")]
        public Product? Product { get; set; }
        [JsonIgnore]
        public long? IdProduct { get; set; }
        [JsonIgnore]
        public int? IdUser { get; set; }
        [JsonIgnore]
        [ForeignKey("IdUser")]
        public User? User { get; set; }

    }
}
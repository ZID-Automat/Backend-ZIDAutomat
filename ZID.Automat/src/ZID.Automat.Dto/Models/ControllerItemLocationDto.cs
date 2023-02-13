using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models
{
    public record ControllerItemLocationDto { 
        public int ItemId { get; set; }
        public string location { get; set; } = string.Empty;
    }
}

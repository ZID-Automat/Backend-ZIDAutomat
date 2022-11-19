using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models
{
    public record ItemDetailedDto
    {
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool Available { get; set; }
        public decimal Price { get; set; }
        public string Categorie { get; set; } = string.Empty;
    }
}

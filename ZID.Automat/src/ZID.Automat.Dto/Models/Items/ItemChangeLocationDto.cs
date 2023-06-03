using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models.Items
{
    public class ItemChangeLocationDto
    {
        public IEnumerable<ItemLocationDto> ItemLocations { get; set; } = null!;
    }

    public class ItemLocationDto
    {
        public string Location { get; set; }
        public int Id { get; set; }
    }
}

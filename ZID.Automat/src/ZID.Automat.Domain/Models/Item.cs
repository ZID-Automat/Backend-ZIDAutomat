using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Interfaces;

namespace ZID.Automat.Domain.Models
{   
    public class Item : HasName
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }

        //noch besser machen(historisierung)
        public string LocationImAutomat { get; set; } = string.Empty;

        public virtual Categorie Categorie { get; set; } = default!;
        public int CategorieId { get; set; }

        public virtual List<ItemInstance> ItemInstances { get; set; } = new List<ItemInstance>();
    }
}

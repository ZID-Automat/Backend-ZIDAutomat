using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Interfaces;

namespace ZID.Automat.Domain.Models
{
    public class Item:HasName
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }

        //noch besser machen(historisierung)
        public string LocationImAutomat { get; set; } = string.Empty;

        public Categorie Categorie = default!;
        public int CategorieId;

        private List<ItemInstance> _ItemInstances { get; set; } = new List<ItemInstance>();
        public IReadOnlyList<ItemInstance> ItemInstances => _ItemInstances;

        private List<Borrow> _borrows { get; set; } = new List<Borrow>();
        public IReadOnlyList<Borrow> Borrows => _borrows;
    }
}

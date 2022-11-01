using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        private List<Item> _Items { get; set; } = new List<Item>();
        public IReadOnlyList<Item> Items => _Items;

        public void AddItemToCategorie(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Can't add new Item to Categorie, because it is null");
            }
            _Items.Add(item);
        }
    }
}

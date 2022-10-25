using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Price { get; set; }

        private List<ItemInstance> _ItemInstances { get; set; } = new List<ItemInstance>();
        public IReadOnlyList<ItemInstance> ItemInstances => _ItemInstances;


        /// <summary>
        /// Adds a new ItemInstance to the Item
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddItemInstance(ItemInstance itemInstance)
        {
            if(itemInstance == null)
            {
                throw new ArgumentNullException();
            }
            if (Math.Abs((DateTime.Now - itemInstance.FirstAdded).TotalHours) > 1)
            {
                throw new ArgumentException("older than 1 hour");
            }

            _ItemInstances.Add(itemInstance);
        }
        
    }
}
